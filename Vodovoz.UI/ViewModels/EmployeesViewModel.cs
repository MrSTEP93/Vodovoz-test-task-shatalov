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
        public ICommand EditEmployeeCommand { get; private set; }
        public ICommand DeleteEmployeeCommand { get; private set; }

        public EmployeesViewModel(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
            AddEmployeeCommand = new RelayCommand(OpenAddEmployeeWindow);
            EditEmployeeCommand = new RelayCommand(OpenEditEmployeeWindow, () => SelectedEmployee != null);
            DeleteEmployeeCommand = new RelayCommand(DeleteEmployee, () => SelectedEmployee != null);
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

        private void OpenEditEmployeeWindow()
        {
            var editVm = new EmployeeEditViewModel(_employeeService, SelectedEmployee);

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

        private void DeleteEmployee()
        {
            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить сотрудника \"{SelectedEmployee.Name} {SelectedEmployee.Surname}\"?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                _employeeService.Delete(SelectedEmployee.Id);
                LoadEmployees();
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
