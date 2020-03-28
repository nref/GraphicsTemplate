using GraphicsTemplate.Irrlicht;
using GraphicsTemplate.Shared;
using GraphicsTemplate.Urho;
using Stylet;
using StyletIoC;

namespace GraphicsTemplate.Client
{
    public class Bootstrapper : Bootstrapper<ShellViewModel>
    {
        protected override void OnStart()
        {
            Stylet.Logging.LogManager.Enabled = true;
        }

        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            builder.Bind<IGraphicsViewModel>().To<GraphicsViewModel>().InSingletonScope();
            //builder.Bind<IGraphicsService>().To<IrrlichtGraphicsService>().InSingletonScope();
            builder.Bind<IGraphicsService>().To<UrhoGraphicsSevice>().InSingletonScope();
        }

        protected override void Configure()
        {
        }
    }
}
