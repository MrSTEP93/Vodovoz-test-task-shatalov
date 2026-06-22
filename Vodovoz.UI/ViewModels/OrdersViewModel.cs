using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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

        public OrdersViewModel(IOrderService orderService)
        {
            _orderService = orderService;
            LoadOrders();
        }

        private void LoadOrders()
        {
            var list = _orderService.GetAll();
            Orders = new ObservableCollection<Order>(list);
        }
    }
}
