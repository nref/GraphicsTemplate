using Urho;
using Math = System.Math;

namespace GraphicsTemplate.Urho
{
    /// <summary>
    /// A tool to rotate objects with the mouse naturally
    /// https://en.wikibooks.org/wiki/OpenGL_Programming/Modern_OpenGL_Tutorial_Arcball
    /// </summary>
    public class ArcBall
    {
        private int _lastX = 0;
        private int _lastY = 0;
        private int _curX = 0;
        private int _curY = 0;
        private readonly Input _input;
        private readonly Graphics _graphics;

        private bool MouseLeft => _input.GetMouseButtonDown(MouseButton.Left);

        public ArcBall(Input input, Graphics graphics)
        {
            _input = input;
            _graphics = graphics;

            _input.MouseButtonDown += HandleMouseButtonDown;
            _input.MouseMoved += HandleMouseMoved;
        }

        public Vector3 Rotate(Vector3 position)
        {
            if (_curX == _lastX && _curY == _lastY)
            {
                return position;
            }

            Vector3 va = GetArcballVector(_lastX, _lastY);
            _lastX = _curX;
            _lastY = _curY;

            Vector3 vb = GetArcballVector(_curX, _curY);
            var angle = Vector3.CalculateAngle(va, vb);
            Vector3 axis = Vector3.Cross(va, vb);
            
            return Vector3.TransformVector(position,
                Matrix4.CreateFromAxisAngle(axis, angle));
        }

        private Vector3 GetArcballVector(int x, int y)
        {
            var P = new Vector3(1.0f * x / _graphics.Width * 2 - 1.0f,
                                1.0f * y / _graphics.Height * 2 - 1.0f,
                                0);
            P.Y = -P.Y;
            float OP_squared = P.X * P.X + P.Y * P.Y;
            if (OP_squared <= 1 * 1)
                P.Z = (float)Math.Sqrt(1 * 1 - OP_squared);  // Pythagoras
            else
                P.Normalize();  // nearest point
            return P;
        }

        private void HandleMouseMoved(MouseMovedEventArgs obj)
        {
            if (!MouseLeft)
            {
                return;
            }

            _curX = _input.MousePosition.X;
            _curY = _input.MousePosition.Y;
        }

        private void HandleMouseButtonDown(MouseButtonDownEventArgs obj)
        {
            if (!MouseLeft)
            {
                return;
            }

            _lastX = _input.MousePosition.X;
            _lastY = _input.MousePosition.Y;
            _curX = _lastX;
            _curY = _lastY;
        }
    }
}
