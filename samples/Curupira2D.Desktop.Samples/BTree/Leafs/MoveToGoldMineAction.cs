using Curupira2D.AI.BehaviorTree;
using Curupira2D.AI.BehaviorTree.Leafs;
using Curupira2D.Desktop.Samples.Systems.BehaviorTreeAndPathfinder;
using Curupira2D.ECS;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Curupira2D.Desktop.Samples.BTree.Leafs
{
    public class MoveToGoldMineAction(Scene scene) : Leaf
    {
        IEnumerable<Vector2> _nearbyGoldMinePath;
        int _pathIndex = 0;
        Vector2 _minerPosition;
        Vector2 _nearbyGoldMinePosition;

        readonly Entity _miner = scene.GetEntity("miner");
        readonly MinerControllerSystem minerControllerSystem = scene.GetSystem<MinerControllerSystem>();

        public override BehaviorState Update(IBlackboard blackboard)
        {
            if (_miner.Position != default && _minerPosition == Vector2.Zero)
                _minerPosition = _miner.Position;

            if (blackboard.HasKey("NearbyGoldMinePath") && (_nearbyGoldMinePath == null || !_nearbyGoldMinePath.Any()))
            {
                _nearbyGoldMinePath = blackboard.Get<IEnumerable<Vector2>>("NearbyGoldMinePath");
                _nearbyGoldMinePosition = _nearbyGoldMinePath?.FirstOrDefault() ?? Vector2.Zero;
            }

            if (_nearbyGoldMinePath != null && _miner.Position != default && _minerPosition != _nearbyGoldMinePosition && _pathIndex < _nearbyGoldMinePath.Count())
            {
                var direction = (_nearbyGoldMinePosition - _minerPosition).GetSafeNormalize();

                if (direction.Length() > 0)
                {
                    minerControllerSystem.MinerState.CurrentDirection = direction.GetSafeNormalize();
                    _minerPosition += direction * MinerState.MaxSpeed * scene.DeltaTime;
                    _miner.SetPosition(_minerPosition);

                    // Next edge position without loop (index reset to zero)
                    if (Vector2.Distance(_minerPosition, _nearbyGoldMinePosition) < 1f && _pathIndex < _nearbyGoldMinePath.Count() - 1)
                    {
                        _pathIndex = (_pathIndex + 1) % _nearbyGoldMinePath.Count();
                        _nearbyGoldMinePosition = _nearbyGoldMinePath.ElementAt(_pathIndex);
                    }

                    // Finish position
                    //if (Vector2.Distance(_minerPosition, _nearbyGoldMinePosition) < 1f)
                    if (_miner.IsCollidedWithAny(scene, "goldMines", true))
                    {
                        _minerPosition = _nearbyGoldMinePosition;
                        minerControllerSystem.MinerState.CurrentMinerAction = MinerState.MinerAction.Mine;
                        Success();
                    }
                    else
                    {
                        minerControllerSystem.MinerState.CurrentMinerAction = MinerState.MinerAction.GoToMine;
                        Running();
                    }
                }
            }

            return State;
        }
    }

    public class MineGoldAction(Scene scene) : Leaf
    {
        readonly MinerControllerSystem minerControllerSystem = scene.GetSystem<MinerControllerSystem>();
        float elapsedTime = 0f;

        public override BehaviorState Update(IBlackboard blackboard)
        {
            if (minerControllerSystem.MinerState.CurrentMinerAction != MinerState.MinerAction.Mine)
                return Failure();

            elapsedTime += scene.DeltaTime;
            if (elapsedTime >= 1)
            {
                if (Random.Shared.NextDouble() > 0.5)
                    minerControllerSystem.MinerState.Energy++;

                minerControllerSystem.MinerState.InventoryCapacity++;
                elapsedTime = 0f;
            }

            if (minerControllerSystem.MinerState.IsInventoryFull)
            {
                minerControllerSystem.MinerState.CurrentMinerAction = MinerState.MinerAction.GoHome;
                return Success();
            }

            return Running();
        }
    }

    public class MoveToHomeAction(Scene scene) : Leaf
    {
        IEnumerable<Vector2> _nearbyGoldMineToHomePath;
        int _pathIndex = 0;
        Vector2 _minerPosition;
        Vector2 _nearbyGoldMinePosition;

        readonly Entity _miner = scene.GetEntity("miner");
        readonly MinerControllerSystem minerControllerSystem = scene.GetSystem<MinerControllerSystem>();

        public override BehaviorState Update(IBlackboard blackboard)
        {
            if (_miner.Position != default && _minerPosition == Vector2.Zero)
                _minerPosition = _miner.Position;

            if (blackboard.HasKey("NearbyGoldMinePath") && (_nearbyGoldMineToHomePath == null || !_nearbyGoldMineToHomePath.Any()))
            {
                _nearbyGoldMineToHomePath = blackboard.Get<IEnumerable<Vector2>>("NearbyGoldMinePath").Reverse();
                _nearbyGoldMinePosition = _nearbyGoldMineToHomePath?.FirstOrDefault() ?? Vector2.Zero;
            }

            if (_nearbyGoldMineToHomePath != null && _miner.Position != default && _minerPosition != _nearbyGoldMinePosition && _pathIndex < _nearbyGoldMineToHomePath.Count())
            {
                var direction = (_nearbyGoldMinePosition - _minerPosition).GetSafeNormalize();

                if (direction.Length() > 0)
                {
                    minerControllerSystem.MinerState.CurrentDirection = direction.GetSafeNormalize();
                    _minerPosition += direction * MinerState.MaxSpeed * scene.DeltaTime;
                    _miner.SetPosition(_minerPosition);

                    // Next edge position without loop (index reset to zero)
                    if (Vector2.Distance(_minerPosition, _nearbyGoldMinePosition) < 1f && _pathIndex < _nearbyGoldMineToHomePath.Count() - 1)
                    {
                        _pathIndex = (_pathIndex + 1) % _nearbyGoldMineToHomePath.Count();
                        _nearbyGoldMinePosition = _nearbyGoldMineToHomePath.ElementAt(_pathIndex);
                    }

                    // Finish position
                    if (Vector2.Distance(_minerPosition, _nearbyGoldMinePosition) < 1f)
                    //if (_miner.IsCollidedWithAny(scene, "goldMines", true))
                    {
                        _minerPosition = _nearbyGoldMinePosition;
                        minerControllerSystem.MinerState.CurrentMinerAction = MinerState.MinerAction.Idle;
                        Success();
                    }
                    else
                    {
                        minerControllerSystem.MinerState.CurrentMinerAction = MinerState.MinerAction.GoHome;
                        Running();
                    }
                }
            }

            return State;
        }
    }
}
