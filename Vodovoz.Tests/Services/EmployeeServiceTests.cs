using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Vodovoz.Domain.Entities;
using Vodovoz.Domain.Enums;
using Vodovoz.Domain.Exceptions;
using Vodovoz.Domain.Interfaces;
using Vodovoz.Services;

namespace Vodovoz.Tests.Services
{
    [TestFixture]
    public class EmployeeServiceTests
    {
        private Mock<IEmployeeRepository> _employeeRepositoryMock = null!;
        private Mock<IClientRepository> _clientRepositoryMock = null!;
        private EmployeeService _employeeService = null!;
        private Employee _testEmployee = null!;

        [SetUp]
        public void SetUp()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _clientRepositoryMock = new Mock<IClientRepository>();

            _employeeService = new EmployeeService(
                _employeeRepositoryMock.Object,
                _clientRepositoryMock.Object);

            _testEmployee = CreateValidEmployee();
        }

        private static Employee CreateValidEmployee()
        {
            return new Employee
            {
                Surname = "Сидоров",
                Name = "Сидор",
                Patronymic = "Сидорович",
                Position = Position.Работник,
                BirthDate = new DateTime(1990, 8, 20)
            };
        }

        [Test]
        public void Save_ValidEmployee_CallsRepositorySave()
        {
            _employeeService.Save(_testEmployee);
            _employeeRepositoryMock.Verify(r => r.Save(_testEmployee), Times.Once);
        }

        [Test]
        public void Save_NullEmployee_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _employeeService.Save(null!));
        }

        [Test]
        public void Save_EmptySurname_ThrowsBusinessRuleException()
        {
            _testEmployee.Surname = string.Empty;
            Assert.Throws<BusinessRuleException>(() => _employeeService.Save(_testEmployee));
        }

        [Test]
        public void Save_EmptyName_ThrowsBusinessRuleException()
        {
            _testEmployee.Name = string.Empty;
            Assert.Throws<BusinessRuleException>(() => _employeeService.Save(_testEmployee));
        }

        [Test]
        public void Save_FutureBirthDate_ThrowsBusinessRuleException()
        {
            _testEmployee.BirthDate = DateTime.Now.AddDays(1);
            Assert.Throws<BusinessRuleException>(() => _employeeService.Save(_testEmployee));
        }

        [Test]
        public void Save_TooYoungEmployee_ThrowsBusinessRuleException()
        {
            _testEmployee.BirthDate = DateTime.Today.AddYears(-17); // 17 лет - меньше 18
            Assert.Throws<BusinessRuleException>(() => _employeeService.Save(_testEmployee));
        }

        [Test]
        public void Save_TooOldEmployee_ThrowsBusinessRuleException()
        {
            _testEmployee.BirthDate = new DateTime(1899, 12, 31); // Раньше 1900
            Assert.Throws<BusinessRuleException>(() => _employeeService.Save(_testEmployee));
        }

        [Test]
        public void Save_InvalidEmployee_DoesNotCallRepository()
        {
            _testEmployee.Surname = " ";
            Assert.Throws<BusinessRuleException>(() => _employeeService.Save(_testEmployee));
            _employeeRepositoryMock.Verify(r => r.Save(It.IsAny<Employee>()), Times.Never);
        }

        [Test]
        public void Delete_EmployeeWithClients_ThrowsBusinessRuleException()
        {
            _testEmployee.Id = 1;

            _employeeRepositoryMock.Setup(r => r.GetById(1)).Returns(_testEmployee);
            _clientRepositoryMock.Setup(r => r.GetByCuratorId(1))
                .Returns(new List<Client> { new Client() });

            Assert.Throws<BusinessRuleException>(() => _employeeService.Delete(1));
        }

        [Test]
        public void Delete_EmployeeWithoutClients_CallsRepositoryDelete()
        {
            _testEmployee.Id = 1;
            _clientRepositoryMock.Setup(r => r.GetByCuratorId(1))
                .Returns(Enumerable.Empty<Client>());
            _employeeRepositoryMock.Setup(r => r.GetById(1)).Returns(_testEmployee);

            _employeeService.Delete(1);
            _employeeRepositoryMock.Verify(r => r.Delete(_testEmployee), Times.Once);
        }

        [Test]
        public void Delete_NonExistingEmployee_ThrowsInvalidOperationException()
        {
            _employeeRepositoryMock.Setup(r => r.GetById(999)).Returns((Employee?)null);
            Assert.Throws<InvalidOperationException>(() => _employeeService.Delete(999));
        }

        [Test]
        public void GetById_ExistingId_ReturnsEmployee()
        {
            _employeeRepositoryMock.Setup(r => r.GetById(1)).Returns(_testEmployee);
            var result = _employeeService.GetById(1);
            Assert.That(result, Is.EqualTo(_testEmployee));
        }

        [Test]
        public void GetAll_CallsRepositoryGetAll()
        {
            _employeeService.GetAll();
            _employeeRepositoryMock.Verify(r => r.GetAll(), Times.Once);
        }
    }
}