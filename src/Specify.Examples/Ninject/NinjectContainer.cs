﻿using System;
using System.Linq;
using Ninject;
using Ninject.Extensions.ChildKernel;
using Specify.Containers;
using Specify.lib;

namespace Specify.Examples.Ninject
{

    public class NinjectContainer : IContainer
    {
        private IKernel _container;

        public NinjectContainer() : this(new StandardKernel())
        {
            
        }
        public NinjectContainer(IKernel container)
        {
            _container = container;
        }

        protected IKernel Container
        {
            get
            {
                return _container;
            }
        }

        public void Register<T>() where T : class
        {
            Container.Bind<T>().To<T>()
                .InTransientScope()
                .BindingConfiguration.IsImplicit = true;
        }

        // This needs to be lifetime scope per scenario
        public void Register<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            Container.Bind<TService>().To<TImplementation>()
                //.InNamedScope(NinjectDependencyResolver.ScenarioLifetimeScopeTag)
                .InSingletonScope()
                .BindingConfiguration.IsImplicit = true;
        }

        // This needs to be lifetime scope per scenario
        public T Register<T>(T valueToSet, string key = null) where T : class
        {
            if (key == null)
            {
                Container.Bind<T>().ToConstant(valueToSet)
                    //.InNamedScope(NinjectDependencyResolver.ScenarioLifetimeScopeTag)
                    .InSingletonScope()
                    .BindingConfiguration.IsImplicit = true;
            }
            else
            {
                Container.Bind<T>().ToConstant(valueToSet)
                    //.InNamedScope(NinjectDependencyResolver.ScenarioLifetimeScopeTag)
                    .InSingletonScope()
                    .Named(key)
                    .BindingConfiguration.IsImplicit = true;
            }
            return valueToSet;
        }

        public T Resolve<T>(string key = null) where T : class
        {
            if (key == null)
            {
                return Container.Get<T>();
            }
            else
            {
                return Container.Get<T>(key);
            }
        }

        public object Resolve(Type serviceType, string key = null)
        {
            if (key == null)
            {
                return Container.Get(serviceType);
            }
            else
            {
                return Container.Get(serviceType, key);
            }
        }

        public bool CanResolve<T>() where T : class
        {
            return Container.CanResolve<T>();
        }

        public bool CanResolve(Type type)
        {
            return Container.CanResolve(type) != null;
        }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
    public class NinjectDependencyResolver : NinjectContainer, IDependencyResolver
    {
        public const string ScenarioLifetimeScopeTag = "ScenarioLifetime";

        public NinjectDependencyResolver()
        {
            var assemblies = AssemblyTypeResolver.GetAllAssembliesFromAppDomain().ToArray();
            // how to register all modules in Ninject
            //_containerBuilder.RegisterAssemblyModules(assemblies);

        }
        public IContainer CreateChildContainer()
        {
            var childContainer = new ChildKernel(Container);
            return new NinjectContainer(childContainer);
        }
    }

}
