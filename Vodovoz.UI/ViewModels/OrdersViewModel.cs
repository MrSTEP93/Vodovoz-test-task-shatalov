using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Vodovoz.Domain.Entities;
using Vodovoz.Domain.Interfaces;
using Vodovoz.UI.Common;

namespace Vodovoz.UI.ViewModels
{
    public class OrdersViewModel : ViewModelBase
    {
        private IOrderService _orderService;
        private ObservableCollection<Order> _orders = null!;
        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set => SetProperty(ref _orders, value);
        }

        private Order _selectedOrder = null!;
        public Order SelectedOrder
        {
            get => _selectedOrder;
            set => SetProperty(ref _selectedOrder, value);
        }

        public ICommand AddOrderCommand { get; private set; }

        public ICommand EditOrderCommand { get; private set; }

        public OrdersViewModel(IOrderService orderService)
        {
            _orderService = orderService;
            //AddOrderCommand = new RelayCommand(OpenAddOrderWindow);
            //EditOrderCommand = new RelayCommand(OpenEditOrderWindow, () => SelectedOrder != null);
            LoadOrders();
        }

        private void LoadOrders()
        {
            var list = _orderService.GetAll();
            Orders = new ObservableCollection<Order>(list);
        }
    }
}
