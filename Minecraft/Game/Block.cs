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
        public virtual short GetID() { return -1; }
        public virtual byte GetHardness() { return 1; }
        public virtual bool IsOpaque() { return true; }
        public virtual bool IsCollidable() { return true; }
        public virtual MaterialType Getmaterial() { return MaterialType.None; }
        public virtual string GetName() { return "block"; }
        public virtual Vector4i GetTextureCoord(Direction direction) { return new Vector4i(0, 0, 16, 16); }

    }
}
