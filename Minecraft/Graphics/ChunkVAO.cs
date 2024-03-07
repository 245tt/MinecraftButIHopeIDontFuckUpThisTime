using OpenTK.Graphics.OpenGL4;

namespace Minecraft.Graphics
{
    internal class chunkVAO : IDisposable
    {
        public int vao {  get; private set; }
        int vbo;
        int size = 0;
        //int ebo;
        public chunkVAO(float[] vertices)
        {

            

            vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();
            //ebo = GL.GenBuffer();

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer,ebo);

            if (vertices != null)
            {
                size = vertices.Length/6;
                GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);
            }
            //vertex pos
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0 * sizeof(float));
            GL.EnableVertexAttribArray(0);

            //tex coord
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            //brightness
            GL.VertexAttribPointer(2, 1, VertexAttribPointerType.Float, false, 6 * sizeof(float), 5 * sizeof(float));
            GL.EnableVertexAttribArray(2);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        public void BindData(float[] vertices)
        {
            size = vertices.Length;

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Use()
        {
            GL.BindVertexArray(vao);
        }
        public int GetSize()
        {
            return size;
        }

        public void Dispose()
        {
            GL.DeleteBuffer(vbo);
            GL.DeleteVertexArray(vao);
            vao = 0;
            vbo = 0;
            size = 0;
        }
    }
}
