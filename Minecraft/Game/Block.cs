using Minecraft.Common;
using Minecraft.Game.Blocks;
using OpenTK.Mathematics;

namespace Minecraft.Game
{
    internal class Block
    {
        public Block()
        {

        }
        public virtual byte GetHardness() { return 1; }
        public virtual bool IsOpaque() { return true; }
        public virtual bool IsCollidable() { return true; }
        public virtual MaterialType Getmaterial() { return MaterialType.None; }
        public virtual string GetName() { return "block"; }
        public virtual Vector4i GetTextureCoord(Direction direction) { return new Vector4i(0, 0, 16, 16); }

        public static void RegisterBlocks()
        {

            BlockRegistry.Register(new BlockUnknown(), -1);
            BlockRegistry.Register(new BlockAir(), 0);
            BlockRegistry.Register(new BlockDirt(), 1);
            BlockRegistry.Register(new BlockStone(), 2);
            BlockRegistry.Register(new BlockObsidian(), 3);
        }
    }
}
