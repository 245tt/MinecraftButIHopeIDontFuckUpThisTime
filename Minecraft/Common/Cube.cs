using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft.Common
{
    static class Cube
    {
        public static readonly float[] topFace = new float[]
        {
            -0.5f, 0.5f,-0.5f,   0f, 1f,
            -0.5f, 0.5f, 0.5f,   0f, 0f,
             0.5f, 0.5f, 0.5f,   1f, 0f,

             0.5f, 0.5f,-0.5f,   1f, 1f,
            -0.5f, 0.5f,-0.5f,   0f, 1f,
             0.5f, 0.5f, 0.5f,   1f, 0f,
        };

        public static readonly float[] bottomFace = new float[] 
        {
            -0.5f, -0.5f,-0.5f,   0f, 0f,
             0.5f, -0.5f, 0.5f,   1f, 1f,
            -0.5f, -0.5f, 0.5f,   0f, 1f,
                   
            -0.5f, -0.5f,-0.5f,   0f, 0f,
             0.5f, -0.5f,-0.5f,   1f, 0f,
             0.5f, -0.5f, 0.5f,   1f, 1f,
        };

        public static readonly float[] northFace = new float[] 
        {
            -0.5f, -0.5f,-0.5f,   1f, 0f,
            -0.5f,  0.5f,-0.5f,   1f, 1f,
             0.5f,  0.5f,-0.5f,   0f, 1f,

             0.5f, -0.5f,-0.5f,   0f, 0f,
            -0.5f, -0.5f,-0.5f,   1f, 0f,
             0.5f,  0.5f,-0.5f,   0f, 1f,
        };
        public static readonly float[] southFace = new float[]
        {
            -0.5f, -0.5f, 0.5f,   0f, 0f,
             0.5f,  0.5f, 0.5f,   1f, 1f,
            -0.5f,  0.5f, 0.5f,   0f, 1f,
                          
            -0.5f, -0.5f, 0.5f,   0f, 0f,
             0.5f, -0.5f, 0.5f,   1f, 0f,
             0.5f,  0.5f, 0.5f,   1f, 1f,
        };

        public static readonly float[] westFace = new float[] 
        {
            -0.5f, -0.5f,-0.5f,   0f, 0f,
            -0.5f,  0.5f, 0.5f,   1f, 1f,
            -0.5f,  0.5f,-0.5f,   0f, 1f,

            -0.5f, -0.5f,-0.5f,   0f, 0f,
            -0.5f, -0.5f, 0.5f,   1f, 0f,
            -0.5f,  0.5f, 0.5f,   1f, 1f,
        };
        public static readonly float[] eastFace = new float[]
        {
             0.5f, -0.5f,-0.5f,   1f, 0f,
             0.5f,  0.5f,-0.5f,   1f, 1f,
             0.5f,  0.5f, 0.5f,   0f, 1f,
             
             0.5f, -0.5f, 0.5f,   0f, 0f,
             0.5f, -0.5f,-0.5f,   1f, 0f,
             0.5f,  0.5f, 0.5f,   0f, 1f,
        };
    }
}
