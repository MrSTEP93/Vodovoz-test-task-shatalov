using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Vodovoz.Domain.Entities;
using Vodovoz.Domain.Exceptions;
using Vodovoz.Domain.Interfaces;
using Vodovoz.Services;
using Vodovoz.UI.Common;
using Vodovoz.UI.Views;

namespace Vodovoz.UI.ViewModels
{
    public class OrdersViewModel : ViewModelBase
    {
        private IOrderService _orderService;
        private IEmployeeService _employeeService;
        private IClientService _clientService;
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
        public ICommand DeleteOrderCommand { get; private set; }

        public OrdersViewModel(IOrderService orderService, IEmployeeService employeeService, IClientService clientService)
        {
            _orderService = orderService;
            _employeeService = employeeService;
            _clientService = clientService;

            AddOrderCommand = new RelayCommand(OpenAddOrderWindow);
            EditOrderCommand = new RelayCommand(OpenEditOrderWindow, () => SelectedOrder != null);
            DeleteOrderCommand = new RelayCommand(DeleteOrder, () => SelectedOrder != null);
            LoadOrders();
        }

        private void LoadOrders()
        {
            var list = _orderService.GetAll();
            Orders = new ObservableCollection<Order>(list);
        }

        private void OpenAddOrderWindow()
        {
            var editVm = new OrderEditViewModel(_orderService, _employeeService, _clientService);

            var window = new OrderEditWindow
            {
                DataContext = editVm,
                Owner = Application.Current.MainWindow
            };

            var result = window.ShowDialog();

            if (result == true)
            {
                LoadOrders();
            }
        }

        private void OpenEditOrderWindow()
        {
            var editVm = new OrderEditViewModel(_orderService, _employeeService, _clientService, SelectedOrder);

            var window = new OrderEditWindow
            {
                DataContext = editVm,
                Owner = Application.Current.MainWindow
            };

            var result = window.ShowDialog();

            if (result == true)
            {
                LoadOrders();
            }
        }


        private void DeleteOrder()
        {
            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить заказ от \"{SelectedOrder.Date}\"?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                _orderService.Delete(SelectedOrder.Id);
                LoadOrders();
            }
            catch (BusinessRuleException ex)
            {
                MessageBox.Show(ex.Message, "Невозможно удалить", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка удаления: {ex}");
                MessageBox.Show("Произошла ошибка при удалении", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
