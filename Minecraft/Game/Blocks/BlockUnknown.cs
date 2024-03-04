using Minecraft.Common;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft.Game.Blocks
{
    internal class BlockUnknown : Block
    {
        public BlockUnknown()
        {
        }

        public override string GetName()
        {
            return base.GetName();
        }


        public override bool IsCollidable()
        {
            return base.IsCollidable();
        }

        public override bool IsOpaque()
        {
            return base.IsOpaque();
        }

        public override MaterialType Getmaterial()
        {
            return base.Getmaterial();
        }

        public override byte GetHardness()
        {
            return base.GetHardness();
        }

        public override Vector4i GetTextureCoord(Direction direction)
        {
            return base.GetTextureCoord(direction);
        }
    }
}
