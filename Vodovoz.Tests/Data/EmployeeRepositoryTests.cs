using Vodovoz.Data.Infrastructure;
using Vodovoz.Data.Repositories;
using Vodovoz.Domain.Entities;
using Vodovoz.Domain.Enums;
using Vodovoz.Domain.Interfaces;
using Vodovoz.Tests.Helpers;

namespace Vodovoz.Tests.Data
{
    [TestFixture]
    public class EmployeeRepositoryTests
    {
        private IBaseRepository<Employee> _repository = null!;

        [SetUp]
        public void SetUp()
        {
            TestDatabaseHelper.ClearAllTables();
            var provider = TestDatabaseHelper.GetProvider();
            _repository = new BaseRepository<Employee>(provider);
        }

        [Test]
        public void Save_NewEmployee_AssignsId()
        {
            var employee = new Employee
            {
                Surname = "Иванов",
                Name = "Иван",
                Patronymic = "Иванович",
                Position = Position.Руководитель,
                BirthDate = new DateTime(1980, 1, 1)
            };
            _repository.Save(employee);

            Assert.That(employee.Id, Is.GreaterThan(0));
        }

        [Test]
        public void GetById_AfterSave_ReturnsEmployee()
        {
            var employee = new Employee
            {
                Surname = "Петров",
                Name = "Пётр",
                Patronymic = "Петрович",
                Position = Position.Работник,
                BirthDate = new DateTime(1995, 5, 15)
            };
            _repository.Save(employee);

            var loaded = _repository.GetById(employee.Id);

            Assert.That(loaded, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(loaded!.Surname, Is.EqualTo(employee.Surname));
                Assert.That(loaded!.Name, Is.EqualTo(employee.Name));
                Assert.That(loaded!.Patronymic, Is.EqualTo(employee.Patronymic));
                Assert.That(loaded.Position, Is.EqualTo(employee.Position));
            }
        }

        [Test]
        public void Delete_ExistingEmployee_RemovesFromDb()
        {
            var employee = new Employee
            {
                Surname = "Сидоров",
                Name = "Сидор",
                Patronymic = "Сидорович",
                Position = Position.Работник,
                BirthDate = new DateTime(1990, 8, 20)
            };
            _repository.Save(employee);
            var id = employee.Id;

            _repository.Delete(employee);
            var loaded = _repository.GetById(id);

            Assert.That(loaded, Is.Null);
        }
    }
}
