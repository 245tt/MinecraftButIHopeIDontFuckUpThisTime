using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Minecraft.Graphics
{
    class DebugRenderer
    {
        Minecraft instance = Minecraft.GetInstance();
        static float[] directionData = new float[]
        {
            //up
            0f,0f,0f,0f,1f,0f,
            0f,1f,0f,0f,1f,0f,

            //north
            0,0,0,  0,0,1,
            0,0,1,  0,0,1,

            //west
            0,0,0,  1,0,0,
            1,0,0,  1,0,0,
        };

        Shader directionsShader = new Shader("Assets/Shaders/debugdir.vert", "Assets/Shaders/debugdir.frag");
        directionsVAO directions = new directionsVAO(directionData);
        public void Prepare(Camera cam)
        {
            directionsShader.Use();
            directionsShader.SetMatrix4("view", cam.GetViewMatrix().ClearTranslation());
            directionsShader.SetMatrix4("projection", cam.GetProjectionMatrix(instance.window.aspectRatio));
            directionsShader.SetMatrix4("model", Matrix4.CreateScale(0.3f)*Matrix4.CreateTranslation(cam.lookDir));
        }
        public void DrawDirections()
        {
            directionsShader.Use();
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthClamp);
            directions.Use();
            GL.LineWidth(4f);
            GL.DrawArrays(PrimitiveType.Lines, 0, directions.GetSize());
            GL.Disable(EnableCap.DepthClamp);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
        }
    }
}
