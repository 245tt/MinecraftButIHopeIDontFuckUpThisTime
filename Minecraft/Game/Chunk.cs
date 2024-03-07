using Minecraft.Graphics;
using OpenTK.Mathematics;

namespace Minecraft.Game
{
    class Chunk
    {
        public static readonly int CHUNKSIZE = 32;
        public static readonly int CHUNKSIZEPOWER2 = CHUNKSIZE * CHUNKSIZE;
        public static readonly int CHUNKSIZEPOWER3 = CHUNKSIZEPOWER2 * CHUNKSIZE;

        //neighbors
        public Chunk north; // neg z
        public Chunk south; // pos z
        public Chunk east; // pos x
        public Chunk west; //neg x
        public Chunk bottom; //neg y
        public Chunk top; // pos y

        short[] blocks = new short[CHUNKSIZEPOWER3];

        public Vector3i pos;
        public bool isMeshed = false;
        public bool needsRemesh = true;
        public chunkVAO chunkVAO;
        public Chunk()
        {
            for (int i = 0; i < CHUNKSIZEPOWER3; i++)
            {
                blocks[i] = 1;
            }
            blocks[5 + CHUNKSIZE * (5 + CHUNKSIZE * 5)] = 0;
        }
        public short GetBlockAt(int x, int y, int z)
        {
            return blocks[x + CHUNKSIZE * (y + CHUNKSIZE * z)];
        }
        public void SetBlockAt(int x, int y, int z, short blockID)
        {
            blocks[x + CHUNKSIZE * (y + CHUNKSIZE * z)] = blockID;
        }
        public void UpdateNeighbors(World world)
        {
            Chunk neighbor;
            if (world.chunks.TryGetValue(pos + new Vector3i(1, 0, 0), out neighbor))
            {
                this.east = neighbor;
                this.east.west = this;
                this.east.needsRemesh = true;
            }
            if (world.chunks.TryGetValue(pos + new Vector3i(-1, 0, 0), out neighbor))
            {
                this.west = neighbor;
                this.west.east = this;
                this.west.needsRemesh = true;
            }
            if (world.chunks.TryGetValue(pos + new Vector3i(0, 1, 0), out neighbor))
            {
                this.top = neighbor;
                this.top.bottom = this;
                this.top.needsRemesh = true;
            }
            if (world.chunks.TryGetValue(pos + new Vector3i(0, -1, 0), out neighbor))
            {
                this.bottom = neighbor;
                this.bottom.top = this;
                this.bottom.needsRemesh = true;
            }
            if (world.chunks.TryGetValue(pos + new Vector3i(0, 0, -1), out neighbor))
            {
                this.north = neighbor;
                this.north.south = this;
                this.north.needsRemesh = true;
            }
            if (world.chunks.TryGetValue(pos + new Vector3i(0, 0, 1), out neighbor))
            {
                this.south = neighbor;
                this.south.north = this;
                this.south.needsRemesh = true;
            }

        }
    }
}
