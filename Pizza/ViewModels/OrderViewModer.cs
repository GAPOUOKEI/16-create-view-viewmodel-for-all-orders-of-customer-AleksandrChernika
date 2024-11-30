using Pizza.Models;
using Pizza.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza.ViewModels
{
    public class OrderViewModer : BindableBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly Customer _customer;

        public Guid Id => _customer?.Id ?? Guid.Empty;
        public ObservableCollection<Order> Orders { get; private set; }

        public OrderViewModer(IOrderRepository orderRepository, Customer customer)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _customer = customer ?? new Customer { Id = Guid.Empty }; // Создаем пустого клиента, если null

            Orders = new ObservableCollection<Order>();
            LoadOrders();
        }

        private async void LoadOrders()
        {
            var orders = await _orderRepository.GetOrdersByCustomerIdAsync(_customer.Id);
            Orders = new ObservableCollection<Order>(orders.OrderByDescending(o => o.OrderDate));
        }
    }
}
