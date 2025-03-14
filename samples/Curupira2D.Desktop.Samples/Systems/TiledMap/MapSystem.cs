using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;

namespace Curupira2D.Desktop.Samples.Systems.TiledMap
{
    [RequiredComponent(typeof(MapSystem), typeof(TiledMapComponent))]
    class MapSystem(string tiledMapRelativePath, string tilesetRelativePath = null) : ECS.System, ILoadable
    {
        public TiledMapComponent TiledMapComponent { get; private set; }

        public void LoadContent()
        {
            TiledMapComponent = Scene.GameCore.Content.CreateTiledMapComponent(tiledMapRelativePath, tilesetRelativePath);
            Scene.CreateEntity("tiledmap", default).AddComponent(TiledMapComponent);
        }
    }
}
