using OpenTK.Mathematics;

namespace Minecraft.Game
{
    class WorldGenerator
    {
        public int seed { get; private set; }
        private World world;
        public WorldGenerator(World world, int seed)
        {
            this.world = world;
            this.seed = seed;
        }
        public void GenerateChunk(Vector3i chunkPos)
        {
            Chunk chunk = new Chunk();
            chunk.pos = chunkPos;
            

            Random random = new Random(seed);

            for (int z = 0; z < Chunk.CHUNKSIZE; z++)
            {
                int Zcoord = z + chunkPos.Z * Chunk.CHUNKSIZE;
                for (int x = 0; x < Chunk.CHUNKSIZE; x++)
                {
                    int Xcoord = x + chunkPos.X * Chunk.CHUNKSIZE;
                    int height = 20 + (int)(MathF.Sin(Xcoord / 10f) * 5);
                    for (int y = 0; y < Chunk.CHUNKSIZE; y++)
                    {
                        int Ycoord = y + chunkPos.Y * Chunk.CHUNKSIZE;

                        if (Ycoord > height)
                        {
                            chunk.SetBlockAt(x, y, z, 0);
                        }
                        else
                        {
                            chunk.SetBlockAt(x, y, z, 1);
                        }
                        if (Ycoord == height)
                        {
                            chunk.SetBlockAt(x, y, z, 4);
                        }
                    }
                }
            }

            world.chunks.Add(chunkPos, chunk);
        }
    }
}
