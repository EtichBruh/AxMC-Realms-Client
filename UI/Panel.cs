using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxMC_Realms_Client.UI
{
    
    public class Panel
    {
        //corners
        static Rectangle[] Corner = new Rectangle[] {
            new(0, 0, 6, 15),
            new(51, 0, 6, 15),
            new(0, 53, 5, 11),
            new(52, 53, 5, 11)
        };
        //panel box
        static Rectangle box = new(3, 4, 1, 1);// 3 29
        static Rectangle boox = new(4, 0, 1, 2);
        static Rectangle boxx = new(3, 29, 1, 2);
        static Rectangle bbox = new(3, 15, 2, 6);
        static Rectangle bbbox = new(3, 15, 1, 6);

        public static void Draw(SpriteBatch sb, Texture2D tex, Vector2 pos, int width, int height)
        {
            pos.X -= Corner[0].Width;
            pos.Y -= Corner[0].Height;
            width += Corner[0].Width;
            height += Corner[0].Height;

            sb.Draw(tex, new Rectangle((int)pos.X+5, (int)pos.Y + 4, width-5, 2), boox, Color.White);
            sb.Draw(tex, new Rectangle((int)pos.X+5, (int)pos.Y + 4 + 2, width-5, height - 4), box, Color.White);
            sb.Draw(tex, new Rectangle((int)pos.X+5, (int)pos.Y + height+2, width-5, 2), boxx, Color.White);
            sb.Draw(tex, new Rectangle((int)pos.X+3, (int)pos.Y + 14, bbox.Width, height-18), bbox, Color.White);
            sb.Draw(tex, new Rectangle((int)pos.X+width, (int)pos.Y + 14, 1, height-18), bbbox, Color.White);

            sb.Draw(tex, pos, Corner[0], Color.White);
            sb.Draw(tex, pos + new Vector2(width-2, 0), Corner[1], Color.White);
            sb.Draw(tex, pos + new Vector2(0, height-4), Corner[2], Color.White);
            sb.Draw(tex, pos + new Vector2(width-1, height-4), Corner[3], Color.White);
        }
    }
}
