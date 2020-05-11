using GraphicsTemplate.ApplicationServices;
using GraphicsTemplate.Infrastructure;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace GraphicsTemplate.Api
{
    public interface IGraphicsApi
    {
        void Run();
        void Close();
        IGraphicsService Service { get; }
    }

    public class ApiCompositionRoot : CompositionRoot, IGraphicsApi
    {
        public IWebHost Host { get; }
        public IGraphicsService Service => Container.GetInstance<IGraphicsService>();

        public ApiCompositionRoot()
        {
            Host = WebHost.CreateDefaultBuilder()
             .UseLamar(Registry)
             .UseStartup<Startup>()
             .Build();
        }

        public void Run()
        {
            Host.Run();
        }

        public void Close()
        {
            Host.StopAsync();
        }
    }
}
