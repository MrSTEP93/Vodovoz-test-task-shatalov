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
    public class ClientsViewModel : ViewModelBase
    {
        private readonly IClientService _clientService;
        private readonly IEmployeeService _employeeService;
        private ObservableCollection<Client> _clients = null!;
        public ObservableCollection<Client> Clients
        {
            get => _clients;
            set => SetProperty(ref _clients, value);
        }

        private Client _selectedClient = null!;
        public Client SelectedClient
        {
            get => _selectedClient;
            set => SetProperty(ref _selectedClient, value);
        }

        public ICommand AddClientCommand { get; private set; }
        public ICommand EditClientCommand { get; private set; }
        public ICommand DeleteClientCommand { get; private set; }

        public ClientsViewModel(IClientService clientService, IEmployeeService employeeService)
        {
            _clientService = clientService;
            _employeeService = employeeService;
            AddClientCommand = new RelayCommand(OpenAddClientWindow);
            DeleteClientCommand = new RelayCommand(DeleteClient, () => SelectedClient != null);
            EditClientCommand = new RelayCommand(OpenEditClientWindow, () => SelectedClient != null);
            LoadClients();
        }

        private void LoadClients()
        {
            var list = _clientService.GetAll();
            Clients = new ObservableCollection<Client>(list);
        }

        private void OpenAddClientWindow()
        {
            var editVm = new ClientEditViewModel(_clientService, _employeeService);

            var window = new ClientEditWindow
            {
                DataContext = editVm,
                Owner = Application.Current.MainWindow
            };

            var result = window.ShowDialog();

            if (result == true)
            {
                LoadClients();
            }
        }

        private void OpenEditClientWindow()
        {
            var editVm = new ClientEditViewModel(_clientService, _employeeService, SelectedClient);

            var window = new ClientEditWindow
            {
                DataContext = editVm,
                Owner = Application.Current.MainWindow
            };

            var result = window.ShowDialog();

            if (result == true)
            {
                LoadClients();
            }
        }

        private void DeleteClient()
        {
            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить контрагента \"{SelectedClient.Name}\"?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                _clientService.Delete(SelectedClient.Id);
                LoadClients();
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
