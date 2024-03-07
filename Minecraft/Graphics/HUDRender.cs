using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.IO;

namespace Minecraft.Graphics
{
    class HUDRender
    {
        Shader crossShader = new Shader("Assets/Shaders/crosshair.vert", "Assets/Shaders/crosshair.frag");
        Shader textShader = new Shader("Assets/Shaders/font.vert", "Assets/Shaders/font.frag");
        FontVAO textVAO = new FontVAO(null);
        crossVAO crossVAO;
        Font font = new Font("Assets/Fonts/Font.png", "Assets/Fonts/Font.fnt");
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
        public void DrawCurrentBlock(string text) 
        {
            textVAO.BindData(font.GenerateText(text,new Vector2(0,-.9f),60));
            textShader.Use();
            font.textureAtlas.Use();
            textVAO.Use();
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.DrawArrays(PrimitiveType.Triangles, 0, textVAO.GetSize());
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
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
