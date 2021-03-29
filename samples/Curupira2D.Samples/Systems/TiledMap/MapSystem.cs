﻿using Curupira2D.ECS.Systems;
using Curupira2D.Extensions;

namespace Curupira2D.Testbed.Systems.TiledMap
{
    class MapSystem : ECS.System, IInitializable
    {
        public void Initialize()
        {
            var tiledMapComponent = Scene.GameCore.Content.CreateTiledMapComponent("TiledMap/PlatformerTiledMap.tmx", "TiledMap/PlatformerTileset");

            Scene.CreateEntity("tiledmap")
                .AddComponent(tiledMapComponent);
        }
    }
}
