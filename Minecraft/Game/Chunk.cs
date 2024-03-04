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
        public bool isCurrentlyMeshing = false;
        public chunkVAO chunkVAO;
        public Task<List<float>> mesher;
        public static byte numberOfActiveMeshers = 0;
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
        public void SetBlockAt(int x, int y, int z,short blockID)
        {
            blocks[x + CHUNKSIZE * (y + CHUNKSIZE * z)] = blockID;
        }
        public void MeshAsync()
        {
            mesher = Task.Run(() => 
            {
                return ChunkMesh.GenerateMesh(this);
            });

        }
        public List<float> Mesh() 
        {
            return ChunkMesh.GenerateMesh(this);
        }
    }
}
