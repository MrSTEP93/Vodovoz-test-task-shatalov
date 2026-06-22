using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Vodovoz.UI.Common;

namespace Vodovoz.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private object _currentViewModel;
        public object CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public ICommand ShowEmployeesCommand { get; }
        public ICommand ShowClientsCommand { get; }
        public ICommand ShowOrdersCommand { get; }

        public MainViewModel(
            EmployeesViewModel employeesVm,
            ClientsViewModel counterpartiesVm,
            OrdersViewModel ordersVm)
        {
            ShowEmployeesCommand = new RelayCommand(() => CurrentViewModel = employeesVm);
            ShowClientsCommand = new RelayCommand(() => CurrentViewModel = counterpartiesVm);
            ShowOrdersCommand = new RelayCommand(() => CurrentViewModel = ordersVm);

            _currentViewModel = employeesVm;
        }
    }
}
