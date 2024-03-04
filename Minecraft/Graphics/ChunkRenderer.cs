using Minecraft.Common;
using Minecraft.Game;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Minecraft.Graphics
{
    class ChunkRenderer
    {
        Minecraft instance = Minecraft.GetInstance();
        public Shader chunkShader = new Shader("Assets/Shaders/final.vert", "Assets/Shaders/final.frag");
        public Shader blockOutline = new Shader("Assets/Shaders/outline.vert", "Assets/Shaders/outline.frag");
        public chunkVAO cube;


        public bool cullfaces = true;
        public bool wireframe = false;
        public ChunkRenderer()
        {
            List<float> data = new List<float>();
            data.AddRange(Cube.northFace);
            data.AddRange(Cube.southFace);
            data.AddRange(Cube.westFace);
            data.AddRange(Cube.southFace);
            data.AddRange(Cube.topFace);
            data.AddRange(Cube.bottomFace);

            cube = new chunkVAO(data.ToArray());
        }
        public void Prepare(Camera camera)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            chunkShader.Use();
            chunkShader.SetMatrix4("view", camera.GetViewMatrix());
            chunkShader.SetMatrix4("projection", camera.GetProjectionMatrix(instance.window.aspectRatio));
            blockOutline.Use();
            blockOutline.SetMatrix4("view", camera.GetViewMatrix());
            blockOutline.SetMatrix4("projection", camera.GetProjectionMatrix(instance.window.aspectRatio));
            GL.Enable(EnableCap.DepthTest);
            GL.LineWidth(2);
        }
        public void DrawBlockOutLine(Vector3 blockPos) 
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            blockOutline.SetMatrix4("model", Matrix4.CreateTranslation(blockPos));
            cube.Use();
            GL.DrawArrays(PrimitiveType.Triangles,0,cube.GetSize());
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }
        public void DrawChunk(Chunk chunk, Shader shader)
        {
            if (cullfaces)
            {
                GL.Enable(EnableCap.CullFace);
            }
            else
            {
                GL.Disable(EnableCap.CullFace);
            }

            if (wireframe)
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            }
            else
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }
            chunkShader.Use();
            chunkShader.SetMatrix4("model", Matrix4.CreateTranslation((Vector3)chunk.pos * Chunk.CHUNKSIZE));
            chunk.chunkVAO.Use();
            GL.DrawArrays(PrimitiveType.Triangles, 0, chunk.chunkVAO.GetSize());
        }
    }
}
