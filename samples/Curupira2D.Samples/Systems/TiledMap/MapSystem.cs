using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.Extensions;

namespace Curupira2D.Samples.Systems.TiledMap
{
    class MapSystem : ECS.System, ILoadable
    {
        private readonly string _tiledMapRelativePath;
        private readonly string _tilesetRelativePath;

        public MapSystem(string tiledMapRelativePath, string tilesetRelativePath = null)
        {
            _tiledMapRelativePath = tiledMapRelativePath;
            _tilesetRelativePath = tilesetRelativePath;
        }

        public TiledMapComponent TiledMapComponent { get; private set; }

        public void LoadContent()
        {
            TiledMapComponent = Scene.GameCore.Content.CreateTiledMapComponent(_tiledMapRelativePath, _tilesetRelativePath);
            Scene.CreateEntity("tiledmap", default).AddComponent(TiledMapComponent);
        }
    }
}
