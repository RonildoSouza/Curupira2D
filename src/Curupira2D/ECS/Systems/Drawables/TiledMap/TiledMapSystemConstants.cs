namespace Curupira2D.ECS.Systems.Drawables
{
    public struct TiledMapSystemConstants
    {
        public struct Properties
        {
            public const string EntityUniqueId = "c2D.entityUniqueId";
            public const string EntityGroup = "c2D.entityGroup";
            public const string Order = "c2D.order";

            public struct Physics
            {
                public const string Restitution = "c2D.physics.restitution";
                public const string Friction = "c2D.physics.friction";
            }
        }
    }
}
