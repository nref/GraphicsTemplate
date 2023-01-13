using GraphicsTemplate.Models;
using Irrlicht;

namespace GraphicsTemplate.Adapters.Irrlicht
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

            switch (key)
            {
                case Key.Space:
                    return KeyCode.Space;

                default:
                    return KeyCode.None;
            }
        }
    }
}
