using Curupira2D.ECS.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Curupira2D.Diagnostics
{
    public class DebugSystem : ECS.System, ILoadable, IRenderable
    {
        SpriteFont _fontArial18;
        readonly StringBuilder _stringBuilder = new StringBuilder();

        public void LoadContent()
        {
            //_fontArial18 = Scene.Content.Load<SpriteFont>("FontArial18");
        }

        public void Draw()
        {
            var entities = Scene.GetEntities(_ => _.Active);

            _stringBuilder.Clear();

            //_stringBuilder.Append(DebugEntityProperties(ref entities));
            //_stringBuilder.Append(DebugComponentProperties(ref entities));

            Scene.SpriteBatch.DrawString(_fontArial18, _stringBuilder, Vector2.One, Color.Black,
                0f, Vector2.Zero, .5f, SpriteEffects.None, 0f);
        }

        //StringBuilder DebugEntityProperties(ref List<Entity> entities)
        //{
        //    var debugEntityProperties = entities
        //        .Select(_ => new DebugModel<Entity, PropertyInfo>
        //        {
        //            Name = _.UniqueId,
        //            MembersInfo = _.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
        //            .Where(p => p.IsDefined(typeof(DebugInspectionAttribute)))
        //            .ToDictionary(v => _, v => new List<PropertyInfo> { v }.AsEnumerable())
        //        });

        //    var stringBuilder = new StringBuilder();

        //    foreach (var debugModel in debugEntityProperties)
        //        BuildDebugEntityString(ref stringBuilder, debugModel);

        //    return stringBuilder;
        //}

        //StringBuilder DebugComponentProperties(ref List<Entity> entities)
        //{
        //    var debugEntityComponentProperties = entities
        //        .Select(_ => new DebugModel<IComponent, PropertyInfo>
        //        {
        //            Name = _.UniqueId,
        //            MembersInfo = _.GetComponents()
        //                .ToDictionary(v => v,
        //                v => v.GetType()
        //                      .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
        //                      .Where(p => p.IsDefined(typeof(DebugInspectionAttribute))))
        //        });

        //    var stringBuilder = new StringBuilder();

        //    foreach (var debugModel in debugEntityComponentProperties)
        //        BuildDebugEntityString(ref stringBuilder, debugModel);

        //    return stringBuilder;
        //}

        //void BuildDebugEntityString<TKey>(ref StringBuilder stringBuilder, DebugModel<TKey, PropertyInfo> debugModel)
        //{
        //    foreach (var memInfo in debugModel.MembersInfo)
        //    {
        //        foreach (var propInfo in memInfo.Value)
        //        {
        //            var value = propInfo.GetValue(memInfo.Key);
        //            stringBuilder.AppendLine($"=> ENTITY: {debugModel.Name}\n------ PROPERTY: {propInfo.Name}: {value}");
        //        }
        //    }
        //}
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class DebugInspectionAttribute : Attribute { }

    public class DebugModel<TKey, TMemberInfo> where TMemberInfo : MemberInfo
    {
        public string Name { get; set; }
        public IDictionary<TKey, IEnumerable<TMemberInfo>> MembersInfo { get; set; }
    }
}
