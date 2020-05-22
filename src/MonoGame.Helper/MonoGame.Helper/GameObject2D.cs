using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGame.Helper.Core;
using System.Collections.Generic;

namespace MonoGame.Helper
{
    /// <summary>
    /// Game objects 2D representation.
    /// </summary>
    public class GameObject2D
    {
        /// <summary>
        /// Game object position - default <see cref="Vector2.Zero"/>
        /// </summary>
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        /// <summary>
        /// Game object scale - default <see cref="Vector2.One"/>
        /// </summary>
        public Vector2 Scale { get; set; }
        /// <summary>
        /// Game object visibility - default true
        /// </summary>
        public bool IsVisible { get; set; }
        public GameObject2D Parent { get; private set; }
        public List<GameObject2D> Children { get; }
        /// <summary>
        /// Game object velocity - default <see cref="Vector2.Zero"/>
        /// </summary>
        public Vector2 Velocity { get; set; }

        public Vector2 Acceleration { get; set; }
        public float Mass { get; set; }
        public float Gravity { get; set; }

        public GameObject2D()
        {
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Children = new List<GameObject2D>();
            IsVisible = true;
            Velocity = Vector2.Zero;
        }

        /// <summary>
        /// Add child 2D game object.
        /// </summary>
        /// <param name="child"></param>
        public void AddChild(GameObject2D child)
        {
            if (Children.Contains(child))
                return;

            Children.Add(child);
            child.Parent = this;
        }

        /// <summary>
        /// Remove child 2D game object.
        /// </summary>
        /// <param name="child"></param>
        public void RemoveChild(GameObject2D child)
        {
            if (!Children.Contains(child))
                return;

            if (Children.Remove(child))
                child.Parent = null;
        }

        public virtual void LoadContent(ContentManager contentManager) { }
        public virtual void Update(RenderContext renderContext) { }
        public virtual void Draw(RenderContext renderContext) { }
    }
}
