using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Helper.Samples.Scenes;

namespace MonoGame.Helper.SamplesX
{
    public class Game1 : GameCore
    {
        public Game1() : base(800, 640, true)
        {
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            //SetScene<MenuScene>();
            SetScene<SpriteAnimationScene>();
            //SetScene<SceneGraphScene>();
            //SetScene<CameraScene>();
            //SetScene<PhysicScene>();
            //SetScene<TiledMapScene>();
            base.Initialize();
        }
    }
}
