/**
* https://gamedevelopment.tutsplus.com/tutorials/quick-tip-use-quadtrees-to-detect-likely-collisions-in-2d-space--gamedev-374
* https://gist.github.com/RonildoSouza/4314d279a02a3214acc59c2cf6bf9b12
*/

using Curupira2D.ECS;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Curupira2D
{
    /// <summary></summary>
    /// <param name="bounds">The 2D space that the node occupies</param>
    /// <param name="level">The current node level (0 being the topmost)</param>
    public sealed class Quadtree(Rectangle bounds, int level = 0) : IDisposable
    {
        const int MAX_OBJECTS = 10;
        const int MAX_LEVELS = 5;
        bool _disposed = false;

        /// <summary>
        /// The list of objects in our current node
        /// </summary>
        List<Entity> _objects = [];

        /// <summary>
        /// The four subnodes. Nodes fill out in a counter clockwise mannor
        /// </summary>
        readonly Quadtree[] _nodes = new Quadtree[4];

        ~Quadtree() => Dispose(disposing: false);

        /// <summary>
        /// Gets the count of how many objects are in this current node.
        /// </summary>
        public int Count => _objects.Count;

        /// <summary>
        /// Clears the quadtree recursively
        /// </summary>
        public void Clear()
        {
            _objects.Clear();

            for (var i = 0; i < _nodes.Length; i++)
            {
                if (_nodes[i] != null)
                {
                    _nodes[i].Clear();
                    _nodes[i] = null;
                }
            }
        }

        /// <summary>
        /// Insert the object into the quad tree. If the node exceeds the capacity, 
        /// it will split and add all objects to their corresponding nodes.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/></param>
        public void Insert(Entity entity)
        {
            if (entity == null)
                return;

            if (_nodes[0] != null)
            {
                var index = GetIndex(entity.GetHitBox());

                if (index != -1)
                {
                    _nodes[index].Insert(entity);
                    return;
                }
            }

            _objects.Add(entity);

            if (_objects.Count > MAX_OBJECTS && level < MAX_LEVELS)
            {
                if (_nodes[0] == null)
                    Split();

                foreach (var entityCurrentNode in _objects.GetRange(0, _objects.Count))
                {
                    var index = GetIndex(entityCurrentNode.GetHitBox());

                    if (index != -1)
                    {
                        _nodes[index].Insert(entityCurrentNode);
                        _objects.Remove(entityCurrentNode);
                    }
                }
            }
        }

        /// <summary>
        /// Deletes the item from this QuadTree. If the object is removed causes the Quadtree to have
        /// no objects in its children, they will also be removed.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/></param>
        public void Delete(Entity entity)
        {
            // If this level contains the object, remove it
            var objectRemoved = false;
            if (_objects != null && _objects.Contains(entity))
            {
                _objects.Remove(entity);
                objectRemoved = true;
            }

            // If we didn't find the object in this tree, try and delete it from its children.
            if (_nodes[0] != null && !objectRemoved)
            {
                _nodes[0].Delete(entity);
                _nodes[1].Delete(entity);
                _nodes[2].Delete(entity);
                _nodes[3].Delete(entity);
            }

            if (_nodes[0] != null)
            {
                // If all the children are empty, delete all the children
                if (_nodes[0].Count == 0 &&
                    _nodes[1].Count == 0 &&
                    _nodes[2].Count == 0 &&
                    _nodes[3].Count == 0)
                {
                    _nodes[0] = null;
                    _nodes[1] = null;
                    _nodes[2] = null;
                    _nodes[3] = null;
                }
            }
        }

        /// <summary>
        /// Returns all objects that could collide with the given object
        /// </summary>
        /// <param name="entity"><see cref="Entity"/></param>
        public IEnumerable<Entity> Retrieve(Entity entity)
        {
            var returnObjects = new List<Entity>(_objects);

            // if we have Subnodes
            if (_nodes[0] != null)
            {
                var index = GetIndex(entity.GetHitBox());

                // If the entity hit box fits into a sub node
                if (index != -1)
                    returnObjects.AddRange(_nodes[index].Retrieve(entity));
                // If the entity hit box does not fit into a sub node, check it against all subnodes
                else
                {
                    for (var i = 0; i < _nodes.Length; i++)
                        returnObjects.AddRange(_nodes[i].Retrieve(entity));
                }
            }

            return returnObjects.Where(_ => _.UniqueId != entity.UniqueId && _.Active && _.IsCollidable);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Clear();
                _objects = null;
            }

            _disposed = true;
        }

        /// <summary>
        /// Splits the node into 4 subnodes, dividing the node into four equal parts and initializing
        /// the four subnodes with the new bounds.
        /// </summary>
        void Split()
        {
            var subWidth = bounds.Width / 2;
            var subHeight = bounds.Height / 2;
            var x = bounds.X;
            var y = bounds.Y;
            var newLevel = level + 1;

            //Right/Top
            _nodes[0] = new Quadtree(new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight), newLevel);

            //Left/Top
            _nodes[1] = new Quadtree(new Rectangle(x, y + subHeight, subWidth, subHeight), newLevel);

            //Left/Bottom
            _nodes[2] = new Quadtree(new Rectangle(x, y, subWidth, subHeight), newLevel);

            //Right/Bottom
            _nodes[3] = new Quadtree(new Rectangle(x + subWidth, y, subWidth, subHeight), newLevel);
        }

        /// <summary>
        /// A helper function of the quadtree. It determines where an object belongs in the quadtree
        /// by determining which node the object can fit into.
        /// </summary>
        /// <param name="hitBox">The rectangle being checked</param>
        /// <returns>The node that the object fits into, -1 means it fits in the parent node</returns>
        int GetIndex(Rectangle hitBox)
        {
            double verticalMidpoint = bounds.X + bounds.Width / 2;
            double horizontalMidpoint = bounds.Y + bounds.Height / 2;

            // Object can completely fit within the top quadrants
            var topQuadrant = hitBox.Y < horizontalMidpoint && hitBox.Y + hitBox.Height < horizontalMidpoint;
            // Object can completely fit within the bottom quadrants
            var bottomQuadrant = hitBox.Y > horizontalMidpoint;

            // Object can completely fit within the left quadrants
            var leftQuadrant = hitBox.X < verticalMidpoint && hitBox.X + hitBox.Width < verticalMidpoint;
            // Object can completely fit within the right quadrants
            var rightQuadrant = hitBox.X > verticalMidpoint;

            if (rightQuadrant && topQuadrant)
                return 0;

            if (leftQuadrant && topQuadrant)
                return 1;

            if (leftQuadrant && bottomQuadrant)
                return 2;

            if (rightQuadrant && bottomQuadrant)
                return 3;

            return -1;
        }
    }
}
