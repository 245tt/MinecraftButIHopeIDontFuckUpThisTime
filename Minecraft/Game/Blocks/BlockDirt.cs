using Minecraft.Common;
using OpenTK.Mathematics;

namespace Minecraft.Game.Blocks
{
    internal class BlockDirt : Block
    {
        public override byte GetHardness()
        {
            return 2;
        }
        public override short GetID()
        {
            return 1;
        }
        public override MaterialType Getmaterial()
        {
            return MaterialType.Dirt;
        }

        public override string GetName()
        {
            return "Dirt";
        }

        public override Vector4i GetTextureCoord(Direction direction)
        {
            return base.GetTextureCoord(direction);
        }

        public override bool IsCollidable()
        {
            return true;
        }

        public override bool IsOpaque()
        {
            return true;
        }
    }
}
