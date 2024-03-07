using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Minecraft
{
    internal class Window
    {
        public NativeWindow window;
        public float aspectRatio;
        public Window()
        {
            NativeWindowSettings settings = new NativeWindowSettings()
            {
                ClientSize = new Vector2i(1600, 900),
                Title = "\"NullReferenceException\" we did a fucky wucky  K Y S",
            };
            window = new NativeWindow(settings);
            window.Closing += Window_Closing;
            window.Resize += Window_Resize;
            window.CursorState = CursorState.Grabbed;
            aspectRatio = (float)window.Size.X / (float)window.Size.Y;
            //window.VSync = VSyncMode.On;
        }
        public void FullScreen(bool fullscreen) 
        {
            if (fullscreen)
                window.WindowState = WindowState.Fullscreen;
            else 
                window.WindowState = WindowState.Normal;
        }
        private void Window_Resize(ResizeEventArgs obj)
        {
            GL.Viewport(0, 0, obj.Width, obj.Height);
            aspectRatio = (float)obj.Width / (float)obj.Height;

        }

        private void Window_Closing(System.ComponentModel.CancelEventArgs obj)
        {
            Minecraft.GetInstance().active = false;
        }

        public void SwapBuffers()
        {
            window.Context.SwapBuffers();
        }
        public void PollEvents()
        {
            window.ProcessEvents(0);
            //GLFW.PollEvents();
        }
    }
}
