using System;
using System.Collections.Generic;
using System.Linq;

namespace Curupira2D.ECS
{
    internal sealed class SceneManager : IDisposable
    {
        readonly List<Scene> _scenes = new List<Scene>();

        public Scene CurrentScene { get; private set; }
        public IReadOnlyList<Scene> Scenes => _scenes;

        public Scene Set(GameCore gameCore, Scene scene)
        {
            // Clean current scene before change
            CurrentScene?.RemoveAllSystems();
            CurrentScene?.RemoveAllEntities();
            CurrentScene?.RemoveAllGameComponents();

            CurrentScene = scene;
            CurrentScene.SetGameCore(gameCore);
            CurrentScene.LoadContent();

            return CurrentScene;
        }

        public TScene Set<TScene>(GameCore gameCore, params object[] args) where TScene : Scene
        {
            var scene = (TScene)Activator.CreateInstance(typeof(TScene), args);
            return Set(gameCore, scene) as TScene;
        }

        public void Add(Scene scene)
        {
            if (_scenes.Any(_ => _.GetType() == scene.GetType()))
                return;

            _scenes.Add(scene);
        }

        public void Add<TScene>(params object[] args) where TScene : Scene
        {
            var scene = (TScene)Activator.CreateInstance(typeof(TScene), args);
            Add(scene);
        }

        public void Change<TScene>(GameCore gameCore) where TScene : Scene
        {
            if (!_scenes.Any() || !_scenes.Any(_ => _.GetType() == typeof(TScene)))
                throw new Exception($"Scene ({typeof(TScene).Name}) not register with method {nameof(GameCore.AddScene)}!");

            var scene = _scenes.OfType<TScene>().FirstOrDefault();
            Set(gameCore, scene);
        }

        public void Dispose()
        {
            _scenes.Clear();
            CurrentScene.Dispose();

            GC.Collect();
        }
    }
}
