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
        public static void Register(Block block, short id)
        {
            if (blocks.ContainsKey(id))
            {
                throw new Exception($"Block with id: {id} is arleady registered");
            }
            else
            {
                blocks.Add(id, block);
            }
        }
        public static void RegisterBlocks()
        {

            Register(new BlockUnknown(), -1);
            Register(new BlockAir(), 0);
            Register(new BlockDirt(), 1);
            Register(new BlockStone(), 2);
            Register(new BlockObsidian(), 3);
            Register(new BlockGrass(), 4);
        }
    }
}
