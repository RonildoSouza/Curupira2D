using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Curupira2D.Diagnostics
{
    public class DebugSystem : ECS.System, ILoadable, IUpdatable
    {
        SpriteFont _diagnosticsFont;
        readonly StringBuilder _stringBuilder = new();
        TextComponent _textComponent;

        public void LoadContent()
        {
            _diagnosticsFont = Scene.GameCore.Content.Load<SpriteFont>("DiagnosticsFont");

            _textComponent = new TextComponent(_diagnosticsFont, _stringBuilder.ToString(), color: Color.Black, layerDepth: 1f, scale: new Vector2(1f));

            Scene.CreateEntity($"***{nameof(DebugSystem)}***", Scene.ScreenCenter, isCollidable: false)
                .AddComponent(_textComponent);
        }

        public void Update()
        {
            _stringBuilder.Clear();

            var entities = Scene.GetEntities(_ => _.Active && _.UniqueId != $"***{nameof(DebugSystem)}***");
            _stringBuilder.Append(DebugEntityProperties(ref entities));
            //_stringBuilder.Append(DebugComponentProperties(ref entities));

            _textComponent.Text = _stringBuilder.ToString();
        }

        static StringBuilder DebugEntityProperties(ref IReadOnlyCollection<Entity> entities)
        {
            var stringBuilder = new StringBuilder();
            var debugEntityProperties = entities
                .Select(_ => new DebugModel<Entity, PropertyInfo>
                {
                    Name = _.UniqueId,
                    MembersInfo = new Dictionary<Entity, IEnumerable<PropertyInfo>>
                    {
                        {
                            _,
                            _.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                .Where(_ => _.Name != nameof(Entity.UniqueId)
                                    && _.Name != nameof(Entity.Children)
                                    && _.Name != nameof(Entity.Parent)
                                    && _.Name != nameof(Entity.Components))
                        }
                    }
                });


            foreach (var debugModel in debugEntityProperties)
                BuildDebugEntityString(ref stringBuilder, debugModel);

            return stringBuilder;
        }

        //StringBuilder DebugComponentProperties(ref IReadOnlyCollection<Entity> entities)
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

        static void BuildDebugEntityString<TKey>(ref StringBuilder stringBuilder, DebugModel<TKey, PropertyInfo> debugModel)
        {
            stringBuilder.AppendLine($"=> ENTITY: {debugModel.Name}");
            foreach (var memInfo in debugModel.MembersInfo)
            {
                foreach (var propInfo in memInfo.Value)
                {
                    var value = propInfo.GetValue(memInfo.Key);
                    stringBuilder.AppendLine($"\r\n=> ------ PROPERTY: {propInfo.Name}: {value}");
                }
            }
        }
    }

    public class DebugModel<TKey, TMemberInfo> where TMemberInfo : MemberInfo
    {
        public string Name { get; set; }
        public IDictionary<TKey, IEnumerable<TMemberInfo>> MembersInfo { get; set; }
    }
}
