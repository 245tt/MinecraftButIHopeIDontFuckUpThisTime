using Minecraft.Common;
using OpenTK.Mathematics;

namespace Minecraft.Game.Blocks
{
    internal class BlockObsidian : Block
    {
        public override byte GetHardness()
        {
            return 20;
        }
        public override short GetID()
        {
            return 3;
        }
        public override MaterialType Getmaterial()
        {
            return MaterialType.Stone;
        }

        public override string GetName()
        {
            return "Obsidian";
        }

        public override Vector4i GetTextureCoord(Direction direction)
        {
            return new Vector4i(32,0,16,16);
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
