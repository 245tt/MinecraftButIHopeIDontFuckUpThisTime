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
        public HUDRender hudRender;
        public TextureAtlas textureAtlas;
        public Camera worldCamera;
        public World world;
        public bool debugMode = false;
        public bool fullscreen = false;
        public List<ChunkMesher> chunkMeshers = new List<ChunkMesher>();
        Stopwatch stopwatch;
        public double deltaTime;
        public double currentTime;
        public static Minecraft GetInstance()
        {
            if (instance == null)
                instance = new Minecraft();
            return instance;
        }
        public void Run()
        {
            window = new Window();
            renderer = new ChunkRenderer();
            debugRenderer = new DebugRenderer();
            hudRender = new HUDRender();
            textureAtlas = new TextureAtlas();
            worldCamera = new Camera(90f);
            BlockRegistry.RegisterBlocks();
            stopwatch = new Stopwatch();
            worldCamera.Rotation.Y = -90;
            worldCamera.Position.Y = 25f;
            world = new World();

            for (int i = 0; i < 5; i++)
            {
                chunkMeshers.Add(new ChunkMesher());
            }

            int placeBlock = 2;
            GL.ClearColor(0, 0, 0.5f, 1.0f);
            stopwatch.Start();
            while (active)
            {
                window.PollEvents();
                float speed = 10f;

                if (window.window.KeyboardState.IsKeyDown(Keys.LeftShift))
                    speed = 50f;

                if (window.window.KeyboardState.IsKeyPressed(Keys.F11))
                {
                    fullscreen = !fullscreen;
                    window.FullScreen(fullscreen);
                }
                //wireframe and face culling
                if (window.window.KeyboardState.IsKeyPressed(Keys.Z))
                {
                    renderer.wireframe = !renderer.wireframe;
                }
                if (window.window.KeyboardState.IsKeyPressed(Keys.X))
                {
                    renderer.cullfaces = !renderer.cullfaces;
                }

                //camera movement
                if (window.window.KeyboardState.IsKeyDown(Keys.W))
                {
                    worldCamera.Position += new Vector3(worldCamera.lookDir.X, 0, worldCamera.lookDir.Z).Normalized() * (float)deltaTime * speed;
                }
                if (window.window.KeyboardState.IsKeyDown(Keys.S))
                {
                    worldCamera.Position -= new Vector3(worldCamera.lookDir.X, 0, worldCamera.lookDir.Z).Normalized() * (float)deltaTime * speed;
                }

                if (window.window.KeyboardState.IsKeyDown(Keys.A))
                {
                    worldCamera.Position += worldCamera.rightDir * (float)deltaTime * speed;
                }
                if (window.window.KeyboardState.IsKeyDown(Keys.D))
                {
                    worldCamera.Position -= worldCamera.rightDir * (float)deltaTime * speed;
                }
                if (window.window.KeyboardState.IsKeyDown(Keys.Space))
                {
                    worldCamera.Position += Vector3.UnitY * (float)deltaTime * speed;
                }
                if (window.window.KeyboardState.IsKeyDown(Keys.LeftControl))
                {
                    worldCamera.Position -= Vector3.UnitY * (float)deltaTime * speed;
                }
                if(fullscreen)
                    window.window.RawMouseInput = true;
                worldCamera.Rotation.X -= window.window.MouseState.Delta.Y;
                worldCamera.Rotation.Y += window.window.MouseState.Delta.X;


                //change placeable blocks
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
                if (window.window.KeyboardState.IsKeyDown(Keys.D4))
                {
                    placeBlock = 4;
                }

                //debug mode
                if (window.window.KeyboardState.IsKeyPressed(Keys.F3))
                {
                    debugMode = !debugMode;
                }

                //raycast
                Physics.RaycastHit hit;
                Physics.BlockRaycast(worldCamera.Position, worldCamera.lookDir, 10f, out hit);
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
                //current chunk position
                Vector3 chunkPos = Vector3.Divide(worldCamera.Position, 32);
                chunkPos.X = MathF.Floor(chunkPos.X);
                chunkPos.Y = MathF.Floor(chunkPos.Y);
                chunkPos.Z = MathF.Floor(chunkPos.Z);

                //unloading too far chunks
                List<Vector3i> chunkToRemove = new List<Vector3i>();
                for (int i = 0; i < world.chunks.Count; i++)
                {
                    Chunk chunk = world.chunks.ElementAt(i).Value;

                    float dist = Vector2.Subtract(chunk.pos.Xz, chunkPos.Xz).Length;
                    if (dist > 10)
                    {
                        chunkToRemove.Add(chunk.pos);
                    }
                }
                foreach (Vector3i pos in chunkToRemove)
                {
                    world.chunks.Remove(pos);
                }
                chunkToRemove.Clear();

                //loading non-loaded chunks
                int maxLoadedChunksPerFrame = 10;
                int currentLoadedChunksPerFrame = 0;
                int renderRadius = 5;
                for (int x = -renderRadius - 1; x < renderRadius + 1; x++)
                {
                    for (int z = -renderRadius - 1; z < renderRadius + 1; z++)
                    {
                        for (int y = 0; y < World.WORLD_HEIGHT; y++)
                        {
                            float distance = MathF.Sqrt((x * x) + (z * z));
                            if (distance <= renderRadius)
                            {
                                Vector3i searchedChunk = new Vector3i(x + (int)chunkPos.X, y, z + (int)chunkPos.Z);
                                if (world.chunks.TryGetValue(searchedChunk, out Chunk chunk))
                                {

                                }
                                else
                                {
                                    if (currentLoadedChunksPerFrame < maxLoadedChunksPerFrame)
                                    {
                                        currentLoadedChunksPerFrame++;
                                        world.LoadChunk(searchedChunk);
                                    }
                                }
                            }
                        }
                    }
                }

                //await mesh
                foreach (ChunkMesher mesher in chunkMeshers)
                {
                    if (mesher.Finished())
                    {
                        mesher.UpdateMesh();
                    }
                }
                //drawing part and eternal suffering
                renderer.Prepare(worldCamera);
                textureAtlas.atlas.Use();

                //chunk rendering and what not
                foreach (Chunk chunk in world.chunks.Values)
                {

                    //start meshing if needed
                    if (chunk.needsRemesh)
                    {
                        foreach (ChunkMesher mesher in chunkMeshers)
                        {
                            if (!mesher.isRunning)
                            {
                                mesher.StartMeshing(chunk);
                                break;
                            }
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
                hudRender.Prepare();
                if (!debugMode)
                    hudRender.DrawCross();
                hudRender.DrawCurrentBlock(BlockRegistry.GetBlockFromID((short)placeBlock).GetName());
                debugRenderer.Prepare(worldCamera);
                if (debugMode)
                {
                    debugRenderer.DrawCoords(worldCamera.Position);
                    debugRenderer.DrawDirections();
                    Chunk chunk;
                    if (world.chunks.TryGetValue((Vector3i)chunkPos, out chunk))
                        debugRenderer.DrawDebugChunk(chunk);
                }
                window.SwapBuffers();
                deltaTime = (stopwatch.Elapsed.TotalNanoseconds - currentTime) / 1000000000d;
                currentTime = stopwatch.Elapsed.TotalNanoseconds;

            }
        }

    }
}
