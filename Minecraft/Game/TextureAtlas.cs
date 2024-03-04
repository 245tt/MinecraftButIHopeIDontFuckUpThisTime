using Minecraft.Graphics;
using OpenTK.Mathematics;

namespace Minecraft.Game
{
    class TextureAtlas
    {
        public Texture atlas;
        public int width;
        public int height;
        public TextureAtlas()
        {
            atlas = new Texture("Assets/Textures/atlas.png");
            width = atlas.Width;
            height = atlas.Height;
        }
    }
}
