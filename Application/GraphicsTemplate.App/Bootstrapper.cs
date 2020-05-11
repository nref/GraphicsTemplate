using GraphicsTemplate.Api;
using Stylet;
using StyletIoC;

namespace GraphicsTemplate.App
{
    public class Bootstrapper : Bootstrapper<ShellViewModel>
    {
        protected override void OnStart()
        {
            Stylet.Logging.LogManager.Enabled = true;
        }

        protected override void ConfigureIoC(IStyletIoCBuilder kernel)
        {
            kernel.Bind<IGraphicsViewModel>().To<GraphicsViewModel>().InSingletonScope();
            kernel.Bind<IGraphicsApi>().To<ApiCompositionRoot>().InSingletonScope();
        }

        protected override void Configure()
        {
        }
    }
}
