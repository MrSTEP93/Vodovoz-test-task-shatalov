using System;
using System.Collections.Generic;
using System.Text;
using Vodovoz.Domain.Entities;
using Vodovoz.Domain.Exceptions;
using Vodovoz.Domain.Interfaces;

namespace Vodovoz.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IClientRepository _clientRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IClientRepository clientRepository)
        {
            _employeeRepository = employeeRepository 
                ?? throw new ArgumentNullException(nameof(employeeRepository));
            _clientRepository = clientRepository 
                ?? throw new ArgumentNullException(nameof(clientRepository));
        }

        public Employee? GetById(int id)
        {
            return _employeeRepository.GetById(id);
        }

        public IEnumerable<Employee> GetAll()
        {
            return _employeeRepository.GetAll();
        }

        public void Save(Employee employee)
        {
            ValidateBeforeSave(employee);
            _employeeRepository.Save(employee);
        }

        public void Delete(int id)
        {
            var employee = _employeeRepository.GetById(id);

            if (employee == null)
                throw new InvalidOperationException($"Сотрудник с ID {id} не найден.");

            ValidateBeforeDelete(employee);
            _employeeRepository.Delete(employee);
        }

        private void ValidateBeforeSave(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            if (string.IsNullOrWhiteSpace(employee.Surname))
                throw new BusinessRuleException("Фамилия сотрудника обязательна для заполнения.");

            if (string.IsNullOrWhiteSpace(employee.Name))
                throw new BusinessRuleException("Имя сотрудника обязательно для заполнения.");

            if (employee.BirthDate > DateTime.Now)
                throw new BusinessRuleException("Дата рождения не может быть в будущем.");

            if (employee.BirthDate.AddYears(18) > DateTime.Today)
                throw new BusinessRuleException("Сотруднику должно быть больше 18 лет.");

            if (employee.BirthDate < new DateTime(1900, 1, 1))
                throw new BusinessRuleException("Некорректная дата рождения.");
        }

        private void ValidateBeforeDelete(Employee employee)
        {
            // Проверяем, является ли сотрудник куратором для каких-либо контрагентов.
            // Если да — запрещаем удаление
            bool isCurator = _clientRepository.GetByCuratorId(employee.Id).Any();

            if (isCurator)
                throw new BusinessRuleException(
                    $"Нельзя удалить сотрудника \"{employee.Surname} {employee.Name}\", так как он является куратором для контрагентов.");
        }
    }
}
