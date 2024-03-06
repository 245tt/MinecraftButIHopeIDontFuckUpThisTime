using OpenTK.Mathematics;

namespace Minecraft.Game
{
    class World
    {
        public Dictionary<Vector3i, Chunk> chunks = new Dictionary<Vector3i, Chunk>();
        WorldGenerator worldGen;
        public static readonly int WORLD_HEIGHT = 10;
        public World()
        {
            worldGen = new WorldGenerator(this, 123);
            int worldSize = 10;
            //for (int i = 0; i < worldSize; i++)
            //{
            //    for (int j = 0; j < worldSize; j++)
            //    {
            //        for (int k = 0; k < worldSize; k++)
            //        {
            //            worldGen.GenerateChunk(new Vector3i(i, j, k));
            //            //Chunk chunk = new Chunk();
            //            //chunk.pos = new Vector3i(i, j, k);
            //            //chunks.Add(new Vector3i(i, j, k), chunk);
            //        }
            //    }
            //}
            //
            //for (int i = 0; i < worldSize; i++)
            //{
            //    for (int j = 0; j < worldSize; j++)
            //    {
            //        for (int k = 0; k < worldSize; k++)
            //        {
            //
            //            Chunk chunk;
            //            Vector3i chunkPos = new Vector3i(i, j, k);
            //            if (chunks.TryGetValue(chunkPos, out chunk)) 
            //            {
            //                chunk.UpdateNeighbors(this);
            //            }
            //            //{
            //            //    Chunk neighbor;
            //            //    if (chunks.TryGetValue(chunkPos + new Vector3i(1, 0, 0), out neighbor))
            //            //    {
            //            //        chunk.east = neighbor;
            //            //    }
            //            //    if (chunks.TryGetValue(chunkPos + new Vector3i(-1, 0, 0), out neighbor))
            //            //    {
            //            //        chunk.west = neighbor;
            //            //    }
            //            //    if (chunks.TryGetValue(chunkPos + new Vector3i(0, 1, 0), out neighbor))
            //            //    {
            //            //        chunk.top = neighbor;
            //            //    }
            //            //    if (chunks.TryGetValue(chunkPos + new Vector3i(0, -1, 0), out neighbor))
            //            //    {
            //            //        chunk.bottom = neighbor;
            //            //    }
            //            //    if (chunks.TryGetValue(chunkPos + new Vector3i(0, 0, -1), out neighbor))
            //            //    {
            //            //        chunk.north = neighbor;
            //            //    }
            //            //    if (chunks.TryGetValue(chunkPos + new Vector3i(0, 0, 1), out neighbor))
            //            //    {
            //            //        chunk.south = neighbor;
            //            //    }
            //            //}
            //        }
            //    }
            //}

        }
        public void LoadChunk(Vector3i pos) 
        {
            if (!chunks.ContainsKey(pos)) 
            {
                worldGen.GenerateChunk(pos);
                Chunk chunk;
                if (chunks.TryGetValue(pos, out chunk)) 
                {
                    chunk.UpdateNeighbors(this);
                }
            }
        }
        public short GetBlockFromCoord(Vector3 pos)
        {
            Vector3 chunkPos = Vector3.Divide(pos, 32);
            chunkPos.X = MathF.Floor(chunkPos.X);
            chunkPos.Y = MathF.Floor(chunkPos.Y);
            chunkPos.Z = MathF.Floor(chunkPos.Z);

            Chunk chunk;
            chunks.TryGetValue((Vector3i)chunkPos, out chunk);
            if (chunk == null)
            {
                return 0;
            }
            //chunk->needsUpdate = true;
            Vector3i blockPos = (Vector3i)(pos - (chunkPos * 32));
            return chunk.GetBlockAt(blockPos.X, blockPos.Y, blockPos.Z);
        }
        public Block GetBlockIDFromCoord(Vector3 pos)
        {
            Vector3 chunkPos = Vector3.Divide(pos, 32);
            chunkPos.X = MathF.Floor(chunkPos.X);
            chunkPos.Y = MathF.Floor(chunkPos.Y);
            chunkPos.Z = MathF.Floor(chunkPos.Z);

            Chunk chunk;
            chunks.TryGetValue((Vector3i)chunkPos, out chunk);
            if (chunk == null)
            {
                return BlockList.BLOCK_AIR;
            }
            //chunk->needsUpdate = true;
            Vector3i blockPos = (Vector3i)(pos - (chunkPos * 32));
            return BlockRegistry.GetBlockFromID(chunk.GetBlockAt(blockPos.X, blockPos.Y, blockPos.Z));
        }

        public void SetBlockFromCoord(Vector3 pos, short blockId)
        {
            Vector3 chunkPos = Vector3.Divide(pos, 32);
            chunkPos.X = MathF.Floor(chunkPos.X);
            chunkPos.Y = MathF.Floor(chunkPos.Y);
            chunkPos.Z = MathF.Floor(chunkPos.Z);

            Chunk chunk;
            chunks.TryGetValue((Vector3i)chunkPos, out chunk);
            if (chunk == null)
            {
                return;
            }
            //chunk->needsUpdate = true;
            Vector3i blockPos = (Vector3i)(pos - (chunkPos * 32));
            chunk.SetBlockAt(blockPos.X, blockPos.Y, blockPos.Z, blockId);
            chunk.needsRemesh = true;
            if (chunk.east != null)
                chunk.east.needsRemesh = true;
            if (chunk.west != null)
                chunk.west.needsRemesh = true;
            if (chunk.north != null)
                chunk.north.needsRemesh = true;
            if (chunk.south != null)
                chunk.south.needsRemesh = true;
            if (chunk.top != null)
                chunk.top.needsRemesh = true;
            if (chunk.bottom != null)
                chunk.bottom.needsRemesh = true;
        }
    }
}