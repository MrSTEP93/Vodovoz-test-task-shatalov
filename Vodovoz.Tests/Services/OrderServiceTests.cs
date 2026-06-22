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
    namespace Vodovoz.Tests.Services
    {
        [TestFixture]
        public class OrderServiceTests
        {
            private Mock<IOrderRepository> _orderRepositoryMock = null!;
            private OrderService _orderService = null!;

            [SetUp]
            public void SetUp()
            {
                _orderRepositoryMock = new Mock<IOrderRepository>();
                _orderService = new OrderService(_orderRepositoryMock.Object);
            }

            private static Order CreateValidOrder()
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
                var order = CreateValidOrder();

                _orderService.Save(order);

                _orderRepositoryMock.Verify(r => r.Save(order), Times.Once);
            }

            [Test]
            public void Save_NullOrder_ThrowsArgumentNullException()
            {
                Assert.Throws<ArgumentNullException>(() => _orderService.Save(null!));
            }

            [Test]
            public void Save_ZeroSum_ThrowsBusinessRuleException()
            {
                var order = CreateValidOrder();
                order.Sum = 0;

                Assert.Throws<BusinessRuleException>(() => _orderService.Save(order));
            }

            [Test]
            public void Save_NegativeSum_ThrowsBusinessRuleException()
            {
                var order = CreateValidOrder();
                order.Sum = -100;

                Assert.Throws<BusinessRuleException>(() => _orderService.Save(order));
            }

            [Test]
            public void Save_NullEmployee_ThrowsBusinessRuleException()
            {
                var order = CreateValidOrder();
                order.Employee = null!;

                Assert.Throws<BusinessRuleException>(() => _orderService.Save(order));
            }

            [Test]
            public void Save_NullClient_ThrowsBusinessRuleException()
            {
                var order = CreateValidOrder();
                order.Client = null!;

                Assert.Throws<BusinessRuleException>(() => _orderService.Save(order));
            }

            [Test]
            public void Save_InvalidOrder_DoesNotCallRepository()
            {
                var order = CreateValidOrder();
                order.Sum = -1;

                Assert.Throws<BusinessRuleException>(() => _orderService.Save(order));

                _orderRepositoryMock.Verify(r => r.Save(It.IsAny<Order>()), Times.Never);
            }

            [Test]
            public void Delete_ExistingOrder_CallsRepositoryDelete()
            {
                var order = CreateValidOrder();
                order.Id = 1;
                _orderRepositoryMock.Setup(r => r.GetById(1)).Returns(order);

                _orderService.Delete(1);

                _orderRepositoryMock.Verify(r => r.Delete(order), Times.Once);
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
                var expected = CreateValidOrder();
                _orderRepositoryMock.Setup(r => r.GetById(1)).Returns(expected);

                var result = _orderService.GetById(1);

                Assert.That(result, Is.EqualTo(expected));
            }

            [Test]
            public void GetAll_CallsRepositoryGetAll()
            {
                _orderService.GetAll();

                _orderRepositoryMock.Verify(r => r.GetAll(), Times.Once);
            }
        }
    }
}
