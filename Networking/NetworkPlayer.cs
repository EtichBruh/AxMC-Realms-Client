using Microsoft.Xna.Framework.Graphics;
using nekoT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxMC_Realms_Client.Networking
{
    public class NetworkPlayer : SpriteAtlas
    {
        public NetworkPlayer(Texture2D SpriteSheet): base(SpriteSheet, 3, 5, 0)
        {

        }
    }
}
