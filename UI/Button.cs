
using AxMC_Realms_Client.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxMC_Realms_Client.UI
{
    public class Button
    {
        public string SetText { set { Text = value; origin = Game1.Arial.MeasureString(value) * .5f; } }
        static Rectangle Source = new(0, 32, 32, 16);
        public Vector2 Position;
        Vector2 origin;
        string Text = "Test";
        bool MouseDown = false;

        public Button(int x,int y)
        {
            Position = new(x,y);
        }
        public bool Update()
        {
            var m = Input.MState;
            bool intersects =
                m.X >= Position.X &&
                m.Y >= Position.Y &&
                m.X <= Position.X + 32 &&
                m.Y <= Position.Y + 32;

            if (m.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && intersects)
            {
                MouseDown = true;
            }
            else if (m.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && MouseDown)
            {
                MouseDown = false;
                if (intersects)
                {
                    return true;
                }
            }
            return false;
        }
        public void Draw(SpriteBatch sb)
        {
            var c = MouseDown ? Color.Gray : Color.White;
            sb.Draw(UI.SlotSprite, Position, Source, c);
            Game1.Arial.MeasureString(Text);
            sb.DrawString(Game1.Arial, Text, Position, Color.BurlyWood, 0, origin, 0.12f, 0, 0);
        }
    }
}