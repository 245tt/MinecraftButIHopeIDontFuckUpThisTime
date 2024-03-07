using Minecraft.Graphics;
using OpenTK.Mathematics;

namespace Minecraft.Game
{
    class ChunkMesher
    {
        static TextureAtlas atlas = Minecraft.GetInstance().textureAtlas;
        public bool isRunning { get; private set; }
        private Task<List<float>> mesher;
        private Chunk workingChunk;

        public bool Finished() 
        {
            if(mesher != null)
                if (mesher.IsCompleted && workingChunk != null) return true;
            return false;
        }
        public void StartMeshing(Chunk chunk) 
        {
            workingChunk = chunk;
            isRunning = true;
            mesher = Task.Run(() => 
            {
                return GenerateMesh(chunk);
            });
            
        }
        public void UpdateMesh() 
        {
            workingChunk.isMeshed = true;
            workingChunk.needsRemesh = false;
            if(workingChunk.chunkVAO != null)
                workingChunk.chunkVAO.Dispose();
            workingChunk.chunkVAO = new chunkVAO(mesher.Result.ToArray());
            workingChunk = null;
            isRunning = false;
        }
        List<float> GenerateMesh(Chunk chunk)
        {
            List<float> data = new List<float>(Chunk.CHUNKSIZEPOWER3 * 30 * 6);

            for (int i = 0; i < Chunk.CHUNKSIZEPOWER3; i++)
            {
                int index = i;
                int z = index / (Chunk.CHUNKSIZE * Chunk.CHUNKSIZE);
                index -= (z * Chunk.CHUNKSIZE * Chunk.CHUNKSIZE);
                int y = index / Chunk.CHUNKSIZE;
                int x = index % Chunk.CHUNKSIZE;
                if (BlockRegistry.GetBlockFromID(GetBlockFromPos(chunk, x, y, z)) != BlockList.BLOCK_AIR)
                {
                   
                    if (!BlockRegistry.GetBlockFromID(GetBlockFromPos(chunk, x, y + 1, z)).IsOpaque())
                        data.AddRange(AddTopFace(x, y, z, BlockRegistry.GetBlockFromID(GetBlockFromPos(chunk, x, y, z)).GetTextureCoord(Common.Direction.Top),1.0f));
                    if (!BlockRegistry.GetBlockFromID(GetBlockFromPos(chunk, x, y - 1, z)).IsOpaque())
                        data.AddRange(AddBottomFace(x, y, z, BlockRegistry.GetBlockFromID(GetBlockFromPos(chunk, x, y, z)).GetTextureCoord(Common.Direction.Bottom),0.2f));
                    if (!BlockRegistry.GetBlockFromID(GetBlockFromPos(chunk, x, y, z - 1)).IsOpaque())
                        data.AddRange(AddNorthFace(x, y, z, BlockRegistry.GetBlockFromID(GetBlockFromPos(chunk, x, y, z)).GetTextureCoord(Common.Direction.North), 0.6f));
                    if (!BlockRegistry.GetBlockFromID(GetBlockFromPos(chunk, x, y, z + 1)).IsOpaque())
                        data.AddRange(AddSouthFace(x, y, z, BlockRegistry.GetBlockFromID(GetBlockFromPos(chunk, x, y, z)).GetTextureCoord(Common.Direction.South), 0.6f));
                    if (!BlockRegistry.GetBlockFromID(GetBlockFromPos(chunk, x - 1, y, z)).IsOpaque())
                        data.AddRange(AddWestFace(x, y, z, BlockRegistry.GetBlockFromID(GetBlockFromPos(chunk, x, y, z)).GetTextureCoord(Common.Direction.West), 0.4f));
                    if (!BlockRegistry.GetBlockFromID(GetBlockFromPos(chunk, x + 1, y, z)).IsOpaque())
                        data.AddRange(AddEastFace(x, y, z, BlockRegistry.GetBlockFromID(GetBlockFromPos(chunk, x, y, z)).GetTextureCoord(Common.Direction.East), 0.4f));
                }
            }
            return data;
        }

        private short GetBlockFromPos(Chunk chunk, int x, int y, int z)
        {

            if (x >= 0 && x < Chunk.CHUNKSIZE && y >= 0 && y < Chunk.CHUNKSIZE && z >= 0 && z < Chunk.CHUNKSIZE)
            {
                return chunk.GetBlockAt(x, y, z);
            }
            Chunk workingChunk = chunk;

            int xCoord = x;
            int yCoord = y;
            int zCoord = z;

            if (x >= Chunk.CHUNKSIZE)
            {
                workingChunk = workingChunk.east;
                xCoord -= Chunk.CHUNKSIZE;
            }
            if (x < 0)
            {
                workingChunk = workingChunk.west;
                xCoord += Chunk.CHUNKSIZE;
            }

            if (y >= Chunk.CHUNKSIZE)
            {
                workingChunk = workingChunk.top;
                yCoord -= Chunk.CHUNKSIZE;
            }
            if (y < 0)
            {
                workingChunk = workingChunk.bottom;
                yCoord += Chunk.CHUNKSIZE;
            }

            if (z >= Chunk.CHUNKSIZE)
            {
                workingChunk = workingChunk.south;
                zCoord -= Chunk.CHUNKSIZE;
            }
            if (z < 0)
            {
                workingChunk = workingChunk.north;
                zCoord += Chunk.CHUNKSIZE;
            }

            if (workingChunk != null)
            {
                return workingChunk.GetBlockAt(xCoord, yCoord, zCoord);
            }
            else
            {
                return 0;
            }

        }
        private float[] AddTopFace(int x, int y, int z, Vector4i textureData,float brightness)
        {
            float[] data = new float[]
            {
            -0.5f + x, 0.5f + y,-0.5f + z,   textureData.X/(float)atlas.width,(textureData.Y+textureData.W)/(float)atlas.height,brightness,
            -0.5f + x, 0.5f + y, 0.5f + z,   textureData.X/(float)atlas.width, textureData.Y/(float)atlas.height,brightness,
             0.5f + x, 0.5f + y, 0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, textureData.Y/(float)atlas.height,brightness,

             0.5f + x, 0.5f + y,-0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,brightness,
            -0.5f + x, 0.5f + y,-0.5f + z,   textureData.X/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,brightness,
             0.5f + x, 0.5f + y, 0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, textureData.Y/(float)atlas.height,brightness,
            };
            return data;
        }
        private float[] AddBottomFace(int x, int y, int z, Vector4i textureData, float brightness)
        {
            float[] data = new float[]
            {
             -0.5f + x, -0.5f + y,-0.5f + z,   textureData.X/(float)atlas.width, textureData.Y/(float)atlas.height, brightness,
              0.5f + x, -0.5f + y, 0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,brightness,
             -0.5f + x, -0.5f + y, 0.5f + z,   textureData.X/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,brightness,

             -0.5f + x, -0.5f + y,-0.5f + z,   textureData.X/(float)atlas.width, textureData.Y/(float)atlas.height,brightness,
              0.5f + x, -0.5f + y,-0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, textureData.Y/(float)atlas.height,brightness,
              0.5f + x, -0.5f + y, 0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,brightness,
            };
            return data;
        }

        private float[] AddNorthFace(int x, int y, int z, Vector4i textureData, float brightness)
        {
            float[] data = new float[]
            {
            -0.5f + x, -0.5f + y,-0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, textureData.Y/(float)atlas.height,brightness,
            -0.5f + x,  0.5f + y,-0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,brightness,
             0.5f + x,  0.5f + y,-0.5f + z,   textureData.X/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,brightness,

             0.5f + x, -0.5f + y,-0.5f + z,   textureData.X/(float)atlas.width, textureData.Y/(float)atlas.height,brightness,
            -0.5f + x, -0.5f + y,-0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, textureData.Y/(float)atlas.height,brightness,
             0.5f + x,  0.5f + y,-0.5f + z,   textureData.X/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,brightness,
            };
            return data;
        }
        private float[] AddSouthFace(int x, int y, int z, Vector4i textureData, float brightness)
        {
            float[] data = new float[]
            {
             -0.5f + x, -0.5f + y, 0.5f + z,   textureData.X/(float)atlas.width, textureData.Y/(float)atlas.height,brightness,
              0.5f + x,  0.5f + y, 0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,brightness,
             -0.5f + x,  0.5f + y, 0.5f + z,   textureData.X/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,brightness,
             -0.5f + x, -0.5f + y, 0.5f + z,   textureData.X/(float)atlas.width, textureData.Y/(float)atlas.height,brightness,
              0.5f + x, -0.5f + y, 0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, textureData.Y/(float)atlas.height,brightness,
              0.5f + x,  0.5f + y, 0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,brightness,
            };
            return data;
        }

        private float[] AddWestFace(int x, int y, int z, Vector4i textureData, float brightness)
        {
            float[] data = new float[]
            {
             -0.5f + x, -0.5f + y,-0.5f + z,   textureData.X/(float)atlas.width, textureData.Y/(float)atlas.height,brightness,
             -0.5f + x,  0.5f + y, 0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,brightness,
             -0.5f + x,  0.5f + y,-0.5f + z,   textureData.X/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,brightness,

             -0.5f + x, -0.5f + y,-0.5f + z,   textureData.X/(float)atlas.width, textureData.Y/(float)atlas.height,brightness,
             -0.5f + x, -0.5f + y, 0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, textureData.Y/(float)atlas.height,brightness,
             -0.5f + x,  0.5f + y, 0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,brightness,
            };
            return data;
        }
        private float[] AddEastFace(int x, int y, int z, Vector4i textureData, float brightness)
        {
            float[] data = new float[]
            {
             0.5f + x, -0.5f + y,-0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, textureData.Y/(float)atlas.height,brightness,
             0.5f + x,  0.5f + y,-0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,brightness,
             0.5f + x,  0.5f + y, 0.5f + z,   textureData.X/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,brightness,

             0.5f + x, -0.5f + y, 0.5f + z,   textureData.X/(float)atlas.width, textureData.Y/(float)atlas.height,brightness,
             0.5f + x, -0.5f + y,-0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, textureData.Y/(float)atlas.height,brightness,
             0.5f + x,  0.5f + y, 0.5f + z,   textureData.X/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,brightness,
            };
            return data;
        }
    }
}
