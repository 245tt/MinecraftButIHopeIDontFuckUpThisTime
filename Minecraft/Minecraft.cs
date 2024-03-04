using Minecraft.Game;
using Minecraft.Game.Physics;
using Minecraft.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace Minecraft
{
    internal class Minecraft
    {
        private Minecraft() { }
        private static Minecraft instance;

        public bool active = true;
        public Window window;
        public ChunkRenderer renderer;
        public DebugRenderer debugRenderer;
        public TextureAtlas textureAtlas;
        public Camera worldCamera;
        public World world;
        Stopwatch stopwatch;
        public double deltaTime;
        public double currentTime;
        public static Minecraft GetInstance()
        {
            if (instance == null)
                instance = new Minecraft();
            return instance;
        }
        public async void Run()
        {
            window = new Window();
            renderer = new ChunkRenderer();
            debugRenderer = new DebugRenderer();
            textureAtlas = new TextureAtlas();
            worldCamera = new Camera(90f);
            Block.RegisterBlocks();
            stopwatch = new Stopwatch();
            worldCamera.Rotation.Y = -90;
            worldCamera.Position.Z = 2f;

            world = new World();
            //Chunk chunk = new Chunk();
            //Chunk chunk2 = new Chunk();
            //Chunk chunk3 = new Chunk();
            //chunk2.pos = new Vector3i(0,0,-1);
            //chunk3.pos = new Vector3i(1,0,0);
            //chunk.north = chunk2;
            //chunk2.south = chunk;
            //chunk.east = chunk3;
            //chunk3.west = chunk;
            //chunk.Mesh();
            //chunk2.Mesh();
            //chunk3.Mesh();
            int placeBlock = 2;
            GL.ClearColor(0, 0, 0.5f, 1.0f);
            stopwatch.Start();
            while (active)
            {
                window.PollEvents();
                float speed = 10f;

                if (window.window.KeyboardState.IsKeyPressed(Keys.Z))
                {
                    renderer.wireframe = !renderer.wireframe;
                }
                if (window.window.KeyboardState.IsKeyPressed(Keys.X))
                {
                    renderer.cullfaces = !renderer.cullfaces;
                }

                if (window.window.KeyboardState.IsKeyDown(Keys.W))
                {
                    worldCamera.Position += worldCamera.lookDir * (float)deltaTime * speed;
                }
                if (window.window.KeyboardState.IsKeyDown(Keys.S))
                {
                    worldCamera.Position -= worldCamera.lookDir * (float)deltaTime * speed;
                }

                if (window.window.KeyboardState.IsKeyDown(Keys.A))
                {
                    worldCamera.Position += worldCamera.rightDir * (float)deltaTime * speed;
                }
                if (window.window.KeyboardState.IsKeyDown(Keys.D))
                {
                    worldCamera.Position -= worldCamera.rightDir * (float)deltaTime * speed;
                }
                worldCamera.Rotation.X -= window.window.MouseState.Delta.Y;
                worldCamera.Rotation.Y += window.window.MouseState.Delta.X;
                Physics.RaycastHit hit;
                Physics.BlockRaycast(worldCamera.Position, worldCamera.lookDir, 10f, out hit);

                //drawing
                renderer.Prepare(worldCamera);
                textureAtlas.atlas.Use();
                if (window.window.KeyboardState.IsKeyDown(Keys.D1)) 
                {
                    placeBlock = 1;
                }
                if (window.window.KeyboardState.IsKeyDown(Keys.D2))
                {
                    placeBlock = 2;
                }
                if (window.window.KeyboardState.IsKeyDown(Keys.D3))
                {
                    placeBlock = 3;
                }

                if (hit.hit)
                {

                    if (window.window.MouseState.IsButtonPressed(MouseButton.Left))
                    {
                        world.SetBlockFromCoord(hit.blockPos, 0);
                    }
                    if (window.window.MouseState.IsButtonPressed(MouseButton.Right))
                    {
                        Vector3i diff = Vector3i.Zero;
                        switch (hit.faceNormal)
                        {
                            case Common.Direction.North:
                                diff = new Vector3i(0, 0, -1);
                                break;
                            case Common.Direction.South:
                                diff = new Vector3i(0, 0, 1);
                                break;
                            case Common.Direction.West:
                                diff = new Vector3i(-1, 0, 0);
                                break;
                            case Common.Direction.East:
                                diff = new Vector3i(1, 0, 0);
                                break;
                            case Common.Direction.Top:
                                diff = new Vector3i(0, 1, 0);
                                break;
                            case Common.Direction.Bottom:
                                diff = new Vector3i(0, -1, 0);
                                break;
                            default:
                                break;
                        }

                        if (world.GetBlockIDFromCoord(hit.blockPos + diff) == BlockList.BLOCK_AIR)
                        {
                            world.SetBlockFromCoord(hit.blockPos + diff, (short)placeBlock);
                        }
                    }
                }

                foreach (Chunk chunk in world.chunks.Values)
                {
                    //start meshing if needed
                    if (chunk.needsRemesh && !chunk.isCurrentlyMeshing)
                    {
                        if (Chunk.numberOfActiveMeshers < 1)
                        {
                            Chunk.numberOfActiveMeshers++;
                            chunk.isCurrentlyMeshing = true;
                            chunk.MeshAsync();
                        }
                    }
                    //await mesh data
                    if (chunk.mesher != null)
                    {
                        if (chunk.mesher.IsCompleted)
                        {
                            List<float> data = await chunk.mesher;
                            if (chunk.chunkVAO != null)
                                chunk.chunkVAO.Dispose();
                            chunk.chunkVAO = new chunkVAO(data.ToArray());
                            data.Clear();
                            data = null;
                            chunk.isMeshed = true;
                            chunk.isCurrentlyMeshing = false;
                            chunk.needsRemesh = false;
                            Chunk.numberOfActiveMeshers--;
                            chunk.mesher = null;
                        }
                    }

                    //draw meshed chunks
                    if (chunk.isMeshed)
                    {
                        renderer.DrawChunk(chunk, renderer.chunkShader);
                    }

                }
                if (hit.hit)
                {
                    renderer.DrawBlockOutLine(hit.blockPos);
                }
                debugRenderer.Prepare(worldCamera);
                debugRenderer.DrawDirections();
                window.SwapBuffers();
                deltaTime = (stopwatch.Elapsed.TotalNanoseconds - currentTime) / 1000000000d;
                //window.window.Title = "FPS:" + ((int)(1d / deltaTime)).ToString();
                currentTime = stopwatch.Elapsed.TotalNanoseconds;

            }
        }

    }
}
