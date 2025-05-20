using Curupira2D.Desktop.Samples.Systems.BehaviorTreeAndPathfinder;
using Curupira2D.ECS;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;

namespace Curupira2D.Desktop.Samples.BTree.Leafs
{
    public class MoveToGoldDepositAction(Scene scene) : BaseMoveCharacterWithPathfindAction(scene, entityUniqueId: "miner", bbPathfindKey: "NearbyGoldMinePathToDeposit")
    {
        private readonly MinerControllerSystem _minerControllerSystem = scene.GetSystem<MinerControllerSystem>();

        protected override void RunningAction(Vector2 currentDirection)
        {
            _minerControllerSystem.MinerState.CurrentDirection = currentDirection.GetSafeNormalize();
            _minerControllerSystem.MinerState.CurrentMinerAction = MinerState.MinerAction.GoToDeposit;
        }

        protected override void SuccessAction()
        {
            _minerControllerSystem.MinerState.CurrentMinerAction = MinerState.MinerAction.Idle;
        }
    }
}
