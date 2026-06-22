using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Vodovoz.Domain.Entities;
using Vodovoz.Domain.Interfaces;
using Vodovoz.UI.Common;

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

        public EmployeesViewModel(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            var list = _employeeService.GetAll();
            Employees = new ObservableCollection<Employee>(list);
        }
    }
}
