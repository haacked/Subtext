using System;
using System.Collections.Generic;
using MbUnit.Framework;
using Ninject;
using Ninject.Syntax;
using Subtext.Web.App_Start;

namespace UnitTests.Subtext.Framework.Infrastructure
{
    [TestFixture]
    public class NinjectMVC3Tests
    {
        [Test]
        public void RegisterServices_DoesNotRegisterSameServiceTwice()
        {
            // arrange
            var kernel = new StubKernel();

            // act
            NinjectMVC3.RegisterServices(kernel);

            // assert
            Assert.GreaterThan(kernel.Types.Count, 1);
        }

        private class StubKernel : IKernel
        {
            IKernel _kernel = new StandardKernel();

            public StubKernel()
            {
                Types = new List<Type>();
            }

            public List<Type> Types { get; private set; }

            public IBindingToSyntax<T> Bind<T>()
            {
                var type = typeof(T);
                if (Types.Contains(type))
                {
                    throw new InvalidOperationException(String.Format("The type {0} is already registered!", type.FullName));
                }
                Types.Add(type);
                return _kernel.Bind<T>();
            }

            public Ninject.Activation.Blocks.IActivationBlock BeginBlock()
            {
                throw new System.NotImplementedException();
            }

            public Ninject.Components.IComponentContainer Components
            {
                get { throw new System.NotImplementedException(); }
            }

            public System.Collections.Generic.IEnumerable<Ninject.Planning.Bindings.IBinding> GetBindings(System.Type service)
            {
                throw new System.NotImplementedException();
            }

            public System.Collections.Generic.IEnumerable<Ninject.Modules.INinjectModule> GetModules()
            {
                throw new System.NotImplementedException();
            }

            public bool HasModule(string name)
            {
                throw new System.NotImplementedException();
            }

            public void Inject(object instance, params Ninject.Parameters.IParameter[] parameters)
            {
                throw new System.NotImplementedException();
            }

            public void Load(System.Collections.Generic.IEnumerable<System.Reflection.Assembly> assemblies)
            {
                throw new System.NotImplementedException();
            }

            public void Load(System.Collections.Generic.IEnumerable<string> filePatterns)
            {
                throw new System.NotImplementedException();
            }

            public void Load(System.Collections.Generic.IEnumerable<Ninject.Modules.INinjectModule> modules)
            {
                throw new System.NotImplementedException();
            }

            public bool Release(object instance)
            {
                throw new System.NotImplementedException();
            }

            public INinjectSettings Settings
            {
                get { throw new System.NotImplementedException(); }
            }

            public void Unload(string name)
            {
                throw new System.NotImplementedException();
            }

            public void AddBinding(Ninject.Planning.Bindings.IBinding binding)
            {
                throw new System.NotImplementedException();
            }

            public Ninject.Syntax.IBindingToSyntax<object> Bind(System.Type service)
            {
                throw new System.NotImplementedException();
            }

            public Ninject.Syntax.IBindingToSyntax<object> Rebind(System.Type service)
            {
                throw new System.NotImplementedException();
            }

            public Ninject.Syntax.IBindingToSyntax<T> Rebind<T>()
            {
                throw new System.NotImplementedException();
            }

            public void RemoveBinding(Ninject.Planning.Bindings.IBinding binding)
            {
                throw new System.NotImplementedException();
            }

            public void Unbind(System.Type service)
            {
                throw new System.NotImplementedException();
            }

            public void Unbind<T>()
            {
                throw new System.NotImplementedException();
            }

            public bool CanResolve(Ninject.Activation.IRequest request)
            {
                throw new System.NotImplementedException();
            }

            public Ninject.Activation.IRequest CreateRequest(System.Type service, System.Func<Ninject.Planning.Bindings.IBindingMetadata, bool> constraint, System.Collections.Generic.IEnumerable<Ninject.Parameters.IParameter> parameters, bool isOptional, bool isUnique)
            {
                throw new System.NotImplementedException();
            }

            public System.Collections.Generic.IEnumerable<object> Resolve(Ninject.Activation.IRequest request)
            {
                throw new System.NotImplementedException();
            }

            public object GetService(System.Type serviceType)
            {
                throw new System.NotImplementedException();
            }

            public bool IsDisposed
            {
                get { throw new System.NotImplementedException(); }
            }

            public void Dispose()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
