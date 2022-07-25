namespace AxMC_Realms_Client.Entity
{
    class Obstacle : BasicEntity
    {
        bool Collide = false;
        public Obstacle(int id, int x, int y) : base(id + 4, x, y)
        {
            Rect.Width = SrcRect.Width * 4;
            Rect.Height = SrcRect.Height * 4;
            SpriteSheetID = 2;
        }
    }
}
