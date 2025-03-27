using Curupira2D.AI.BehaviorTree;
using Curupira2D.AI.BehaviorTree.Leafs;

namespace Curupira2D.Console.Samples.AI
{
    public static class BehaviorTreeTest
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
                    .RandomSequence()
                        .Leaf<AttackEnemyAction>()
                        .ExecuteAction((bb) =>
                        {
                            System.Console.WriteLine("🛡️ BLOCKING THE ENEMY ATTACK");
                            return BehaviorState.Success;
                        })
                    .Close()
                .Close()
                .Leaf<PatrolAction>()
            .Close();

            //behaviorTreeBuilder
            //.RandomSelector()
            //    .RandomSequence()
            //        .ExecuteAction((bb) =>
            //        {
            //            System.Console.WriteLine("Action 1.1");
            //            return Random.Shared.NextDouble() < 0.5 ? BehaviorState.Success : BehaviorState.Running;
            //        })
            //        //.Repeater(2)
            //        .ExecuteAction((bb) =>
            //        {
            //            System.Console.WriteLine("Action 1.2");
            //            return BehaviorState.Success;
            //        })
            //        .ExecuteAction((bb) =>
            //        {
            //            System.Console.WriteLine("Action 1.3");
            //            return BehaviorState.Failure;
            //        })
            //        .ExecuteAction((bb) =>
            //        {
            //            System.Console.WriteLine("Action 1.4");
            //            return BehaviorState.Success;
            //        })
            //    .Close()
            //    .Sequence()
            //        .ExecuteAction((bb) =>
            //        {
            //            System.Console.WriteLine("Action 2.1");
            //            return Random.Shared.NextDouble() < 0.5 ? BehaviorState.Success : BehaviorState.Failure;
            //        })
            //    .Close()
            //.Close();

            var behaviorTree = behaviorTreeBuilder.Build(blackboard, updateIntervalInMilliseconds: 0);

            System.Console.WriteLine($"BEHAVIOR TREE STRUCTURE" +
                $"\n{behaviorTree.GetTreeStructure()}\n");

            // Simulating AI Tick Loop
            for (int i = 1; i <= 10; i++)
            {
                System.Console.WriteLine($"==== Tick {i} ====");
                behaviorTree.Tick();
#if DEBUG
                System.Console.WriteLine($"{blackboard.Get<string>("_BehaviorTreeStructureWithState")}\n");
#endif
                Thread.Sleep(500);
            }
        }
    }

    public class CheckForEnemyAction : Leaf
    {
        public override BehaviorState Update(IBlackboard blackboard)
        {
            var enemyNearby = Random.Shared.Next(0, 2) == 1; // Simulated enemy detection
            blackboard.Set("EnemyDetected", enemyNearby);

            System.Console.WriteLine($"{(enemyNearby ? "⚠️ ENEMY FOUND!" : "✅ NO ENEMIES")}");

            return State = enemyNearby ? BehaviorState.Success : BehaviorState.Failure;
        }
    }

    public class AttackEnemyAction : Leaf
    {
        public override BehaviorState Update(IBlackboard blackboard)
        {
            if (blackboard.Get<bool>("EnemyDetected"))
            {
                System.Console.WriteLine("🗡️ ATTACKING THE ENEMY!");
                return State = BehaviorState.Success;
            }

            System.Console.WriteLine("👣 NO ENEMY TO ATTACK KEEP PATROLLING");
            return State = BehaviorState.Failure;
        }
    }

    public class PatrolAction : Leaf
    {
        public override BehaviorState Update(IBlackboard blackboard)
        {
            System.Console.WriteLine("👣 PATROLLING THE AREA");
            return State = Random.Shared.Next(0, 2) == 1 ? BehaviorState.Success : BehaviorState.Running;
        }
    }
}