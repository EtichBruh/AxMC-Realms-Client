
using AxMC_Realms_Client.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxMC_Realms_Client.UI
{
    public class Button
    {
        static Rectangle srect = new(0,32,32,16);
        public string Text;
        public Vector2 pos;
        bool MouseDown = false;

        public Button()
        {

        }
        public void Update()
        {
            if(Input.MState)
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(UI.SlotSprite, pos, srect,MouseDown ? Color.Gray:Color.White);
            sb.DrawString(Game1.Arial, Text, pos, Color.White);
        }
    }
}