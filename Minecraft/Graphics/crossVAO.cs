using OpenTK.Graphics.OpenGL4;

namespace Minecraft.Graphics
{
    class crossVAO
    {
        public int vao { get; private set; }
        int vbo;
        int size = 0;
        public crossVAO(float[] vertices)
        {
            vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();
            //ebo = GL.GenBuffer();

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer,ebo);

            if (vertices != null)
            {
                size = vertices.Length / 2;
                GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            }
            //vertex pos
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0 * sizeof(float));
            GL.EnableVertexAttribArray(0);


            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        public void BindData(float[] vertices)
        {
            size = vertices.Length;

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

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
        }
    }
}
