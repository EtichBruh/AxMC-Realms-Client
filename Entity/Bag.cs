using AxMC_Realms_Client.UI;
using Nez;

namespace AxMC_Realms_Client.Entity
{
    public class Bag : BasicEntity
    {
        public FastList<int> items;
        //public bool isChoosed;
        public Bag(int x, int y) : base(BasicEntity.SRect.Length-1,x, y)
        {
            SpriteSheetID = 0; // Bag spriteSheet
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