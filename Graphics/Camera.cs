using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxMC.Camera
{
    public static class Camera
    {
        public static Matrix Transform { get; set; }
        public static Viewport View { get; set; }

        public static float RotDegr; // Rotation Degrees in radians

        public static float CamZoom = 1f;

        public static void Follow(Vector2 target)
        {
            
            Transform = Matrix.CreateTranslation(-target.X, -target.Y, 0)
            * Matrix.CreateScale(CamZoom, CamZoom, 1)
            * Matrix.CreateRotationZ(RotDegr)
            * Matrix.CreateTranslation(View.Width * .5f, View.Height * .5f, 0);
        }
    }
}