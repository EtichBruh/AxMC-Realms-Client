using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxMC_Realms_Client.Graphics
{
    public class ProgressBar
    {
        /// <summary>
        /// Value in % of 100
        /// set to 100 by default
        /// </summary>
        public int ProgressValue = 100;
        private Rectangle ProgressRect = new(0, 0, 100, 30);
        private static Texture2D Pixel;
        /// <summary>
        /// Making a pixel for bar if none was created before
        /// </summary>
        public ProgressBar(GraphicsDevice GD)
        {
            if(Pixel.Width < 3) {
                Pixel = new Texture2D(GD, 3, 3);
                Pixel.SetData(new Color[9]
                { Color.Transparent,Color.Transparent,Color.Transparent, //Bar pixel is made like that to apply shader on it
            Color.Transparent,Color.White,Color.Transparent,
            Color.Transparent,Color.Transparent,Color.Transparent
                });
            }
        }
        /// <summary>
        /// Updates Position of Health Bar
        /// TargetY equal targetY + half of targetHeight
        /// </summary>
        public void Update(int TargetX, int TargetY)
        {
            ProgressRect.X = TargetX - (ProgressRect.Width / 2);
            ProgressRect.Y = TargetY;

        }
        public void Draw(SpriteBatch SB)
        {
            SB.Draw(Pixel, ProgressRect, Color.Red);
        }
    }
}
