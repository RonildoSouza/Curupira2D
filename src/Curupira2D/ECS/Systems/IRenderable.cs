﻿using System.Collections.Generic;

namespace Curupira2D.ECS.Systems
{
    public interface IRenderable : ISystem
    {
        void Draw(ref IReadOnlyList<Entity> entities);
    }
}
