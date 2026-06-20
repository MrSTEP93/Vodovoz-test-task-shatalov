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
    public class ClientRepositoryTests
    {
        private IBaseRepository<Client> _clientRepository = null!;
        private IBaseRepository<Employee> _employeeRepository = null!;

        private Employee _testEmployee = null!;

        [SetUp]
        public void SetUp()
        {
            TestDatabaseHelper.ClearAllTables();
            var provider = TestDatabaseHelper.GetProvider();
            _clientRepository = new BaseRepository<Client>(provider);
            _employeeRepository = new BaseRepository<Employee>(provider);

            _testEmployee = CreateTestEmployee();
            _employeeRepository.Save(_testEmployee);
        }

        private static Employee CreateTestEmployee()
        {
            return new Employee
            {
                Surname = "Михайлов",
                Name = "Михаил",
                Patronymic = "Михаилович",
                Position = Position.Работник,
                BirthDate = new DateTime(1938, 10, 3)
            };
        }

        [Test]
        public void Save_NewClient_AssignsId()
        {
            var client = new Client
            {
                Name = "ООО Ромашка",
                Inn = "1234567890",
                Curator = _testEmployee
            };
            _clientRepository.Save(client);

            Assert.That(client.Id, Is.GreaterThan(0));
        }

        [Test]
        public void GetById_AfterSave_ReturnsClientWithCurator()
        {
            var client = new Client
            {
                Name = "ИП Петров",
                Inn = "987654321012",
                Curator = _testEmployee
            };
            _clientRepository.Save(client);

            var loaded = _clientRepository.GetById(client.Id);

            Assert.That(loaded, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(loaded!.Name, Is.EqualTo(client.Name));
                Assert.That(loaded.Inn, Is.EqualTo(client.Inn));
                Assert.That(loaded.Curator, Is.Not.Null);
                Assert.That(loaded.Curator.Surname, Is.EqualTo(_testEmployee.Surname));
            }
        }

        [Test]
        public void GetAll_AfterMultipleSaves_ReturnsAll()
        {
            _clientRepository.Save(new Client { Name = "Клиент 1", Inn = "1111111111", Curator = _testEmployee });
            _clientRepository.Save(new Client { Name = "Клиент 2", Inn = "2222222222", Curator = _testEmployee });
            _clientRepository.Save(new Client { Name = "Клиент 3", Inn = "3333333333", Curator = _testEmployee });

            var all = _clientRepository.GetAll().ToList();

            Assert.That(all, Has.Count.EqualTo(3));
        }

        [Test]
        public void Delete_ExistingClient_RemovesFromDb()
        {
            var client = new Client
            {
                Name = "Временный клиент",
                Inn = "5555555555",
                Curator = _testEmployee
            };
            _clientRepository.Save(client);
            var id = client.Id;

            _clientRepository.Delete(client);
            var loaded = _clientRepository.GetById(id);

            Assert.That(loaded, Is.Null);
        }
    }
}
