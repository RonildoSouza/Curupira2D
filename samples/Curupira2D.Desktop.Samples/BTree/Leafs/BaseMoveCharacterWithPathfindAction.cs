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
    public abstract class BaseMoveCharacterWithPathfindAction(Scene scene, string entityUniqueId, string bbPathfindKey) : Leaf
    {
        Vector2 _characterEntityTempPosition;
        IEnumerable<Vector2> _path;
        Vector2 _nextPathPosition;
        int _pathIndex = 1;

        readonly Entity _characterEntity = scene.GetEntity(entityUniqueId);

        public override BehaviorState Update(IBlackboard blackboard)
        {
            if (_characterEntity.Position != default && _characterEntityTempPosition == Vector2.Zero)
                _characterEntityTempPosition = _characterEntity.Position;

            if (blackboard.HasKey(bbPathfindKey) && (_path == null || !_path.Any()))
            {
                _path = blackboard.Get<IEnumerable<Vector2>>(bbPathfindKey);
                _nextPathPosition = _path?.ElementAtOrDefault(_pathIndex) ?? Vector2.Zero;
            }

            if (_path != null && _characterEntity.Position != default && _pathIndex < _path.Count())
            {
                var direction = (_nextPathPosition - _characterEntityTempPosition).GetSafeNormalize();

                if (direction.Length() > 0)
                {
                    _characterEntityTempPosition += direction * MinerState.MaxSpeed * scene.DeltaTime;
                    _characterEntity.SetPosition(_characterEntityTempPosition);

                    // Next edge position without loop (index reset to zero)
                    if (Vector2.Distance(_characterEntityTempPosition, _nextPathPosition) < 1f && _pathIndex < _path.Count() - 1)
                    {
                        _pathIndex = (_pathIndex + 1) % _path.Count();
                        _nextPathPosition = _path.ElementAt(_pathIndex);
                    }

                    // Finish position
                    if (Vector2.Distance(_characterEntityTempPosition, _nextPathPosition) < 1f)
                    {
                        SuccessAction();
                        Reset();
                        Success();
                    }
                    else
                    {
                        RunningAction(direction);
                        Running();
                    }
                }
            }

            return State;
        }

        protected abstract void RunningAction(Vector2 currentDirection);

        protected abstract void SuccessAction();

        private void Reset()
        {
            _pathIndex = 1;
            _path = null;
            _nextPathPosition = default;
            _characterEntityTempPosition = Vector2.Zero;
        }
    }
}
