using IrrlichtNetCore;
using System.Windows.Input;

namespace GraphicsTemplate.Irrlicht
{
    public static class KeyMapper
    {
        public static KeyCode Map(Key key)
        {

            if ((int)key >= (int)Key.A && (int)key <= (int)Key.Z)
            {
                int diff = (int)KeyCode.KeyA - (int)Key.A;
                return (KeyCode)(key - diff);
            }

            return KeyCode.None;
        }
    }
}
