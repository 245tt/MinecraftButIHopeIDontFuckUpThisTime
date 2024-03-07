using Minecraft.Common;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft.Game.Blocks
{
    class BlockAir : Block
    {
        public override byte GetHardness()
        {
            return 0;
        }

        public override short GetID()
        {
            return 0;
        }

        public override MaterialType Getmaterial()
        {
            return MaterialType.None;
        }

        public override string GetName()
        {
            return string.Empty;
        }

        public override Vector4i GetTextureCoord(Direction direction)
        {
            return base.GetTextureCoord(direction);
        }

        public override bool IsCollidable()
        {
            return false;
        }

        public override bool IsOpaque()
        {
            return false;
        }
    }
}
