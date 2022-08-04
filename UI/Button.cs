
using AxMC_Realms_Client.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxMC_Realms_Client.UI
{
    public class Button
    {
        static Rectangle Source = new(0, 64, 32, 16);
        public string SetText { set { Text = value; origin = Game1.Arial.MeasureString(value) * .5f; } }
        public Rectangle rect;
        Vector2 origin;
        string Text = "Test";
        bool MouseDown = false;

        public Button(int x, int y, int width, int height)
        {
            rect = new(x, y, width, height);
        }
        public Button()
        {
        }
        public bool Update()
        {
            bool intersects = rect.Intersects(UI.MRect);
            if (Input.MState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && intersects)
            {
                MouseDown = true;
            }
            else if (Input.MState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && MouseDown)
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
            sb.Draw(UI.SlotSprite, rect, Source, c);
            Game1.Arial.MeasureString(Text);
            sb.DrawString(Game1.Arial, Text, rect.Center.ToVector2(), Color.BurlyWood, 0, origin, 0.12f, 0, 0);
        }
    }
}