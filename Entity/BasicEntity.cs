using AxMC.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace AxMC_Realms_Client.Entity
{

    public abstract class BasicEntity
    {
        public static Texture2D[] SpriteSheet;
        /// <summary>
        /// interactable entities
        /// </summary>
        public static FastList<BasicEntity> InteractEnt = new();
        /// <summary>
        /// nearest interactable entity ID in InteractEnt list
        /// </summary>
        public static int NInteract = -1;
        readonly static Rectangle[] SRect = new Rectangle[] {
            new(0,0,8,8),
            new(8,0,9,8),
            new(0,0,6,9),
            new(0,9,7,11),
            new(7,0,11,13),
            new(7,13,12,15),
        };
        public Rectangle SrcRect;
        public Rectangle Rect;
        public int id;
        protected int SpriteSheetID = 0;// By default its bag 
        public BasicEntity(int x, int y)
        {
            Rect.X = x;
            Rect.Y = y;
        }
        public BasicEntity(int id, int x, int y)
        {
            SrcRect = SRect[id];
            Rect.X = x;
            Rect.Y = y;
        }
        public static void Add(BasicEntity ent)
        {
            InteractEnt.Add(ent);
        }
        public static BasicEntity GetNear()
        {
            return NInteract == -1 ? null : InteractEnt[NInteract];
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(SpriteSheet[SpriteSheetID], Rect, SrcRect, Color.White, -Camera.RotDegr, SrcRect.Size.ToVector2() * .5f, 0, 0);
        }
    }
}
