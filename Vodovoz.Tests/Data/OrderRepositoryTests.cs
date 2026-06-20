using System;
using System.Collections.Generic;
using System.Text;
using Vodovoz.Data.Repositories;
using Vodovoz.Domain.Entities;
using Vodovoz.Domain.Enums;
using Vodovoz.Domain.Interfaces;
using Vodovoz.Tests.Helpers;

namespace Vodovoz.Tests.Data
{
    [TestFixture]
    public class OrderRepositoryTests
    {
        private IBaseRepository<Order> _orderRepository = null!;
        private IBaseRepository<Client> _clientRepository = null!;
        private IBaseRepository<Employee> _employeeRepository = null!;

        private Employee _testEmployee = null!;
        private Client _testClient = null!;
        private Order _testOrder = null!;

        [SetUp]
        public void SetUp()
        {
            TestDatabaseHelper.ClearAllTables();
            var provider = TestDatabaseHelper.GetProvider();
            _orderRepository = new BaseRepository<Order>(provider);
            _clientRepository = new BaseRepository<Client>(provider);
            _employeeRepository = new BaseRepository<Employee>(provider);

            _testEmployee = CreateTestEmployee();
            _employeeRepository.Save(_testEmployee);

            _testClient = CreateTestClient(_testEmployee);
            _clientRepository.Save(_testClient);

            _testOrder = CreateTestOrder(_testEmployee, _testClient);
            _orderRepository.Save(_testOrder);
        }

        private static Employee CreateTestEmployee()
        {
            return new Employee
            {
                Surname = "Сидоров",
                Name = "Сидор",
                Patronymic = "Сидорович",
                Position = Position.Работник,
                BirthDate = new DateTime(1988, 3, 10)
            };
        }

        private static Client CreateTestClient(Employee curator)
        {
            return new Client
            {
                Name = "Тестовый клиент",
                Inn = "1234567890",
                Curator = curator
            };
        }

        private static Order CreateTestOrder(Employee employee, Client client)
        {
            return new Order
            {
                Date = DateTime.Now,
                Sum = 15000.50m,
                Employee = employee,
                Client = client
            };
        }

        [Test]
        public void Save_NewOrder_AssignsId()
        {
            Assert.That(_testOrder.Id, Is.GreaterThan(0));
        }

        [Test]
        public void GetById_AfterSave_ReturnsOrderWithClient()
        {
            var loaded = _orderRepository.GetById(_testOrder.Id);

            Assert.That(loaded, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(loaded!.Client, Is.Not.Null);
                Assert.That(loaded.Client.Name, Is.EqualTo(_testClient.Name));
                Assert.That(loaded.Sum, Is.EqualTo(_testOrder.Sum));
            }
        }

        [Test]
        public void Save_OrderWithEmployee_SavesSuccessfully()
        {
            var loaded = _orderRepository.GetById(_testOrder.Id);
            Assert.That(loaded, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(loaded!.Employee, Is.Not.Null);
                Assert.That(loaded.Employee.Surname, Is.EqualTo(_testEmployee.Surname));
                Assert.That(loaded.Employee.Name, Is.EqualTo(_testEmployee.Name));
                Assert.That(loaded.Employee.Patronymic, Is.EqualTo(_testEmployee.Patronymic));
            }
        }

        [Test]
        public void Delete_ExistingOrder_RemovesFromDb()
        {
            var id = _testOrder.Id;

            _orderRepository.Delete(_testOrder);
            var loaded = _orderRepository.GetById(id);

            Assert.That(loaded, Is.Null);
        }
    }
}
