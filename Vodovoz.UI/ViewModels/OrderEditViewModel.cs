using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using Vodovoz.Domain.Entities;
using Vodovoz.Domain.Exceptions;
using Vodovoz.Domain.Interfaces;
using Vodovoz.UI.Common;

namespace Vodovoz.UI.ViewModels
{
    public class OrderEditViewModel : ViewModelBase
    {
        private readonly IOrderService _orderService;
        private readonly IClientService _clientService;
        private readonly IEmployeeService _employeeService;
        private readonly Order _order = null!;
        private bool _isEditMode;

        public IEnumerable<Employee> Employees { get; }
        public IEnumerable<Client> Clients { get; }

        public DateTime? Date { get; set; } = DateTime.Now;
        public Decimal? Sum { get; set; } = 0.00M;
        public Employee Employee { get; set; } = null!;
        public Client Client { get; set; } = null!;

        public int EmployeeId { get; set; } = 0;
        public int ClientId { get; set; } = 0;

        public string Title => _isEditMode ? "Редактирование заказа" : "Добавление заказа";

        public ICommand SaveCommand { get; private set; } = null!;
        public ICommand CancelCommand { get; private set; } = null!;

        public OrderEditViewModel(IOrderService orderService, IEmployeeService employeeService, IClientService clientService)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _clientService = clientService ?? throw new ArgumentNullException(nameof(orderService));

            _isEditMode = false;

            Employees = _employeeService.GetAll();
            Clients = _clientService.GetAll();
            InitializeCommands();
        }

        public OrderEditViewModel(IOrderService orderService, IEmployeeService employeeService, 
            IClientService clientService, Order selectedOrder)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _clientService = clientService ?? throw new ArgumentNullException(nameof(orderService));

            _isEditMode = true;

            Date = selectedOrder.Date;
            Sum = selectedOrder.Sum;
            Employee = selectedOrder.Employee;
            Client = selectedOrder.Client;
            EmployeeId = selectedOrder.Employee.Id;
            ClientId = selectedOrder.Client.Id;

            Employees = _employeeService.GetAll();
            Clients = _clientService.GetAll();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            SaveCommand = new RelayCommand(Save, () => Date.HasValue && Sum.HasValue && Sum.Value > 0);

            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            try
            {
                if (_isEditMode)
                {
                    _order.Date = Date!.Value;
                    _order.Sum = Sum!.Value;
                    _order.Employee = Employees.FirstOrDefault(e => e.Id == EmployeeId)!;
                    _order.Client = Clients.FirstOrDefault(e => e.Id == ClientId)!;
                    _orderService.Save(_order);
                }
                else
                {
                    var newOrder = new Order
                    {
                        Date = Date!.Value,
                        Sum = Sum!.Value,
                        Employee = Employees.FirstOrDefault(e => e.Id == EmployeeId)!,
                        Client = Clients.FirstOrDefault(e => e.Id == ClientId)!,
                    };
                    _orderService.Save(newOrder);
                }
                CloseWindow(true);
            }
            catch (BusinessRuleException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Критическая ошибка: {ex}");
                MessageBox.Show("Произошла непредвиденная ошибка", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel()
        {
            CloseWindow(false);
        }

        private void CloseWindow(bool dialogResult)
        {
            foreach (var window in Application.Current.Windows)
            {
                if (window is Window w && w.DataContext == this)
                {
                    w.DialogResult = dialogResult;
                    w.Close();
                    break;
                }
            }
        }
    }
}
