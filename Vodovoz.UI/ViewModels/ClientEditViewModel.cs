using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Vodovoz.Domain.Entities;
using Vodovoz.Domain.Enums;
using Vodovoz.Domain.Exceptions;
using Vodovoz.Domain.Interfaces;
using Vodovoz.Services;
using Vodovoz.UI.Common;

namespace Vodovoz.UI.ViewModels
{
    public class ClientEditViewModel : ViewModelBase
    {
        private readonly IClientService _clientService;
        private readonly IEmployeeService _employeeService;
        private readonly Client _client = null!;
        private bool _isEditMode;

        public IEnumerable<Employee> Employees { get; }

        public string Name { get; set; } = string.Empty;
        public string Inn { get; set; } = string.Empty;
        public Employee Curator { get; set; } = null!;

        public int CuratorId { get; set; } = 0;

        public string Title => _isEditMode ? "Редактирование контрагента" : "Добавление контрагента";

        public ICommand SaveCommand { get; private set; } = null!;
        public ICommand CancelCommand { get; private set; } = null!;

        public ClientEditViewModel(IClientService clientService, IEmployeeService employeeService)
        {
            _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _isEditMode = false;

            Employees = _employeeService.GetAll();
            InitializeCommands();
        }

        public ClientEditViewModel(IClientService clientService, IEmployeeService employeeService, Client client)
        {
            _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _client = client;
            _isEditMode = true;

            Name = client.Name;
            Inn = client.Inn;
            Curator = client.Curator;
            CuratorId = client.Curator.Id;

            Employees = _employeeService.GetAll();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            SaveCommand = new RelayCommand(Save, () =>
                !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Inn));

            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            try
            {
                if (_isEditMode)
                {
                    _client.Name = Name;
                    _client.Inn = Inn;
                    _client.Curator = Employees.FirstOrDefault(e => e.Id == CuratorId)!;
                    _clientService.Save(_client);
                }
                else
                {
                    var newClient = new Client
                    {
                        Name = Name,
                        Inn = Inn,
                        Curator = Employees.FirstOrDefault(e => e.Id == CuratorId)!
                    };
                    _clientService.Save(newClient);
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
