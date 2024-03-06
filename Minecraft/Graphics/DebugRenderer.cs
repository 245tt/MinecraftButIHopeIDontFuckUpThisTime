using Minecraft.Game;
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
        Shader textShader = new Shader("Assets/Shaders/font.vert", "Assets/Shaders/font.frag");
        FontVAO fontVAO = new FontVAO(null);
        directionsVAO directions = new directionsVAO(directionData);
        Font font = new Font("Assets/Fonts/Font.png", "Assets/Fonts/Font.fnt");
        public void Prepare(Camera cam)
        {
            directionsShader.Use();
            directionsShader.SetMatrix4("view", cam.GetViewMatrix().ClearTranslation());
            directionsShader.SetMatrix4("projection", cam.GetProjectionMatrix(instance.window.aspectRatio));
            directionsShader.SetMatrix4("model", Matrix4.CreateScale(0.3f) * Matrix4.CreateTranslation(cam.lookDir));
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
        public void DrawChunkBorders(Vector3i pos) 
        {

        }
        public void DrawDebugChunk(Chunk chunk)
        {
            string chunkWest = "null";
            string chunkEast = "null";
            string chunkNorth = "null";
            string chunkSouth = "null";
            string chunkTop = "null";
            string chunkBottom = "null";
            if (chunk.west != null)
                chunkWest = chunk.west.pos.ToString();
            if (chunk.east != null)
                chunkEast = chunk.east.pos.ToString();
            if (chunk.north != null)
                chunkNorth = chunk.north.pos.ToString();
            if (chunk.south != null)
                chunkSouth = chunk.south.pos.ToString();
            if (chunk.top != null)
                chunkTop = chunk.top.pos.ToString();
            if (chunk.bottom != null)
                chunkBottom = chunk.bottom.pos.ToString();
            string text = $"Current Chunk Pos: {chunk.pos}\n" +
                $"West Chunk: {chunkWest}\n" +
                $"East Chunk: {chunkEast}\n" +
                $"North Chunk: {chunkNorth}\n" +
                $"South Chunk: {chunkSouth}\n" +
                $"Top Chunk: {chunkTop}\n" +
                $"Bottom Chunk: {chunkBottom}\n";
            fontVAO.BindData(font.GenerateText(text, new Vector2(-1, .1f), 30));

            textShader.Use();
            font.textureAtlas.Use();
            fontVAO.Use();
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);
            GL.PolygonMode(MaterialFace.FrontAndBack,PolygonMode.Fill);
            GL.DrawArrays(PrimitiveType.Triangles, 0, fontVAO.GetSize());
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
        }
    }
}
