﻿using System;
using System.Windows.Input;
using System.Windows;

namespace GraphicsTemplate.Shared
{
    public interface IGraphicsService
    {
        Size Size { get; set; }
        void HandleKey(Key key, bool pressed);
        void AddMesh(string file);

        /// <summary>
        /// Create a graphics instance inside Window of the given handle.
        /// </summary>
        void Start(IntPtr hwnd);

        /// <summary>
        /// Call from main thread. Blocks until Close() is called.
        /// </summary>
        void Run();
        void Close();
    }
}
