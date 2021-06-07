using System;
using System.Collections.Generic;
using System.Linq;

namespace Curupira2D.ECS
{
    internal sealed class SceneManager : IDisposable
    {
        readonly List<Scene> _scenes = new List<Scene>();
        static readonly Lazy<SceneManager> _sceneManager = new Lazy<SceneManager>(() => new SceneManager());

        SceneManager() { }

        public static SceneManager Instance => _sceneManager.Value;
        public Scene CurrentScene { get; private set; }
        public IReadOnlyList<Scene> Scenes => _scenes;

        public Scene Set(GameCore gameCore, Scene scene)
        {
            // Clean current scene before change
            CurrentScene?.RemoveAllSystems();
            CurrentScene?.RemoveAllEntities();

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
                return;

            var scene = _scenes.OfType<TScene>().FirstOrDefault();
            Set(gameCore, scene);
        }

        public void Dispose()
        {
            _scenes.Clear();
            _sceneManager.Value.Dispose();
            CurrentScene.Dispose();

            GC.Collect();
        }
    }
}
