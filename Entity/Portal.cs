namespace AxMC_Realms_Client.Entity
{
    public class Portal : BasicEntity
    {
        public Portal(int id, int x, int y) : base(id, x, y)
        {
            SpriteSheetID = 1; // Portal spritesheet
            Rect.Width = SrcRect.Width * 4;
            Rect.Height = SrcRect.Height * 4;
        }
    }
}
