using AxMC_Realms_Client.UI;
using Nez;

namespace AxMC_Realms_Client.Entity
{
    public class Bag : BasicEntity
    {
        public FastList<byte> items;
        //public bool isChoosed;
        public Bag(int x, int y) : base(x, y)
        {
            SpriteSheetID = 0; // Bag spriteSheet
            SrcRect = new(0, 0, 16, 14);
            Rect.Width = 32;
            Rect.Height = 28;
            items = new(4);
            items.Add(0);
            items.Add(1);
            items.Add(2);
            items.Add(3);
        }
    }
}