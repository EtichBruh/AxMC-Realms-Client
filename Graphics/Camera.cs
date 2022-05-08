using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxMC.Camera
{
    public static class Camera
    {
        public static Matrix CamTransform { get; set; }
        public static Viewport CamView { get; set; }
        public static float CamRotationDegrees = 0;

        public static float CamZoom = 1f;

        public static void Follow(Vector2 target)
        {
            CamRotationDegrees = CamRotationDegrees >= 360 ? 0 : CamRotationDegrees;
            CamZoom = CamZoom >= 0.2f ? CamZoom : CamZoom = 0.2f;
            CamTransform = Matrix.CreateTranslation(-target.X, -target.Y, 0)
            * Matrix.CreateScale(CamZoom, CamZoom, 1)
            * Matrix.CreateRotationZ(CamRotationDegrees)
            * Matrix.CreateTranslation(CamView.Width * 0.5f, CamView.Height * 0.5f, 0);
        }
    }
}