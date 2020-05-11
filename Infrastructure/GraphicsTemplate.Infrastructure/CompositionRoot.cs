using Microsoft.Extensions.Configuration;
using GraphicsTemplate.Adapters;
using GraphicsTemplate.Adapters.Irrlicht;
using GraphicsTemplate.ApplicationServices;
using Lamar;

namespace GraphicsTemplate.Infrastructure
{
    public interface ICompositionRoot
    {
        IContainer Container { get; }
    }

    public class CompositionRoot : ICompositionRoot
    {
        public IContainer Container { get; private set; }
        public ServiceRegistry Registry { get; private set; }
        public DomainConfiguration Configuration { get; private set; }

        public CompositionRoot()
        {
            Configuration = ConfigurationLoader.Load()
                .GetSection("DomainConfiguration")
                .Get<DomainConfiguration>();

            Container = new Container(registry =>
            {
                // Bind IAnything to Anything in the GraphicsTemplate namespace
                registry.Scan(_ =>
                {
                    _.AssembliesFromApplicationBaseDirectory(a => a.FullName.StartsWith("GraphicsTemplate"));
                    _.WithDefaultConventions();
                });

                var graphics = new IrrlichtGraphicsAdapter(Configuration.GraphicsRoot);

                registry.For<IGraphicsAdapter>().Use(graphics);
                registry.For<IGraphicsService>().Use<GraphicsService>();

                Registry = registry;
            });
        }
    }
}
