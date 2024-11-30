using Pizza.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;

namespace Pizza.Services
{
    public static class RepoContainer
    {
        private static IUnityContainer _container;
        static RepoContainer()
        {
            _container = new UnityContainer();
            _container.RegisterType<ICustomerRepository, CustomerRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IOrderRepository, OrderRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<MainWindowViewModel>(); // Добавить регистрацию для MainWindowViewModel
            _container.RegisterType<CustomerListViewModel>();
            _container.RegisterType<AddEditCustomerViewModel>();
            _container.RegisterType<OrderPerpViewModel>();
            _container.RegisterType<OrderViewModer>();
        }

        public static IUnityContainer Container
        {
            get { return _container; }
        }
    }
}
