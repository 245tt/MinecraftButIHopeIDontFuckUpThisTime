using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft.Game
{
    static class BlockRegistry
    {
        static Dictionary<short,Block> blocks = new Dictionary<short,Block>();

        public static Block GetBlockFromID(short id) 
        {
            Block block = null;
            if(blocks.TryGetValue(id, out block)) 
            {
                return block;
            }
            return GetBlockFromID(-1);
        }
        public static void Register(Block block,short id)
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
    }
}
