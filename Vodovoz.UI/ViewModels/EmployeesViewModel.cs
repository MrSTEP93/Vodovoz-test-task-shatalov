using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Vodovoz.Domain.Entities;
using Vodovoz.Domain.Interfaces;
using Vodovoz.UI.Common;
using Vodovoz.UI.Views;

namespace Vodovoz.UI.ViewModels
{
    public class EmployeesViewModel : ViewModelBase 
    {
        private readonly IEmployeeService _employeeService;

        private ObservableCollection<Employee> _employees = null!;
        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set => SetProperty(ref _employees, value);
        }

        private Employee _selectedEmployee = null!;
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set => SetProperty(ref _selectedEmployee, value);
        }

        public ICommand AddEmployeeCommand { get; private set; }

        public EmployeesViewModel(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
            AddEmployeeCommand = new RelayCommand(OpenAddEmployeeWindow);
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            var list = _employeeService.GetAll();
            Employees = new ObservableCollection<Employee>(list);
        }

        private void OpenAddEmployeeWindow()
        {
            var editVm = new EmployeeEditViewModel(_employeeService);

            var window = new EmployeeEditWindow
            {
                DataContext = editVm,
                Owner = Application.Current.MainWindow
            };

            var result = window.ShowDialog();

            if (result == true)
            {
                LoadEmployees();
            }
        }
    }
}
