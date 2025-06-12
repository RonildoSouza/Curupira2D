using System;
using System.Collections.Generic;

namespace Curupira2D.ECS
{
    internal sealed class SceneManager : IDisposable
    {
        readonly List<Scene> _scenes = [];
        bool _disposed = false;

        ~SceneManager() => Dispose(disposing: false);

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
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _scenes.Clear();
                CurrentScene.Dispose();
            }

            _disposed = true;
        }
    }
}
