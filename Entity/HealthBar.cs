using AxMC_Realms_Client.Graphics;
using Microsoft.Xna.Framework;

namespace AxMC_Realms_Client.Entity
{
    public class HealthBar : ProgressBar
    {
        public HealthBar(int CurrentHp) :
            base(Color.Red, true, true, CurrentHp, 8)
        {
        }
    }
}
