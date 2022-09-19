using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curupira2D.Diagnostics
{
    public class DebugOptions
    {
        public bool DebugActive { get; set; }
        public bool DebugWithUICamera2D { get; set; }
        public Color TextColor { get; set; } = Color.Black;
    }
}
