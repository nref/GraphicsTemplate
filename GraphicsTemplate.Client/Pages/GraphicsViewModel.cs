using GraphicsTemplate.Irrlicht;
using GraphicsTemplate.Shared;
using Stylet;
using System.Windows;

namespace GraphicsTemplate.Client
{
    /// <summary>
    /// Creates a Win32 Window hosted in WPF. Exposes the handle and some events
    /// </summary>
    public interface IGraphicsViewModel
    {
        public IGraphicsViewModel Graphics { get; }

        void HandleKeyDown(object sender, System.Windows.Input.KeyEventArgs e);
        void HandleKeyUp(object sender, System.Windows.Input.KeyEventArgs e);
    }

    public class GraphicsViewModel : Screen, IGraphicsViewModel
    {
        public IGraphicsViewModel Graphics { get; private set; }
        private readonly IGraphicsService _service;

        public GraphicsViewModel(IGraphicsService service)
        {
            _service = service;
        }

        public void HandleKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (_service == null)
                return;

            _service.HandleKey(e.Key, e.IsDown);
        }

        public void HandleKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (_service == null)
                return;

            _service.HandleKey(e.Key, e.IsDown);
        }

        protected override void OnViewLoaded()
        {
            IGraphicsWindow window = (View as GraphicsView).Graphics;

            window.Unloaded += HandleUnloaded;
            window.SizeChanged += HandleSizeChanged;

            _service.Start(window.Handle);
            _service.Run();
        }

        private void HandleUnloaded(object sender, RoutedEventArgs e)
        {
            if (_service == null)
                return;

            _service.Close();
        }

        private void HandleSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_service == null)
                return;

            _service.Size = e.NewSize;
        }
    }
}
