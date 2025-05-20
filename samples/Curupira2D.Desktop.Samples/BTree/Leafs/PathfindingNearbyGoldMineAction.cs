using Curupira2D.AI.BehaviorTree;
using Curupira2D.AI.BehaviorTree.Leafs;
using Curupira2D.AI.Extensions;
using Curupira2D.AI.Pathfinding.AStar;
using Curupira2D.AI.Pathfinding.Graphs;
using Curupira2D.Desktop.Samples.Systems.TiledMap;
using Curupira2D.ECS;
using Curupira2D.Extensions.Pathfinding;
using Curupira2D.Extensions.TiledMap;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TiledLib;
using TiledLib.Layer;

namespace Curupira2D.Desktop.Samples.BTree.Leafs
{
    public class PathfindingNearbyGoldMineAction(Scene scene) : Leaf
    {
        private static GridGraph _gridGraph;
        private static Map _map;

        public override BehaviorState Update(IBlackboard blackboard)
        {
            if (blackboard.HasKey("NearbyGoldMinePath") && blackboard.Get<IEnumerable<Vector2>>("NearbyGoldMinePath") != null)
                return Failure();

            // Get the tiled map from the scene
            if (_map == null)
            {
                _map = scene.GetSystem<MapSystem>()?.TiledMapComponent?.Map;
                return Running();
            }

            // Build the grid graph from the tiled map
            if (_gridGraph == null)
            {
                var tileLayerPathfindWalls = _map.Get<TileLayer>("pathfind-walls");
                _gridGraph = GridGraphBuilder.Build(tileLayerPathfindWalls, true);

                return Running();
            }

            var goldMineEntities = scene.GetEntities(_ => _.Group == "goldMines" && _.Active)
                .OrderBy(_ => _.UniqueId).ToArray();

            if (goldMineEntities == null || goldMineEntities.Length == 0)
                return Failure();

            // Find path with A* algorithm
            var start = scene.GetEntity("miner").Position.Vector2ToGridGraphPoint(_map, scene);
            var goal = goldMineEntities[0].Position.Vector2ToGridGraphPoint(_map, scene);
            var path = AStarPathfinder.FindPath(_gridGraph, start, goal);

            Debug.WriteLine(_gridGraph.GetDebugPathfinder(start, goal, path, true));

            // Add the miner position to the path
            var nearbyGoldMinePath = new List<Vector2>
            {
                scene.GetEntity("miner").Position
            };
            nearbyGoldMinePath.AddRange(path.Edges.Select(_ => _.GridGraphPointToPositionScene(_map, scene)));

            blackboard.Set("NearbyGoldMineLastPath", nearbyGoldMinePath.LastOrDefault());

            // Remove the last position from the path
            nearbyGoldMinePath = [.. nearbyGoldMinePath.Take(nearbyGoldMinePath.Count - 1)];

            blackboard.Set("NearbyGoldMineEntityUniqueId", goldMineEntities[0].UniqueId);
            blackboard.Set("NearbyGoldMinePath", nearbyGoldMinePath);

            return Success();
        }
    }
}
