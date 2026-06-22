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
    public class OrderServiceTests
    {
        private Mock<IOrderRepository> _orderRepositoryMock = null!;
        private OrderService _orderService = null!;
        private Order _testOrder = null!;

        [SetUp]
        public void SetUp()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _orderService = new OrderService(_orderRepositoryMock.Object);
            _testOrder = CreateTestOrder();
        }

        private static Order CreateTestOrder()
        {
            return new Order
            {
                Date = DateTime.Now,
                Sum = 15000.50m,
                Employee = new Employee
                {
                    Id = 1,
                    Surname = "Сидоров",
                    Name = "Сидор",
                    Patronymic = "Сидорович",
                    Position = Position.Работник,
                    BirthDate = new DateTime(1988, 3, 10)
                },
                Client = new Client
                {
                    Id = 1,
                    Name = "Тестовый клиент",
                    Inn = "1234567890"
                }
            };
        }

        [Test]
        public void Save_ValidOrder_CallsRepositorySave()
        {
            _orderService.Save(_testOrder);
            _orderRepositoryMock.Verify(r => r.Save(_testOrder), Times.Once);
        }

        [Test]
        public void Save_NullOrder_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _orderService.Save(null!));
        }

        [Test]
        public void Save_ZeroSum_ThrowsBusinessRuleException()
        {
            _testOrder.Sum = 0;
            Assert.Throws<BusinessRuleException>(() => _orderService.Save(_testOrder));
        }

        [Test]
        public void Save_NegativeSum_ThrowsBusinessRuleException()
        {
            _testOrder.Sum = -100;
            Assert.Throws<BusinessRuleException>(() => _orderService.Save(_testOrder));
        }

        [Test]
        public void Save_NullEmployee_ThrowsBusinessRuleException()
        {
            _testOrder.Employee = null!;
            Assert.Throws<BusinessRuleException>(() => _orderService.Save(_testOrder));
        }

        [Test]
        public void Save_NullClient_ThrowsBusinessRuleException()
        {
            _testOrder.Client = null!;
            Assert.Throws<BusinessRuleException>(() => _orderService.Save(_testOrder));
        }

        [Test]
        public void Save_InvalidOrder_DoesNotCallRepository()
        {
            _testOrder.Sum = -1;
            Assert.Throws<BusinessRuleException>(() => _orderService.Save(_testOrder));
            _orderRepositoryMock.Verify(r => r.Save(It.IsAny<Order>()), Times.Never);
        }

        [Test]
        public void Delete_ExistingOrder_CallsRepositoryDelete()
        {
            _testOrder.Id = 1;
            _orderRepositoryMock.Setup(r => r.GetById(1)).Returns(_testOrder);
            _orderService.Delete(1);
            _orderRepositoryMock.Verify(r => r.Delete(_testOrder), Times.Once);
        }

        [Test]
        public void Delete_NonExistingOrder_ThrowsInvalidOperationException()
        {
            _orderRepositoryMock.Setup(r => r.GetById(999)).Returns((Order?)null);
            Assert.Throws<InvalidOperationException>(() => _orderService.Delete(999));
        }

        [Test]
        public void GetById_ExistingId_ReturnsOrder()
        {
            _orderRepositoryMock.Setup(r => r.GetById(1)).Returns(_testOrder);

            var result = _orderService.GetById(1);
            Assert.That(result, Is.EqualTo(_testOrder));
        }

        [Test]
        public void GetAll_CallsRepositoryGetAll()
        {
            _orderService.GetAll();
            _orderRepositoryMock.Verify(r => r.GetAll(), Times.Once);
        }
    }
}