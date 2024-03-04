using OpenTK.Mathematics;

namespace Minecraft.Graphics
{
    public class Camera
    {
        public Vector3 Position;
        public Vector3 Rotation; //pitch //yaw //roll
        public Vector3 lookDir;
        public Vector3 rightDir;
        float fov;
        public Camera(float fov)
        {
            this.fov = MathHelper.DegreesToRadians(fov);
            Position = Vector3.Zero;
            Rotation = Vector3.Zero;
            Rotation.Y = -MathHelper.PiOver2; // without this camera would be rotated 90degrees;
        }

        public Matrix4 GetViewMatrix()
        {
            Rotation.X = Math.Clamp(Rotation.X, -89.99f, 89.99f);
            lookDir.X = (float)Math.Cos(MathHelper.DegreesToRadians(Rotation.X)) * (float)Math.Cos(MathHelper.DegreesToRadians(Rotation.Y));
            lookDir.Y = (float)Math.Sin(MathHelper.DegreesToRadians(Rotation.X));
            lookDir.Z = (float)Math.Cos(MathHelper.DegreesToRadians(Rotation.X)) * (float)Math.Sin(MathHelper.DegreesToRadians(Rotation.Y));
            lookDir.Normalize();
            rightDir = Vector3.Cross(Vector3.UnitY, lookDir).Normalized();
            return Matrix4.LookAt(Position, Position + lookDir, Vector3.UnitY);
        }

        public Matrix4 GetProjectionMatrix(float aspectRatio)
        {
            return Matrix4.CreatePerspectiveFieldOfView(fov, aspectRatio, 0.1f, 10000f);
        }
    }
}
