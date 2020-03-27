using Stylet;

namespace GraphicsTemplate.Client
{
    public class ShellViewModel : Screen
    {
        public IGraphicsViewModel Graphics {get;}

        public ShellViewModel(IGraphicsViewModel graphics)
        {
            Graphics = graphics;
        }

        public void HandleKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Graphics.HandleKeyDown(sender, e);
        }

        public void HandleKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Graphics.HandleKeyUp(sender, e);
        }
    }
}
