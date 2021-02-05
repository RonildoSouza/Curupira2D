using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems;
using System.IO;
using TiledLib;

namespace MonoGame.Helper.Samples.Systems.TiledMap
{
    class MapSystem : ECS.System, IInitializable
    {
        public void Initialize()
        {
            Map map;
            var mapFilePath = Path.Combine(Scene.GameCore.Content.RootDirectory, "TiledMap", "PlatformerTiledMap.tmx");
            var tilesetTexture = Scene.GameCore.Content.Load<Texture2D>("TiledMap/PlatformerTileset");

            using (var stream = File.OpenRead(mapFilePath))
                map = Map.FromStream(stream);

            Scene.CreateEntity("tiledmap")
                .AddComponent(new TiledMapComponent(map, tilesetTexture));
        }
    }
}
