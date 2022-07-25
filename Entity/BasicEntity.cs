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
        public static Rectangle[] SRect = new Rectangle[] {
            new(0,0,8,8),
            new(8,0,9,8),
            new(17,0,9,8),
            new(26,0,9,8),
            new(35,0,9,8),
            new(44,0,9,8),
            new(0,8,6,9),
            new(6,8,7,11),
            new(13,8,11,13),
            new(24,8,12,14),
            new(36,8,16,14),
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
            sb.Draw(SpriteSheet[0], Rect, SrcRect, Color.White, -Camera.RotDegr, SrcRect.Size.ToVector2() * .5f, 0, 0);
        }
    }
}
