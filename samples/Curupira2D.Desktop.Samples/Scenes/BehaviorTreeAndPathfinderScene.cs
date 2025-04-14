using Curupira2D.AI.BehaviorTree;
using Curupira2D.Desktop.Samples.Common.Scenes;
using Curupira2D.Desktop.Samples.Systems.TiledMap;
using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Curupira2D.Desktop.Samples.Scenes
{
    class BehaviorTreeAndPathfinderScene : SceneBase
    {
        IBlackboard _blackboard;
        BehaviorTree _behaviorTree;

        public override void LoadContent()
        {
            SetTitle(nameof(BehaviorTreeAndPathfinderScene));

            AddSystem<MinerControllerSystem>();
            AddSystem<GoldMineControllerSystem>();
            AddSystem(new MapSystem("AI/BehaviorTreeAndPathfinderTiledMap.tmx", "AI/BehaviorTreeAndPathfinderTileset"));

            ShowControlTips("MOVIMENT: Keyboard Arrows", y: 120f);


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
    }

    [RequiredComponent(typeof(MinerControllerSystem), typeof(SpriteAnimationComponent))]
    class MinerControllerSystem : ECS.System, ILoadable, IUpdatable
    {
        Texture2D _minerTexture;
        Entity _miner;
        //Entity _pixelTexture;

        IEnumerable<Vector2> _closestGoldMineEdges;
        int _pathIndex = 0;
        Vector2 _minerPosition;
        Vector2 _nearestGoldMinePosition;
        readonly float _speed = 100f;
        SpriteAnimationComponent _movementSpriteAnimationComponent, _mineSpriteAnimationComponent;

        public void LoadContent()
        {
            _minerTexture = Scene.GameCore.Content.Load<Texture2D>("AI/GoblinSpritesheet");
            //_pixelTexture = Scene.GameCore.GraphicsDevice.CreateTextureRectangle(10, Color.Red * 0.5f);

            _movementSpriteAnimationComponent = new SpriteAnimationComponent(
                    texture: _minerTexture,
                    frameRowsCount: 1,
                    frameColumnsCount: 8,
                    frameTimeMilliseconds: 100,
                    animateType: AnimateType.PerRow,
                    sourceRectangle: new Rectangle(128, 0, 64, 64),
                    isLooping: true,
                    layerDepth: 0.02f,
                    textureSizeOffset: new Vector2(192, 0));

            _mineSpriteAnimationComponent = new SpriteAnimationComponent(
                    texture: _minerTexture,
                    frameRowsCount: 1,
                    frameColumnsCount: 10,
                    frameTimeMilliseconds: 100,
                    animateType: AnimateType.PerRow,
                    sourceRectangle: new Rectangle(0, 64, 64, 64),
                    isLooping: true,
                    layerDepth: 0.02f);

            _miner = Scene.CreateEntity("miner", default)
                .AddComponent(_movementSpriteAnimationComponent);


            //// PATHFINDING
            //var baseGridGraph = 20;

            //var gridGraph = GridGraphBuilder.Build(Scene.ScreenWidth / baseGridGraph, Scene.ScreenHeight / baseGridGraph, true);

            //var start = new System.Drawing.Point(
            //    (int)_miner.Position.X / baseGridGraph,
            //    (int)Scene.InvertPositionY(_miner.Position.Y) / baseGridGraph);

            //var goal = new System.Drawing.Point(
            //    (int)_goldMine.Position.X / baseGridGraph,
            //    (int)Scene.InvertPositionY(_goldMine.Position.Y) / baseGridGraph);


            //var path = AStarPathfinder.FindPath(gridGraph, start, goal);
            //_closestGoldMineEdges = path.Edges.Select(_ => Scene.PositionToScene(new Vector2(_.X * baseGridGraph, _.Y * baseGridGraph)));

            //_nearestGoldMinePosition = _closestGoldMineEdges.ElementAt(_pathIndex);

            //Debug.WriteLine(gridGraph.GetDebugPathfinder(start, goal, path, true));
        }

        public void Update()
        {
            _movementSpriteAnimationComponent.IsPlaying = true;
        }

        //public override void Update(GameTime gameTime)
        //{
        //    if (_pathIndex < _closestGoldMineEdges.Count())
        //    {
        //        var direction = (_nearestGoldMinePosition - _minerPosition).GetSafeNormalize();

        //        if (direction.Length() > 0)
        //        {
        //            _minerPosition += direction * _speed * DeltaTime;
        //            _miner.SetPosition(_minerPosition);

        //            // Next edge position without loop (index reset to zero)
        //            if (Vector2.Distance(_minerPosition, _nearestGoldMinePosition) < 1f && _pathIndex < _closestGoldMineEdges.Count() - 1)
        //            {
        //                _pathIndex = (_pathIndex + 1) % _closestGoldMineEdges.Count();
        //                _nearestGoldMinePosition = _closestGoldMineEdges.ElementAt(_pathIndex);
        //            }

        //            // Finish position
        //            if (Vector2.Distance(_minerPosition, _nearestGoldMinePosition) < 1f)
        //                _minerPosition = _nearestGoldMinePosition;
        //        }
        //    }

        //    base.Update(gameTime);
        //}

        //public override void Draw()
        //{
        //    SpriteBatch.Begin();

        //    // DRAW EDGES FOR DEBUG
        //    for (int i = 0; i < _closestGoldMineEdges.Count(); i++)
        //        SpriteBatch.Draw(_pixelTexture, _closestGoldMineEdges.Select(PositionToScene).ElementAt(i), Color.White);

        //    SpriteBatch.End();

        //    base.Draw();
        //}
    }

    [RequiredComponent(typeof(GoldMineControllerSystem), typeof(SpriteComponent))]
    class GoldMineControllerSystem : ECS.System, ILoadable, IUpdatable
    {
        Texture2D _goldMineTexture;
        static readonly IDictionary<string, int> _goldMinesAndAvailable = new Dictionary<string, int>();

        public void LoadContent()
        {
            _goldMineTexture = Scene.GameCore.Content.Load<Texture2D>("AI/GoldMineSpritesheet");

            for (int i = 0; i < 4; i++)
            {
                _goldMinesAndAvailable.Add($"goldMine{i}", 0);

                Scene.CreateEntity(_goldMinesAndAvailable.Keys.ElementAt(i), default, "goldMines")
                    .AddComponent(new SpriteComponent(
                        texture: _goldMineTexture,
                        //frameRowsCount: 1,
                        //frameColumnsCount: 4,
                        //frameTimeMilliseconds: 500,
                        //animateType: AnimateType.PerRow,
                        sourceRectangle: new Rectangle(0, 0, 32, 32),
                        layerDepth: 0.02f));
            }
        }

        public void Update()
        {
            for (int i = 0; i < _goldMinesAndAvailable.Count(); i++)
            {
                var entityUniqueId = _goldMinesAndAvailable.Keys.ElementAt(i);

                if (_goldMinesAndAvailable[entityUniqueId] >= 100)
                    continue;

                var entity = Scene.GetEntity(entityUniqueId);

                if (entity == null || !entity.Active)
                    continue;

                var spriteComponent = entity.GetComponent<SpriteComponent>();

                if (_goldMinesAndAvailable[entityUniqueId] == 0)
                {
                    spriteComponent.SourceRectangle = new Rectangle(96, 0, 32, 32);
                    //_goldMinesAndAvailable[entityUniqueId] = -1;
                }

                if (_goldMinesAndAvailable[entityUniqueId] >= 75)
                    spriteComponent.SourceRectangle = new Rectangle(32, 0, 32, 32);

                if (_goldMinesAndAvailable[entityUniqueId] <= 50)
                    spriteComponent.SourceRectangle = new Rectangle(64, 0, 32, 32);

                if (_goldMinesAndAvailable[entityUniqueId] < 0)
                    entity.SetActive(false);
            }
        }

        public static bool ThereIsGoldAvailable(string entityUniqueId)
            => _goldMinesAndAvailable.TryGetValue(entityUniqueId, out int available) && available > 0;

        private int GetGoldMineState(string entityUniqueId)
        {
            if (_goldMinesAndAvailable.TryGetValue(entityUniqueId, out int available))
                return available;

            return 0;
        }
    }
}
