using Curupira2D.AI.BehaviorTree;
using Curupira2D.AI.BehaviorTree.Leafs;

namespace Curupira2D.Console.Samples.AI
{
    public static class BehaviorTreeSoldier
    {
        public static void Main()
        {
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
            System.Console.WriteLine(@"
                ROOT (selector)
                |
                O-------------------------------------------------------O
                |                                                       |
                V                                                       |
                COMPOSITE (sequence)                                    |
                |                                                       |
                O-----------------------O                               |
                |                       |                               |
                |                       |                               |
                |                       |                               |
                V                       V                               V
                LEAF (check enemy)      DECORATOR (repeat attack)       LEAF (patrol area)
                                        |
                                        |
                                        |
                                        V
                                        LEAF (attack enemy)
            ");

            var blackboard = new Blackboard();
            var behaviorTreeBuilder = BehaviorTreeBuilder.GetInstance();

            // Define behavior tree structure
            behaviorTreeBuilder
            .Selector()
                    .Sequence()
                    .Leaf<CheckForEnemyAction>()
                    .Repeater(2)
                    .Leaf<AttackEnemyAction>()
                    .Close()
                .Leaf<PatrolAction>()
            .Close();

            var behaviorTree = behaviorTreeBuilder.Build(blackboard, updateIntervalInMilliseconds: 10);

            // Simulating AI Tick Loop
            for (int i = 0; i < 10; i++)
            {
                behaviorTree.Tick();

                System.Console.WriteLine("-------------------");
                Thread.Sleep(1000);
            }
        }
    }

    public class CheckForEnemyAction : ActionLeaf
    {
        public override State Tick(IBlackboard blackboard)
        {
            var enemyNearby = Random.Shared.Next(0, 2) == 1; // Simulated enemy detection
            blackboard.Set("EnemyDetected", enemyNearby);

            System.Console.WriteLine($"{(enemyNearby ? "⚠️ ENEMY FOUND!" : "✅ NO ENEMIES")}");

            return enemyNearby ? State.Success : State.Failure;
        }
    }

    public class AttackEnemyAction : ActionLeaf
    {
        public override State Tick(IBlackboard blackboard)
        {
            if (blackboard.Get<bool>("EnemyDetected"))
            {
                System.Console.WriteLine("⚔️ ATTACKING THE ENEMY!");
                return State.Success;
            }

            System.Console.WriteLine("👣 NO ENEMY TO ATTACK KEEP PATROLLING");
            return State.Failure;
        }
    }

    public class PatrolAction : ActionLeaf
    {
        public override State Tick(IBlackboard blackboard)
        {
            System.Console.WriteLine("👣 PATROLLING THE AREA");
            return State.Running;
        }
    }
}