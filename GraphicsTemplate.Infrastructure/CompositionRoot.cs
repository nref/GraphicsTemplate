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

        public CompositionRoot()
        {
            Container = new Container(registry =>
            {
                // Bind IAnything to Anything in the RayHospital namespace
                registry.Scan(_ =>
                {
                    _.AssembliesFromApplicationBaseDirectory(a => a.FullName.StartsWith("GraphicsTemplate"));
                    _.WithDefaultConventions();
                });

                Registry = registry;
            });
        }
    }
}
