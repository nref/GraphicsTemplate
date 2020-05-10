using GraphicsTemplate.Api;
using GraphicsTemplate.Irrlicht;
using GraphicsTemplate.Shared;
using GraphicsTemplate.Urho;
using Ninject;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GraphicsTemplate.Client
{
    public class NinjectBootstrapper<TRootViewModel> : BootstrapperBase where TRootViewModel : class
    {
        private IKernel _kernel;

        private object _rootViewModel;
        protected virtual object RootViewModel
        {
            get { return _rootViewModel ?? (_rootViewModel = GetInstance(typeof(TRootViewModel))); }
        }

        protected override void ConfigureBootstrapper()
        {
            _kernel = new StandardKernel();
            DefaultConfigureIoC(_kernel);
            ConfigureIoC(_kernel);
        }

        /// <summary>
        /// Carries out default configuration of the IoC container. Override if you don't want to do this
        /// </summary>
        protected virtual void DefaultConfigureIoC(IKernel kernel)
        {
            var viewManagerConfig = new ViewManagerConfig()
            {
                ViewFactory = GetInstance,
                ViewAssemblies = new List<Assembly>() { GetType().Assembly }
            };
            kernel.Bind<IViewManager>().ToConstant(new ViewManager(viewManagerConfig));

            kernel.Bind<IWindowManagerConfig>().ToConstant(this).InTransientScope();
            kernel.Bind<IWindowManager>()
                .ToMethod(c => new WindowManager(c.Kernel.Get<IViewManager>(), 
                    () => c.Kernel.Get<IMessageBoxViewModel>(), c.Kernel.Get<IWindowManagerConfig>()))
                .InSingletonScope();
            kernel.Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();
            kernel.Bind<IMessageBoxViewModel>().To<MessageBoxViewModel>(); // Not singleton!
        }

        /// <summary>
        /// Override to add your own types to the IoC container.
        /// </summary>
        protected virtual void ConfigureIoC(IKernel kernel) { }

        public override object GetInstance(Type type)
        {
            return _kernel.Get(type);
        }

        protected override void Launch()
        {
            base.DisplayRootView(RootViewModel);
        }

        public override void Dispose()
        {
            ScreenExtensions.TryDispose(_rootViewModel);
            if (_kernel != null)
                _kernel.Dispose();

            base.Dispose();
        }
    }

    public class Bootstrapper : NinjectBootstrapper<ShellViewModel>
    {
        protected override void OnStart()
        {
            Stylet.Logging.LogManager.Enabled = true;
        }

        protected override void ConfigureIoC(IKernel kernel)
        {
            kernel.Bind<IGraphicsViewModel>().To<GraphicsViewModel>().InSingletonScope();
            kernel.Bind<IGraphicsAdapter, IIrrlichtGraphicsAdapter>().To<IrrlichtGraphicsAdapter>().InSingletonScope();
            //kernel.Bind<IGraphicsAdapter, IUrhoGraphicsAdapter>().To<UrhoGraphicsAdapter>().InSingletonScope();
            kernel.Bind<IGraphicsService>().To<IrrlichtGraphicsService>().InSingletonScope();
            kernel.Bind<IGraphicsApi>().To<ApiCompositionRoot>().InSingletonScope();
        }

        protected override void Configure()
        {
        }
    }
}
