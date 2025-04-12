using Curupira2D.AI.BehaviorTree;
using Curupira2D.AI.Extensions;
using Curupira2D.AI.Pathfinding;
using Curupira2D.AI.Pathfinding.AStar;
using Curupira2D.Desktop.Samples.Common.Scenes;
using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Curupira2D.Desktop.Samples.Scenes
{
    class BehaviorTreeAndPathfinderScene : SceneBase
    {
        IBlackboard _blackboard;
        BehaviorTree _behaviorTree;
        Texture2D _minerTexture, _goldMineTexture, _pixelTexture;
        Entity _miner, _goldMine;
        IEnumerable<Vector2> _closestGoldMineEdges;

        int _pathIndex = 0;
        Vector2 _minerPosition;
        Vector2 _nearestGoldMinePosition;
        readonly float _speed = 100f;

        public override void LoadContent()
        {
            SetTitle(nameof(BehaviorTreeAndPathfinderScene));

            //AddSystem(new MapSystem("AI/BehaviorTreeAndPathfinderTiledMap.tmx", "AI/BehaviorTreeAndPathfinderTileset"));

            ShowControlTips("MOVIMENT: Keyboard Arrows", y: 120f);


            _minerTexture = GameCore.Content.Load<Texture2D>("AI/GoblinSpritesheet");
            _goldMineTexture = GameCore.Content.Load<Texture2D>("AI/GoldMineSpritesheet");
            _pixelTexture = GameCore.GraphicsDevice.CreateTextureRectangle(10, Color.Red * 0.5f);

            _minerPosition = new Vector2(700, 500);

            _miner = CreateEntity("miner", _minerPosition)
                .AddComponent(new SpriteComponent(_minerTexture, sourceRectangle: new Rectangle(0, 0, 64, 64)));

            _goldMine = CreateEntity("goldMine", 100, 200)
               .AddComponent(new SpriteComponent(_goldMineTexture, sourceRectangle: new Rectangle(0, 0, 32, 32)));



            // PATHFINDING
            var baseGridGraph = 20;

            var gridGraph = GridGraphBuilder.Build(ScreenWidth / baseGridGraph, ScreenHeight / baseGridGraph, true);

            var start = new System.Drawing.Point(
                (int)_miner.Position.X / baseGridGraph,
                (int)InvertPositionY(_miner.Position.Y) / baseGridGraph);

            var goal = new System.Drawing.Point(
                (int)_goldMine.Position.X / baseGridGraph,
                (int)InvertPositionY(_goldMine.Position.Y) / baseGridGraph);


            var path = AStarPathfinder.FindPath(gridGraph, start, goal);
            _closestGoldMineEdges = path.Edges.Select(_ => PositionToScene(new Vector2(_.X * baseGridGraph, _.Y * baseGridGraph)));

            _nearestGoldMinePosition = _closestGoldMineEdges.ElementAt(_pathIndex);

            Debug.WriteLine(gridGraph.GetDebugPathfinder(start, goal, path, true));


            ///*
            //Tired
            //Not Tired
            //Bag Empty
            //Bag Full
            //Mine End
            //Find Other Mine
            // */

            //_blackboard = new Blackboard();

            //_behaviorTree = BehaviorTreeBuilder.GetInstance()
            //    .Selector()
            //        .DebugLogAction("Finding mine")
            //        .Sequence()
            //            .DebugLogAction("Fatigue?")
            //            .DebugLogAction("Go home")
            //            .DebugLogAction("Sleep")
            //        .Close()
            //        .Sequence()
            //            .DebugLogAction("Mine has gold?")
            //            .DebugLogAction("Miner with bag empty")
            //            .DebugLogAction("Deposit gold")
            //        .Close()
            //    .Close()
            //.Build(_blackboard);


            //Debug.WriteLine(_behaviorTree.GetTreeStructure());

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //Debug.WriteLine(_pathIndex);

            if (_pathIndex < _closestGoldMineEdges.Count())
            {
                var direction = (_nearestGoldMinePosition - _minerPosition).GetSafeNormalize();

                if (direction.Length() > 0)
                {
                    _minerPosition += direction * _speed * DeltaTime;
                    _miner.SetPosition(_minerPosition);

                    // Next edge position without loop (index reset to zero)
                    if (Vector2.Distance(_minerPosition, _nearestGoldMinePosition) < 1f && _pathIndex < _closestGoldMineEdges.Count() - 1)
                    {
                        _pathIndex = (_pathIndex + 1) % _closestGoldMineEdges.Count();
                        _nearestGoldMinePosition = _closestGoldMineEdges.ElementAt(_pathIndex);
                    }

                    // Finish position
                    if (Vector2.Distance(_minerPosition, _nearestGoldMinePosition) < 1f)
                        _minerPosition = _nearestGoldMinePosition;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw()
        {
            SpriteBatch.Begin();

            // DRAW EDGES FOR DEBUG
            for (int i = 0; i < _closestGoldMineEdges.Count(); i++)
                SpriteBatch.Draw(_pixelTexture, _closestGoldMineEdges.Select(PositionToScene).ElementAt(i), Color.White);

            SpriteBatch.End();

            base.Draw();
        }
    }
}
