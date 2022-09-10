using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace AxMC_Realms_Client.Classes
{
    public struct Input
    {
        public static Keys MoveUp, MoveRight, MoveLeft, MoveDown, RotateCameraLeft, RotateCameraRight, ResetRotation, ZoomOut, ZoomIn;
        public static MouseState MState;
        public static KeyboardState PKState,KState;
        public static void setKeys()
        {
            MoveUp = Keys.W;
            MoveRight = Keys.D;
            MoveLeft = Keys.A;
            MoveDown = Keys.S;
            RotateCameraLeft = Keys.Q;
            RotateCameraRight = Keys.E;
            ResetRotation = Keys.Z;
            ZoomIn = Keys.OemPlus;
            ZoomOut = Keys.OemMinus;
        }
    }
}
