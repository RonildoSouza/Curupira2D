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
using TiledLib.Objects;

namespace Curupira2D.Desktop.Samples.BTree.Leafs
{
    public abstract class BasePathfindingStartingNearbyGoldMineAction(Scene scene, string bbPathfindKey, string pointObjectName) : Leaf
    {
        private static GridGraph _gridGraph;
        private static Map _map;

        public override BehaviorState Update(IBlackboard blackboard)
        {
            if (blackboard.HasKey(bbPathfindKey) && blackboard.Get<IEnumerable<Vector2>>(bbPathfindKey) != null)
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

            // Find path with A* algorithm
            var goldDeposit = _map.Get<ObjectLayer>("object-positions").Get<PointObject>(pointObjectName);

            var start = blackboard.Get<Vector2>("NearbyGoldMineLastPath").Vector2ToGridGraphPoint(_map, scene);
            var goal = goldDeposit.ToVector2(_map, scene).Vector2ToGridGraphPoint(_map, scene);
            var path = AStarPathfinder.FindPath(_gridGraph, start, goal);

            Debug.WriteLine(_gridGraph.GetDebugPathfinder(start, goal, path, true));

            blackboard.Set(bbPathfindKey, path.Edges.Select(_ => _.GridGraphPointToPositionScene(_map, scene)));

            return Success();
        }
    }
}
