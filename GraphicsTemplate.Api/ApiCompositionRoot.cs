using GraphicsTemplate.Infrastructure;
using GraphicsTemplate.Shared;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace GraphicsTemplate.Api
{
    public interface IGraphicsApi
    {
        void Run();
    }

    public class ApiCompositionRoot : CompositionRoot, IGraphicsApi
    {
        public IWebHost Host { get; }

        public ApiCompositionRoot(IGraphicsService service)
        {
            Registry.For<IGraphicsService>().Use(service);
            Host = WebHost.CreateDefaultBuilder()
             .UseLamar(Registry)
             .UseStartup<Startup>()
             .Build();
        }

        public void Run()
        {
            Host.Run();
        }
    }
}
