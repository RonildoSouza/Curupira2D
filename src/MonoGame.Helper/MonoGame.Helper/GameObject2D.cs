using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGame.Helper
{
    public class GameObject2D
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; }
        public bool IsVisible { get; set; }
        public GameObject2D Parent { get; private set; }
        public List<GameObject2D> Children { get; }
        public Vector2 Velocity { get; set; }
        public RenderContext RenderContext { get; private set; }

        public GameObject2D(Game game)
        {
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Children = new List<GameObject2D>();
            IsVisible = true;
            SetRenderContext(game);
        }

        public void AddChild(GameObject2D child)
        {
            if (Children.Contains(child))
                return;

            Children.Add(child);
            child.Parent = this;
        }

        public void RemoveChild(GameObject2D child)
        {
            if (!Children.Contains(child))
                return;

            if (Children.Remove(child))
                child.Parent = null;
        }

        public virtual void LoadContent() { }
        public virtual void Update() { }
        public virtual void Draw() { }

        #region Private Methods
        private void SetRenderContext(Game game)
        {
            if (game == null)
                return;

            RenderContext = new RenderContext(game);

            if (!game.Components.Contains(RenderContext))
                game.Components?.Add(RenderContext);
        }
        #endregion
    }
}
