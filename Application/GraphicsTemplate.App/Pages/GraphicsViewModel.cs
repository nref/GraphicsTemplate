using GraphicsTemplate.Api;
using Stylet;
using System.Threading.Tasks;
using System.Windows;

namespace GraphicsTemplate.App
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
        private readonly IGraphicsApi _api;

        public GraphicsViewModel(IGraphicsApi api)
        {
            _api = api;
            Task.Run(() => _api.Run());
        }

        public void HandleKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (_api.Service == null)
                return;

            _api.Service.HandleKey((Models.Key)e.Key, e.IsDown);
        }

        public void HandleKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (_api.Service == null)
                return;

            _api.Service.HandleKey((Models.Key)e.Key, e.IsDown);
        }

        protected override void OnViewLoaded()
        {
            IGraphicsWindow window = (View as GraphicsView).Graphics;

            window.Unloaded += HandleUnloaded;
            window.SizeChanged += HandleSizeChanged;

            _api.Service.Start(window.Handle);
            _api.Service.Run();
        }

        private void HandleUnloaded(object sender, RoutedEventArgs e)
        {
            if (_api.Service == null)
                return;

            _api.Service.Close();
        }

        private void HandleSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_api.Service == null)
                return;

            _api.Service.Size = new Models.Size
            {
                Width = e.NewSize.Width,
                Height = e.NewSize.Height
            };
        }
    }
}
