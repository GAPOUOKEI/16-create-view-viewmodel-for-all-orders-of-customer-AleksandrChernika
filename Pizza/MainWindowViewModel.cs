using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Pizza.Models;
using Pizza.Services;
using Pizza.ViewModels;
using Pizza.Views;
using Unity;

namespace Pizza
{
    public class MainWindowViewModel : BindableBase
    {
        private AddEditCustomerViewModel _addEditCustomerVewModel;
        private CustomerListViewModel _customerListViewModel;
        private OrderPerpViewModel _orderPrepViewModel;
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private OrderViewModer _orderViewModel;

        // Конструктор без параметров для XAML
        public MainWindowViewModel()
        {
            // Разрешаем зависимости через контейнер Unity
            _orderRepository = RepoContainer.Container.Resolve<IOrderRepository>();
            _customerRepository = RepoContainer.Container.Resolve<ICustomerRepository>();
            _orderViewModel = new OrderViewModer(_orderRepository, null); // Инициализация без клиента (для примера)

            // Инициализация команд или других объектов
            NavigationCommand = new RelayCommand<string>(OnNavigation);

            // Разрешаем зависимости через контейнер
            _customerListViewModel = RepoContainer.Container.Resolve<CustomerListViewModel>();
            _addEditCustomerVewModel = RepoContainer.Container.Resolve<AddEditCustomerViewModel>();

            _customerListViewModel.AddCustomerRequested += NavigationToAddCustomer;
            _customerListViewModel.EditCustomerRequested += NavigationToEditCustomer;
            _customerListViewModel.PlaceOrderRequested += NavigateToOrder;
            _customerListViewModel.ViewOrdersRequested += OnViewOrdersRequested;
        }

        // Свойства и команды

        private BindableBase _currentViewModel;
        public BindableBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public RelayCommand<string> NavigationCommand { get; private set; }

        private void OnNavigation(string dest)
        {
            switch (dest)
            {
                case "orderPrep":
                    CurrentViewModel = _orderPrepViewModel;
                    break;
                case "customers":
                default:
                    CurrentViewModel = _customerListViewModel;
                    break;
            }
        }

        private void NavigationToEditCustomer(Customer customer)
        {
            _addEditCustomerVewModel.IsEditeMode = true;
            _addEditCustomerVewModel.SetCustomer(customer);
            CurrentViewModel = _addEditCustomerVewModel;
        }

        private void NavigationToAddCustomer()
        {
            _addEditCustomerVewModel.IsEditeMode = false;
            _addEditCustomerVewModel.SetCustomer(new Customer { Id = Guid.NewGuid() });
            CurrentViewModel = _addEditCustomerVewModel;
        }

        private void OnViewOrdersRequested(Customer customer)
        {
            if (customer == null)
            {
                MessageBox.Show("Customer is null. Cannot view orders.");
                return;
            }

            var orderViewModel = new OrderViewModer(_orderRepository, customer); // Передаем клиента
            var orderWindow = new Window
            {
                Title = "Заказы клиента",
                Content = new OrderView { DataContext = orderViewModel },
                Width = 800,
                Height = 450
            };

            orderWindow.ShowDialog();
        }

        private void NavigateToOrder(Customer customer)
        {
            if (customer == null)
            {
                MessageBox.Show("Customer is null. Cannot navigate to order.");
                return;
            }

            _orderViewModel = new OrderViewModer(_orderRepository, customer);
            CurrentViewModel = _orderViewModel;
        }
    }
}
