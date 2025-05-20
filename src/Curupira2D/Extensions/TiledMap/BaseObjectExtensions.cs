using Curupira2D.ECS;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TiledLib;
using TiledLib.Layer;
using TiledLib.Objects;

namespace Curupira2D.Extensions.TiledMap
{
    public static class BaseObjectExtensions
    {
        public static T Get<T>(this ObjectLayer objectLayer, string name) where T : BaseObject
            => objectLayer.GetAll<T>().FirstOrDefault(_ => _.Name == name);

        public static T Get<T>(this ObjectLayer objectLayer, int id) where T : BaseObject
            => objectLayer.GetAll<T>().FirstOrDefault(_ => _.Id == id);

        public static IEnumerable<T> GetAll<T>(this ObjectLayer objectLayer) where T : BaseObject
            => objectLayer.Objects.OfType<T>();

        public static IEnumerable<T> GetAll<T>(this ObjectLayer objectLayer, Func<T, bool> predicate) where T : BaseObject
            => objectLayer.Objects.OfType<T>().Where(predicate);

        public static Vector2 ToVector2(this BaseObject baseObject, Map map, Scene scene)
        {
            if (map.Orientation == Orientation.isometric)
            {
                var positionOffset = baseObject.GetIsometricOffsetPositionY(map);
                return new Vector2((float)(baseObject.X - baseObject.Y), scene.InvertPositionY((float)(baseObject.X + baseObject.Y)) + positionOffset);
            }

            return new Vector2((float)baseObject.X, scene.InvertPositionY((float)baseObject.Y));
        }

        internal static float GetIsometricOffsetPositionY(this BaseObject baseObject, Map map)
        {
            var totalTilesAbovePosition = (float)(baseObject.X + baseObject.Y) / map.CellWidth;
            return totalTilesAbovePosition * map.CellHeight + map.CellHeight * 0.5f;
        }
    }
}
