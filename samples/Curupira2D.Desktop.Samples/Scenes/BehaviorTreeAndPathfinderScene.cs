using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Curupira2D.AI.BehaviorTree;
using Curupira2D.AI.Extensions;
using Curupira2D.AI.Pathfinding.AStar;
using Curupira2D.AI.Pathfinding.Graphs;
using Curupira2D.Desktop.Samples.Common.Scenes;
using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Curupira2D.Desktop.Samples.Scenes
{
    class BehaviorTreeAndPathfinderScene : SceneBase
    {
        IBlackboard _blackboard;
        BehaviorTree _behaviorTree;
        Texture2D _minerTexture, _goldMineTexture, _pixelTexture;
        Entity _miner, _goldMine;
        IEnumerable<Vector2> _pathtoScreen;

        int _pathIndex = 0;
        Vector2 _objectPosition;
        Vector2 _targetPosition;
        readonly float _speed = 100f;

        public override void LoadContent()
        {
            SetTitle(nameof(BehaviorTreeAndPathfinderScene));

            //AddSystem(new MapSystem("AI/BehaviorTreeAndPathfinderTiledMap.tmx", "AI/BehaviorTreeAndPathfinderTileset"));

            ShowControlTips("MOVIMENT: Keyboard Arrows", y: 120f);




            _minerTexture = GameCore.Content.Load<Texture2D>("AI/GoblinSpritesheet");
            _goldMineTexture = GameCore.Content.Load<Texture2D>("AI/GoldMineSpritesheet");
            _pixelTexture = GameCore.GraphicsDevice.CreateTextureRectangle(10, Color.Red * 0.5f);

            _objectPosition = ScreenCenter;

            _miner = CreateEntity("miner", _objectPosition)
                .AddComponent(new SpriteComponent(_minerTexture, sourceRectangle: new Microsoft.Xna.Framework.Rectangle(0, 0, 64, 64)));

            _goldMine = CreateEntity("goldMine", 100, 200)
               .AddComponent(new SpriteComponent(_goldMineTexture, sourceRectangle: new Microsoft.Xna.Framework.Rectangle(0, 0, 32, 32)));




            var gridGraph = new GridGraph(80, 64, true)
            {
                //Walls = walls,
            };

            var start = new System.Drawing.Point(
                (int)_miner.Position.X / 10, (int)InvertPositionY(_miner.Position.Y) / 10);
            var goal = new System.Drawing.Point(
                (int)_goldMine.Position.X / 10, (int)InvertPositionY(_goldMine.Position.Y) / 10);

            var path = AStarPathfinder.FindPath(gridGraph, start, goal);
            _pathtoScreen = path.Edges.Select(_ => new Vector2(_.X * 10, InvertPositionY(_.Y * 10)));

            _targetPosition = _pathtoScreen.ElementAt(_pathIndex);


            /*
            Tired
            Not Tired
            Bag Empty
            Bag Full
            Mine End
            Find Other Mine
             */

            _blackboard = new Blackboard();

            _behaviorTree = BehaviorTreeBuilder.GetInstance()
                .Selector()
                    .DebugLogAction("Finding mine")
                    .Sequence()
                        .DebugLogAction("Fatigue?")
                        .DebugLogAction("Go home")
                        .DebugLogAction("Sleep")
                    .Close()
                    .Sequence()
                        .DebugLogAction("Mine has gold?")
                        .DebugLogAction("Miner with bag empty")
                        .DebugLogAction("Deposit gold")
                    .Close()
                .Close()
            .Build(_blackboard);


            Debug.WriteLine(_behaviorTree.GetTreeStructure());

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (_objectPosition != _targetPosition)
            {
                var direction = _targetPosition - _objectPosition;

                if (direction.Length() > 0)
                {
                    direction.Normalize();
                    _objectPosition += direction * _speed * DeltaTime;

                    if (Vector2.Distance(_objectPosition, _targetPosition) < 1f)
                    {
                        _objectPosition = _targetPosition;
                        _miner.SetPosition(_objectPosition);

                        _pathIndex = (_pathIndex + 1) % _pathtoScreen.Count();
                        _targetPosition = _pathtoScreen.ElementAt(_pathIndex);
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Draw()
        {
            SpriteBatch.Begin();

            //Draw edges for debug purposes.
            for (int i = 0; i < _pathtoScreen.Count(); i++)
                SpriteBatch.Draw(_pixelTexture, _pathtoScreen.Select(_ => new Vector2(_.X, InvertPositionY(_.Y))).ElementAt(i), Color.White);

            SpriteBatch.End();

            base.Draw();
        }
    }
}
