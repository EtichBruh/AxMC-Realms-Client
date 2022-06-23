using AxMC_Realms_Client.UI;
using Nez;

namespace AxMC_Realms_Client.Entities
{
    public class Bag : BasicEntity
    {
        public FastList<Item> items;
        //public bool isChoosed;
        public Bag(int x, int y) : base(x, y)
        {
            SpriteSheetID = 0; // Bag spriteSheet
            SrcRect = new(0, 0, 16, 14);
            Rect.Width = 32;
            Rect.Height = 28;
            items = new(4);
            items.Add(new Item(0));
            items.Add(new Item(1));
            items.Add(new Item(2));
            items.Add(new Item(3));
        }
        #region Collision
        /*
        private void GetCollisionMask(Texture2D org, int _x, int _y) // x is position on sprite sheet, same as y
        {
            org.GetData(0, new(_x, _y, SrcRect.Width, SrcRect.Height), collisionMask, 0, collisionMask.Length);
            for(int i =0; i < collisionMask.Length; i++)
            {
                if (collisionMask[i] != 0) collisionMask[i] = 1;
            }
            var lines = new StringBuilder();
            for (var i = 0; i < SrcRect.Height; i++)
            {
                var row = new StringBuilder();
                for (var j = 0; j < SrcRect.Width; j++)
                {
                    row.Append(collisionMask[j + i* SrcRect.Width] !=0 ? "*" : "g");
                }
                lines.AppendLine(row.ToString());
            }
            Console.WriteLine(lines.ToString());

            /*for (int x = 0; x < SrcRect.Width; x++)
            {
                for (int y = 0; y < SrcRect.Height; y++)
                {
                    Console.WriteLine(x);
                    int i = x + y * SrcRect.Width;
                     if (y > 0 && x % 16 == 0)
                    {
                        somedebug[i] = '\n';
                    }
                    else if (collisionMask[i] != 0) somedebug[i] = '#';

                }
            }
            // Console.WriteLine(somedebug);
        }*/
        #endregion

    }
}