using Minecraft.Common;
using OpenTK.Mathematics;

namespace Minecraft.Game.Blocks
{
    class BlockGrass : Block
    {
        public override byte GetHardness()
        {
            return 2;
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
            if (direction == Direction.Top)
                return new Vector4i(48, 0, 16, 16);
            if (direction == Direction.North || direction == Direction.South
                || direction == Direction.West || direction == Direction.East)
                return new Vector4i(64, 0, 16, 16);
            return base.GetTextureCoord(direction);
        }

        public override bool IsCollidable()
        {
            return base.IsCollidable();
        }

        public override bool IsOpaque()
        {
            return base.IsOpaque();
        }
    }
}
