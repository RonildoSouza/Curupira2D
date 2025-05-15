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
    public class MoveToHomeAction(Scene scene) : Leaf
    {
        IEnumerable<Vector2> _nearbyGoldMineToHomePath;
        int _pathIndex = 1;
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
                _nearbyGoldMinePosition = _nearbyGoldMineToHomePath?.ElementAtOrDefault(_pathIndex) ?? Vector2.Zero;
            }

            if (_nearbyGoldMineToHomePath != null && _miner.Position != default && _pathIndex < _nearbyGoldMineToHomePath.Count())
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
                    {
                        minerControllerSystem.MinerState.CurrentMinerAction = MinerState.MinerAction.Idle;
                        Reset();
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

        private void Reset()
        {
            _minerPosition = Vector2.Zero;
            _nearbyGoldMineToHomePath = null;
            _nearbyGoldMinePosition = default;
            _pathIndex = 1;
        }
    }
}
