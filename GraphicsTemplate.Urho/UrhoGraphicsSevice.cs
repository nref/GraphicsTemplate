using GraphicsTemplate.Shared;
using System;
using System.IO;
using System.Windows;
using Urho;
using Key = System.Windows.Input.Key;

namespace GraphicsTemplate.Urho
{
    public class UrhoGraphicsSevice : IGraphicsService
    {
        private UrhoApp _app;

        public Size Size { get; set; }

        public void Start(IntPtr hwnd)
        {
            bool d3d = true;
            bool win32 = IntPtr.Size != 8;
            var dll = $@"Win{(win32 ? "32" : "64")}_{(d3d ? "DirectX" : "OpenGL")}/mono-urho.dll";
            _ = Kernel32.LoadLibrary(dll);

            Directory.CreateDirectory("Data");
            Directory.CreateDirectory("CoreData");

            _app = new UrhoApp(new ApplicationOptions { ExternalWindow = hwnd });
        }

        public void AddMesh(string file)
        {
        }

        public void Run()
        {
            _app.Run();
        }

        public void Close()
        {
            _app.Exit();
        }

        public void HandleKey(Key key, bool pressed)
        {
        }
    }
}
