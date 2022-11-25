using System;
using System.Collections.Generic;

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
            CurrentScene?.UnloadContent();

            // Set new current scene
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

        public void Dispose()
        {
            _scenes.Clear();
            CurrentScene.Dispose();

            GC.Collect();
        }
    }
}
