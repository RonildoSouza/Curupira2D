using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems;
using System.IO;
using TiledLib;

namespace Helper.TiledMap.Systems
{
    public class MapSystem : MonoGame.Helper.ECS.System, IInitializable
    {
        public void Initialize()
        {
            Map map;
            var mapFilePath = Path.Combine(Scene.GameCore.Content.RootDirectory, "PlatformerTiledMap.tmx");
            var tilesetTexture = Scene.GameCore.Content.Load<Texture2D>("PlatformerTileset");

            using (var stream = File.OpenRead(mapFilePath))
                map = Map.FromStream(stream);

            Scene.CreateEntity("tiledmap")
                .AddComponent(new TiledMapComponent(map, tilesetTexture));
        }
    }
}
