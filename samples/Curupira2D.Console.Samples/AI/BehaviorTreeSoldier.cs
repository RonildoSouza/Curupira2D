using Curupira2D.AI.BehaviorTree;
using Curupira2D.AI.BehaviorTree.Leafs;

namespace Curupira2D.Console.Samples.AI
{
    public static class BehaviorTreeSoldier
    {
        public static void Main()
        {
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;

            var blackboard = new Blackboard();
            var behaviorTreeBuilder = BehaviorTreeBuilder.GetInstance();

            // Define behavior tree structure
            behaviorTreeBuilder
            .Selector()
                .Sequence()
                    .Leaf<CheckForEnemyAction>()
                    .RandomSelector()
                        .Leaf<AttackEnemyAction>()
                        .ExecuteAction((bb) =>
                        {
                            System.Console.WriteLine("🛡️ BLOCKING THE ENEMY ATTACK");
                            return NodeState.Success;
                        })
                    .Close()
                .Close()
                .Leaf<PatrolAction>()
            .Close();

            var behaviorTree = behaviorTreeBuilder.Build(blackboard, updateIntervalInMilliseconds: 0);

            //System.Console.WriteLine($"BEHAVIOR TREE STRUCTURE\n{behaviorTree.GetStringTree()}\n");

            // Simulating AI Tick Loop
            for (int i = 0; i < 10; i++)
            {
                behaviorTree.Tick();
                System.Console.WriteLine($"BEHAVIOR TREE STRUCTURE\n{behaviorTree.GetStringTree()}\n");

                System.Console.WriteLine("-------------------");
                Thread.Sleep(1000);
            }
        }
    }

    public class CheckForEnemyAction : ActionLeaf
    {
        public override NodeState Update(IBlackboard blackboard)
        {
            var enemyNearby = Random.Shared.Next(0, 2) == 1; // Simulated enemy detection
            blackboard.Set("EnemyDetected", enemyNearby);

            System.Console.WriteLine($"{(enemyNearby ? "⚠️ ENEMY FOUND!" : "✅ NO ENEMIES")}");

            return enemyNearby ? NodeState.Success : NodeState.Failure;
        }
    }

    public class AttackEnemyAction : ActionLeaf
    {
        public override NodeState Update(IBlackboard blackboard)
        {
            if (blackboard.Get<bool>("EnemyDetected"))
            {
                System.Console.WriteLine("🗡️ ATTACKING THE ENEMY!");
                return NodeState.Success;
            }

            System.Console.WriteLine("👣 NO ENEMY TO ATTACK KEEP PATROLLING");
            return NodeState.Failure;
        }
    }

    public class PatrolAction : ActionLeaf
    {
        public override NodeState Update(IBlackboard blackboard)
        {
            System.Console.WriteLine("👣 PATROLLING THE AREA");
            return Random.Shared.Next(0, 2) == 1 ? NodeState.Success : NodeState.Failure;
        }
    }
}