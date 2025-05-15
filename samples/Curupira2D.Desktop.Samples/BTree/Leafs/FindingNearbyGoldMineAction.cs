using Curupira2D.AI.BehaviorTree;
using Curupira2D.AI.BehaviorTree.Leafs;
using Curupira2D.AI.Extensions;
using Curupira2D.AI.Pathfinding;
using Curupira2D.AI.Pathfinding.AStar;
using Curupira2D.AI.Pathfinding.Graphs;
using Curupira2D.Desktop.Samples.Systems.TiledMap;
using Curupira2D.ECS;
using Curupira2D.Extensions.Pathfinding;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TiledLib;
using TiledLib.Layer;

namespace Curupira2D.Desktop.Samples.BTree.Leafs
{
    public class FindingNearbyGoldMineAction(Scene scene) : Leaf
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
                var tileLayerPathfindWalls = _map.Layers.OfType<TileLayer>().FirstOrDefault(_ => _.Name == "pathfind-walls");
                _gridGraph = GridGraphBuilder.Build(tileLayerPathfindWalls, true);

                return Running();
            }

            var goldMines = scene.GetEntities(_ => _.Group == "goldMines" && _.Active);

            if (!goldMines?.Any() ?? false)
                return Failure();

            var goldMinePaths = new List<Path<System.Drawing.Point>>();

            // Find paths to all gold mines with A* algorithm
            foreach (var goldMine in goldMines)
            {
                var start = scene.GetEntity("miner").Position.Vector2ToGridGraphPoint(_map, scene);
                var goal = goldMine.Position.Vector2ToGridGraphPoint(_map, scene);
                var path = AStarPathfinder.FindPath(_gridGraph, start, goal);

                goldMinePaths.Add(path);
                //Debug.WriteLine(_gridGraph.GetDebugPathfinder(start, goal, path, true));
            }

            // Add the miner position to the path
            var nearbyGoldMinePath = new List<Vector2>
            {
                scene.GetEntity("miner").Position
            };

            // Get the nearest gold mine path
            nearbyGoldMinePath.AddRange(goldMinePaths
                .OrderBy(_ => _.DurationCostSoFar)
                .ElementAt(0)
                //.FirstOrDefault(_ => _.FoundPath)
                .Edges
                .Select(_ => _.GridGraphPointToPositionScene(_map, scene))
            );

            // Remove the last position from the path
            nearbyGoldMinePath = [.. nearbyGoldMinePath.Take(nearbyGoldMinePath.Count - 1)];

            blackboard.Set("NearbyGoldMinePath", nearbyGoldMinePath);

            return Success();
        }
    }
}
