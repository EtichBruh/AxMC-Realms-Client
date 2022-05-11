using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxMC.Camera
{
    public static class Camera
    {
        public static Matrix Transform { get; set; }
        public static Viewport View { get; set; }

        public static float RotDegr { get; set; } // Rotation Degrees in radians

        public static float CamZoom = 1f;

        public static void Follow(Vector2 target)
        {
            RotDegr = RotDegr >= 360 ? 0 : RotDegr;
            CamZoom = CamZoom >= 0.2f ? CamZoom : CamZoom = 0.2f;
            Transform = Matrix.CreateTranslation(-target.X, -target.Y, 0)
            * Matrix.CreateScale(CamZoom, CamZoom, 1)
            * Matrix.CreateRotationZ(RotDegr)
            * Matrix.CreateTranslation(View.Width * 0.5f, View.Height * 0.5f, 0);
        }
    }
}