using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Microsoft.Practices.ServiceLocation;

namespace BuiltToRoam.Lifecycle
{
    public class RootApplication
    {
        public async Task Startup(Action<ContainerBuilder> buildDependencies = null)
        {
            await CommenceStartup();

            var builder = new ContainerBuilder();

            buildDependencies?.Invoke(builder);

            // Perform registrations and build the container.
            var container = builder.Build();

            await BuildCoreDependencies(container);

            // Set the service locator to an AutofacServiceLocator.
            var csl = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => csl);

            await CompleteStartup();
        }

        protected virtual async Task CommenceStartup()
        {
        }

        protected virtual async Task CompleteStartup()
        {
        }

        protected IContainer DependencyContainer { get; private set; }
        protected virtual async Task BuildCoreDependencies(IContainer container)
        {
            DependencyContainer = container;
        }

        public void RegisterDependencies(Action<ContainerBuilder> build)
        {
            var cb = new ContainerBuilder();
            build?.Invoke(cb);
            cb.Update(DependencyContainer);

        }

    }
}
