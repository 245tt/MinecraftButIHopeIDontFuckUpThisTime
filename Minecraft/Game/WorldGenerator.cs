using OpenTK.Mathematics;

namespace Minecraft.Game
{
    class WorldGenerator
    {
        public int seed { get; private set; }
        FastNoise noise;
        private World world;
        public WorldGenerator(World world, int seed)
        {
            this.world = world;
            this.seed = seed;
            noise = new FastNoise(seed);
        }
        public void GenerateChunk(Vector3i chunkPos)
        {
            Chunk chunk = new Chunk();
            chunk.pos = chunkPos;

            for (int z = 0; z < Chunk.CHUNKSIZE; z++)
            {
                int Zcoord = z + chunkPos.Z * Chunk.CHUNKSIZE;
                for (int x = 0; x < Chunk.CHUNKSIZE; x++)
                {
                    int Xcoord = x + chunkPos.X * Chunk.CHUNKSIZE;
                    float noiseheight = noise.GetPerlin(Xcoord * 3, Zcoord * 3) + (2 * noise.GetPerlin(Xcoord / 5, Zcoord / 5)) + noise.GetPerlinFractal(Xcoord, Zcoord);
                    int height = (int)(20 + noiseheight * 5);
                    for (int y = 0; y < Chunk.CHUNKSIZE; y++)
                    {
                        int Ycoord = y + chunkPos.Y * Chunk.CHUNKSIZE;

                        if (Ycoord > height)
                        {
                            chunk.SetBlockAt(x, y, z, 0);
                        }
                        else
                        {
                            if (Ycoord >= height - 3)
                                chunk.SetBlockAt(x, y, z, 1);
                            else
                                chunk.SetBlockAt(x, y, z, 2);
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
