using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.IO;

namespace Minecraft.Graphics
{
    class HUDRender
    {
        Shader crossShader = new Shader("Assets/Shaders/crosshair.vert", "Assets/Shaders/crosshair.frag");
        crossVAO crossVAO;
        Minecraft instance = Minecraft.GetInstance();
        public HUDRender()
        {
            float[] crossData = new float[]
            {
                //left
                -1f,0f,
                 0f,0f,

                 //right
                 1f,0f,
                 0f,0f,

                 //up
                 0f,1f,
                 0f,0f,

                 //down
                 0f,-1f,
                 0f, 0f
            };
            crossVAO = new crossVAO(crossData);
            crossShader.SetVector3("color", new Vector3(0f, 0f, 0f));

        }

        public void Prepare()
        {
            float size = 0.1f;
            crossShader.SetMatrix4("model", Matrix4.CreateScale(size / instance.window.aspectRatio, size, 0));
        }
        public void DrawCross()
        {
            crossShader.Use();
            crossVAO.Use();
            GL.Disable(EnableCap.DepthTest);
            GL.DrawArrays(PrimitiveType.Lines, 0, crossVAO.GetSize());
            GL.Enable(EnableCap.DepthTest);
        }
    }
}
