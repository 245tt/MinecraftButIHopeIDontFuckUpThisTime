using Minecraft.Game.Blocks;

namespace Minecraft.Game
{
    static class BlockRegistry
    {
        static Dictionary<short, Block> blocks = new Dictionary<short, Block>();

        public static Block GetBlockFromID(short id)
        {
            Block block = null;
            if (blocks.TryGetValue(id, out block))
            {
                return block;
            }
            return GetBlockFromID(-1);
        }
        public static void Register(Block block)
        {
            if (blocks.ContainsKey(block.GetID()))
            {
                throw new Exception($"Block with id: {block.GetID()} is arleady registered");
            }
            else
            {
                blocks.Add(block.GetID(), block);
            }
        }
        public static void RegisterBlocks()
        {

            Register(new BlockUnknown());
            Register(new BlockAir());
            Register(new BlockDirt());
            Register(new BlockStone());
            Register(new BlockObsidian());
            Register(new BlockGrass());
        }
    }
}
