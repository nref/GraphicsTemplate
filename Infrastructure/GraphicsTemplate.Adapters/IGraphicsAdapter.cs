using System;
using GraphicsTemplate.Models;

namespace GraphicsTemplate.Adapters
{
    public interface IGraphicsAdapter
    {
        Size Size { get; set; }
        void HandleKey(Key key, bool pressed);
        Guid AddMesh(string file);

        /// <summary>
        /// Create a graphics instance inside Window of the given handle.
        /// </summary>
        void Start(IntPtr hwnd);

        /// <summary>
        /// Call from main thread. Blocks until Close() is called.
        /// </summary>
        void Run();
        void Close();

        /// <summary>
        /// Set the transform of the object of the given id
        /// </summary>
        void SetTransform(Guid id, Transform t);
    }
}
