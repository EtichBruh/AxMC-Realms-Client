using Nez;
using System;

namespace AxMC_Realms_Client.Entity
{
    public class Bag : BasicEntity
    {
        public FastList<int> items;
        //public bool isChoosed;
        public Bag(int x, int y) : base(SRect.Length - 1, x, y)
        {
            SpriteSheetID = 0; // Bag spriteSheet
            Rect.Width = 32;
            Rect.Height = 28;
            items = new(4);
            var rand = new Random();
            if (rand.NextDouble() <= 0.5)
                items.Add(0);
            if (rand.NextDouble() <= 0.5)
                items.Add(1);
            if (rand.NextDouble() <= 0.5)
                items.Add(2);
            items.Add(3);
        }
    }
}