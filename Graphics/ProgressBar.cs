using AxMC.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nekoT;

namespace AxMC_Realms_Client.Graphics
{
    public class ProgressBar
    {
        /// <summary>
        /// value like current HP or smth like that, honestly dunno how to name it
        /// </summary>
        public int Progress;
        float Factor;
        private Vector2 Pos;
        public static Texture2D Pixel;

        public ProgressBar()
        {
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
            Pixel.SetData(new Color[] {Color.White}); // im lazy to load 1 pixel throught content :D
        }
        /// <summary>
        /// Updates Position of Health Bar
        /// TargetY equal targetY + half of target size Height
        /// </summary>
        public void Update(float TargetX, float TargetY)
        {
            Pos.X = TargetX;
            Pos.Y = TargetY + 10;
        }
        public void SetFactor(int Max)
        {
            Factor = 1f/ (Max *.03f); // its made for optimization, max is value needed to scale progress by % ( i guess )
        }
        public void Draw(SpriteBatch SB)
        {
            SB.Draw(Pixel, Pos, null,Color.Red, -Camera.RotDegr,Vector2.One*.5f, new Vector2(Progress*Factor, 8), SpriteEffects.None,0);
        }
    }
}
