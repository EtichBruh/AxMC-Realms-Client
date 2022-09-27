using AxMC.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxMC_Realms_Client.Graphics
{
    public class ProgressBar
    {
        /// <summary>
        /// value like current HP or smth like that, honestly dunno how to name it
        /// </summary>
        public int Progress;
        int height;
        float Factor;
        public Vector2 Pos;
        private Color col;
        bool Rotate = false;
        bool _hor = false;
        public static Texture2D Pixel;

        /// <summary>
        /// Creates a new progress bar 
        /// </summary>
        /// <param name="color">Color of drawn progress bar</param>
        /// <param name="DoRotate">Should progress bar rotate backdwards camera?</param>
        public ProgressBar(Color color, bool DoRotate, bool ProgressFromCenter, int _progress, int h)
        {
            Progress = _progress;
            Rotate = DoRotate;
            col = color;
            _hor = ProgressFromCenter;
            height = h;
        }
        /// <summary>
        /// Makes a pixel for bar
        /// </summary>
        public static void Init(GraphicsDevice GD)
        {
            /*Pixel = new Texture2D(GD, 3, 3);
            Pixel.SetData(new Color[9]
            { Color.Transparent,Color.Transparent,Color.Transparent, //Bar pixel is made like that to apply shader on it
            Color.Transparent,Color.White,Color.Transparent,
            Color.Transparent,Color.Transparent,Color.Transparent
            });*/
            Pixel = new Texture2D(GD, 1, 1);
            Pixel.SetData(new Color[] { Color.White }); // im lazy to load 1 pixel throught content :D
        }
        /// <summary>
        /// Updates Position of Health Bar
        /// </summary>
        public void Update(float TargetX, float TargetY)
        {
            Pos.X = TargetX;
            Pos.Y = TargetY;
        }
        public void SetFactor(int Max, int width)
        {
            Factor = 1f / (Max * (1f / width)); // its made for optimization, max is value needed to scale progress by % ( i guess )
        }
        public void Draw(SpriteBatch SB)
        {
            SB.Draw(Pixel, Pos, null, col, Rotate ? -Camera.RotDegr : 0, _hor ? new(0.5f, -2.75f) : Vector2.Zero, new Vector2(Progress * Factor, height), SpriteEffects.None, 0);
        }
    }
}
