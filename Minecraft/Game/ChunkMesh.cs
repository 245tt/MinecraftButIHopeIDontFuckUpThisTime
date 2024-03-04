using OpenTK.Mathematics;

namespace Minecraft.Game
{
    static class ChunkMesh
    {
        static TextureAtlas atlas = Minecraft.GetInstance().textureAtlas;
        public static List<float> GenerateMesh(Chunk chunk)
        {
            chunk.isCurrentlyMeshing = true;
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
                    Vector4i texdata = BlockRegistry.GetBlockFromID(GetBlockFromPos(chunk, x, y, z)).GetTextureCoord(Common.Direction.Top);
                    if (!BlockRegistry.GetBlockFromID(GetBlockFromPos(chunk, x, y + 1, z)).IsOpaque())
                        data.AddRange(AddTopFace(x, y, z, texdata));
                    if (!BlockRegistry.GetBlockFromID(GetBlockFromPos(chunk, x, y - 1, z)).IsOpaque())
                        data.AddRange(AddBottomFace(x, y, z, texdata));
                    if (!BlockRegistry.GetBlockFromID(GetBlockFromPos(chunk, x, y, z - 1)).IsOpaque())
                        data.AddRange(AddNorthFace(x, y, z, texdata));
                    if (!BlockRegistry.GetBlockFromID(GetBlockFromPos(chunk, x, y, z + 1)).IsOpaque())
                        data.AddRange(AddSouthFace(x, y, z, texdata));
                    if (!BlockRegistry.GetBlockFromID(GetBlockFromPos(chunk, x - 1, y, z)).IsOpaque())
                        data.AddRange(AddWestFace(x, y, z, texdata));
                    if (!BlockRegistry.GetBlockFromID(GetBlockFromPos(chunk, x + 1, y, z)).IsOpaque())
                        data.AddRange(AddEastFace(x, y, z, texdata));
                }
            }
            //if (chunk.chunkVAO != null)
            //    chunk.chunkVAO.Dispose();
            //chunk.chunkVAO = new chunkVAO(data.ToArray());
            chunk.isCurrentlyMeshing = false;
            return data;
        }

        private static short GetBlockFromPos(Chunk chunk, int x, int y, int z)
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
        private static float[] AddTopFace(int x, int y, int z, Vector4i textureData)
        {
            float[] data = new float[]
            {
            -0.5f + x, 0.5f + y,-0.5f + z,   textureData.X/(float)atlas.width,(textureData.Y+textureData.W)/(float)atlas.height,
            -0.5f + x, 0.5f + y, 0.5f + z,   textureData.X/(float)atlas.width, textureData.Y/(float)atlas.height,
             0.5f + x, 0.5f + y, 0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, textureData.Y/(float)atlas.height,

             0.5f + x, 0.5f + y,-0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,
            -0.5f + x, 0.5f + y,-0.5f + z,   textureData.X/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,
             0.5f + x, 0.5f + y, 0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, textureData.Y/(float)atlas.height,
            };
            return data;
        }
        private static float[] AddBottomFace(int x, int y, int z, Vector4i textureData)
        {
            float[] data = new float[]
            {
             -0.5f + x, -0.5f + y,-0.5f + z,   textureData.X/(float)atlas.width, textureData.Y/(float)atlas.height,
              0.5f + x, -0.5f + y, 0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,
             -0.5f + x, -0.5f + y, 0.5f + z,   textureData.X/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,

             -0.5f + x, -0.5f + y,-0.5f + z,   textureData.X/(float)atlas.width, textureData.Y/(float)atlas.height,
              0.5f + x, -0.5f + y,-0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, textureData.Y/(float)atlas.height,
              0.5f + x, -0.5f + y, 0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,
            };
            return data;
        }

        private static float[] AddNorthFace(int x, int y, int z, Vector4i textureData)
        {
            float[] data = new float[]
            {
            -0.5f + x, -0.5f + y,-0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, textureData.Y/(float)atlas.height,
            -0.5f + x,  0.5f + y,-0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,
             0.5f + x,  0.5f + y,-0.5f + z,   textureData.X/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,

             0.5f + x, -0.5f + y,-0.5f + z,   textureData.X/(float)atlas.width, textureData.Y/(float)atlas.height,
            -0.5f + x, -0.5f + y,-0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, textureData.Y/(float)atlas.height,
             0.5f + x,  0.5f + y,-0.5f + z,   textureData.X/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,
            };
            return data;
        }
        private static float[] AddSouthFace(int x, int y, int z, Vector4i textureData)
        {
            float[] data = new float[]
            {
             -0.5f + x, -0.5f + y, 0.5f + z,   textureData.X/(float)atlas.width, textureData.Y/(float)atlas.height,
              0.5f + x,  0.5f + y, 0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,
             -0.5f + x,  0.5f + y, 0.5f + z,   textureData.X/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,
             -0.5f + x, -0.5f + y, 0.5f + z,   textureData.X/(float)atlas.width, textureData.Y/(float)atlas.height,
              0.5f + x, -0.5f + y, 0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, textureData.Y/(float)atlas.height,
              0.5f + x,  0.5f + y, 0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,
            };
            return data;
        }

        private static float[] AddWestFace(int x, int y, int z, Vector4i textureData)
        {
            float[] data = new float[]
            {
             -0.5f + x, -0.5f + y,-0.5f + z,   textureData.X/(float)atlas.width, textureData.Y/(float)atlas.height,
             -0.5f + x,  0.5f + y, 0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,
             -0.5f + x,  0.5f + y,-0.5f + z,   textureData.X/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,

             -0.5f + x, -0.5f + y,-0.5f + z,   textureData.X/(float)atlas.width, textureData.Y/(float)atlas.height,
             -0.5f + x, -0.5f + y, 0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, textureData.Y/(float)atlas.height,
             -0.5f + x,  0.5f + y, 0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,
            };
            return data;
        }
        private static float[] AddEastFace(int x, int y, int z, Vector4i textureData)
        {
            float[] data = new float[]
            {
             0.5f + x, -0.5f + y,-0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, textureData.Y/(float)atlas.height,
             0.5f + x,  0.5f + y,-0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,
             0.5f + x,  0.5f + y, 0.5f + z,   textureData.X/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,

             0.5f + x, -0.5f + y, 0.5f + z,   textureData.X/(float)atlas.width, textureData.Y/(float)atlas.height,
             0.5f + x, -0.5f + y,-0.5f + z,   (textureData.X+textureData.Z)/(float)atlas.width, textureData.Y/(float)atlas.height,
             0.5f + x,  0.5f + y, 0.5f + z,   textureData.X/(float)atlas.width, (textureData.Y+textureData.W)/(float)atlas.height,
            };
            return data;
        }
    }
}
