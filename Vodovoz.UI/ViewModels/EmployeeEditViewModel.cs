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
using Vodovoz.UI.Common;

namespace Vodovoz.UI.ViewModels
{
    public class EmployeeEditViewModel : ViewModelBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly Employee _employee = null!;
        private bool _isEditMode;
        
        public string Surname { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public Position Position { get; set; }
        public DateTime BirthDate { get; set; } = DateTime.Today;

        public string Title => _isEditMode ? "Редактирование сотрудника" : "Добавление сотрудника";

        public ICommand SaveCommand { get; private set; } = null!;
        public ICommand CancelCommand { get; private set; } = null!;

        public EmployeeEditViewModel(IEmployeeService employeeService, Employee employee)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _employee = employee ?? throw new ArgumentNullException(nameof(employee));
            _isEditMode = true;

            Surname = employee.Surname;
            Name = employee.Name;
            Patronymic = employee.Patronymic;
            Position = employee.Position;
            BirthDate = employee.BirthDate;

            InitializeCommands();
        }

        public EmployeeEditViewModel(IEmployeeService employeeService)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _isEditMode = false;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            SaveCommand = new RelayCommand(Save, () =>
                !string.IsNullOrWhiteSpace(Surname) && !string.IsNullOrWhiteSpace(Name));

            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            try
            {
                if (_isEditMode)
                {
                    _employee.Surname = Surname;
                    _employee.Name = Name;
                    _employee.Patronymic = Patronymic;
                    _employee.Position = Position;
                    _employee.BirthDate = BirthDate;

                    _employeeService.Save(_employee);
                }
                else
                {
                    var newEmployee = new Employee
                    {
                        Surname = Surname,
                        Name = Name,
                        Patronymic = Patronymic,
                        Position = Position,
                        BirthDate = BirthDate
                    };

                    _employeeService.Save(newEmployee);
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
