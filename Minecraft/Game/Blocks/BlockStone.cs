using Minecraft.Common;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft.Game.Blocks
{
    class BlockStone : Block
    {
        public override byte GetHardness()
        {
            return 5;
        }

        public override MaterialType Getmaterial()
        {
            return MaterialType.Stone;
        }

        public override string GetName()
        {
            return "Stone";
        }

        public override Vector4i GetTextureCoord(Direction direction)
        {
            return new Vector4i(16,0,16,16);
        }

        public override bool IsCollidable()
        {
            return true;
        }
    }
}
