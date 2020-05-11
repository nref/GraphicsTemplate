using System;
using GraphicsTemplate.Adapters;
using GraphicsTemplate.Models;

namespace GraphicsTemplate.ApplicationServices
{
    public interface IGraphicsService
    {
        Size Size { get; set; }
        void HandleKey(Key key, bool pressed);
        void Start(IntPtr hwnd);
        void Run();
        void Close();
        void SetTransform(Guid id, Transform t);
    }

    public class GraphicsService : IGraphicsService
    {
        private readonly IGraphicsAdapter _adapter;

        public GraphicsService(IGraphicsAdapter adapter)
        {
            _adapter = adapter;
        }

        public Size Size 
        { 
            get => _adapter.Size; 
            set => _adapter.Size = value; 
        }

        public void HandleKey(Key key, bool pressed) => _adapter.HandleKey(key, pressed);
        public void Start(IntPtr hwnd) => _adapter.Start(hwnd);
        public void Run() => _adapter.Run();
        public void Close() => _adapter.Close();
        public void SetTransform(Guid id, Transform t) => _adapter.SetTransform(id, t);

    }
}
