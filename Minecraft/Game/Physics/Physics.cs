using Minecraft.Common;
using OpenTK.Mathematics;

namespace Minecraft.Game.Physics
{
    static class Physics
    {
        static Minecraft instance = Minecraft.GetInstance();
        static World world = instance.world;


        public struct RaycastHit
        {
            public bool hit;
            public float distance;
            public Vector3 hitPos;
            public Vector3i blockPos;
            public Direction faceNormal;
            public short blockID;
        }
        public static void BlockRaycast(Vector3 origin, Vector3 direction, float distance, out RaycastHit hit)
        {
            hit = new RaycastHit();
            World world = instance.world;
            float stepDist = 0.1f;
            int numberOfSteps = (int)(distance / stepDist);

            Vector3 dir = direction.Normalized();
            Vector3 currentPoint = origin + new Vector3(0.5f);
            for (int i = 0; i < numberOfSteps; i++)
            {
                hit.distance += stepDist;

                //move x 
                currentPoint.X += dir.X * stepDist;

                short blockId = world.GetBlockFromCoord(currentPoint);
                if (BlockRegistry.GetBlockFromID(blockId).IsCollidable())
                {
                    hit.hit = true;
                    if (dir.X > 0)
                    {
                        hit.faceNormal = Direction.West;
                    }
                    else
                    {
                        hit.faceNormal = Direction.East;
                    }
                    hit.blockID = blockId;
                    break;
                }

                //move y 
                currentPoint.Y += dir.Y * stepDist;

                blockId = world.GetBlockFromCoord(currentPoint);
                if (BlockRegistry.GetBlockFromID(blockId).IsCollidable())
                {
                    hit.hit = true;
                    if (dir.Y > 0)
                    {
                        hit.faceNormal = Direction.Bottom;
                    }
                    else
                    {
                        hit.faceNormal = Direction.Top;
                    }
                    hit.blockID = blockId;
                    break;
                }
                //move z 
                currentPoint.Z += dir.Z * stepDist;

                blockId = world.GetBlockFromCoord(currentPoint);
                if (BlockRegistry.GetBlockFromID(blockId).IsCollidable())
                {
                    hit.hit = true;
                    if (dir.Z > 0)
                    {
                        hit.faceNormal = Direction.North;
                    }
                    else
                    {
                        hit.faceNormal = Direction.South;
                    }
                    hit.blockID = blockId;
                    break;
                }

            }
            if (hit.hit)
            {
                hit.hitPos = currentPoint;
                Vector3 blockPos = currentPoint;
                blockPos.X = MathF.Floor(blockPos.X);
                blockPos.Y = MathF.Floor(blockPos.Y);
                blockPos.Z = MathF.Floor(blockPos.Z);
                hit.blockPos = (Vector3i)blockPos;
            }

        }
    }
}
