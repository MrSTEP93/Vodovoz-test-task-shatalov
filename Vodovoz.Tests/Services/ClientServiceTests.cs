using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Vodovoz.Domain.Entities;
using Vodovoz.Domain.Enums;
using Vodovoz.Domain.Exceptions;
using Vodovoz.Domain.Interfaces;
using Vodovoz.Services;

namespace Vodovoz.Tests.Services
{
    [TestFixture]
    public class ClientServiceTests
    {
        private Mock<IClientRepository> _clientRepositoryMock = null!;
        private Mock<IOrderRepository> _orderRepositoryMock = null!;
        private ClientService _clientService = null!;
        private Client _testClient = null!;

        [SetUp]
        public void SetUp()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            _orderRepositoryMock = new Mock<IOrderRepository>();

            _clientService = new ClientService(
                _clientRepositoryMock.Object,
                _orderRepositoryMock.Object);

            _testClient = CreateValidClient();
        }

        private static Client CreateValidClient()
        {
            return new Client
            {
                Name = "ООО Тестовая Компания",
                Inn = "7701234567",
                Curator = new Employee
                {
                    Id = 1,
                    Surname = "Иванов",
                    Name = "Иван",
                    Patronymic = "Иванович",
                    Position = Position.Работник,
                    BirthDate = new DateTime(1990, 1, 1)
                }
            };
        }

        [Test]
        public void Save_ValidClient_CallsRepositorySave()
        {
            _clientService.Save(_testClient);
            _clientRepositoryMock.Verify(r => r.Save(_testClient), Times.Once);
        }

        [Test]
        public void Save_NullClient_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _clientService.Save(null!));
        }

        [Test]
        public void Save_EmptyName_ThrowsBusinessRuleException()
        {
            _testClient.Name = string.Empty;
            Assert.Throws<BusinessRuleException>(() => _clientService.Save(_testClient));
        }

        [Test]
        public void Save_NullInn_ThrowsBusinessRuleException()
        {
            _testClient.Inn = null!;
            Assert.Throws<BusinessRuleException>(() => _clientService.Save(_testClient));
        }

        [Test]
        public void Save_InvalidInnLength_ThrowsBusinessRuleException()
        {
            _testClient.Inn = "123";
            Assert.Throws<BusinessRuleException>(() => _clientService.Save(_testClient));
        }

        [Test]
        public void Save_NullCurator_ThrowsBusinessRuleException()
        {
            _testClient.Curator = null!;
            Assert.Throws<BusinessRuleException>(() => _clientService.Save(_testClient));
        }

        [Test]
        public void Save_InvalidClient_DoesNotCallRepository()
        {
            _testClient.Name = "";
            Assert.Throws<BusinessRuleException>(() => _clientService.Save(_testClient));
            _clientRepositoryMock.Verify(r => r.Save(It.IsAny<Client>()), Times.Never);
        }

        [Test]
        public void Delete_ClientWithOrders_ThrowsBusinessRuleException()
        {
            _testClient.Id = 1;

            _clientRepositoryMock.Setup(r => r.GetById(1)).Returns(_testClient);
            _orderRepositoryMock.Setup(r => r.GetByClientId(1))
                .Returns(new List<Order> { new Order() });

            Assert.Throws<BusinessRuleException>(() => _clientService.Delete(1));
        }

        [Test]
        public void Delete_ClientWithoutOrders_CallsRepositoryDelete()
        {
            _testClient.Id = 1;
            _orderRepositoryMock.Setup(r => r.GetByClientId(1))
                .Returns(Enumerable.Empty<Order>());
            _clientRepositoryMock.Setup(r => r.GetById(1)).Returns(_testClient);

            _clientService.Delete(1);
            _clientRepositoryMock.Verify(r => r.Delete(_testClient), Times.Once);
        }

        [Test]
        public void Delete_NonExistingClient_ThrowsInvalidOperationException()
        {
            _clientRepositoryMock.Setup(r => r.GetById(999)).Returns((Client?)null);
            Assert.Throws<InvalidOperationException>(() => _clientService.Delete(999));
        }

        [Test]
        public void GetById_ExistingId_ReturnsClient()
        {
            _clientRepositoryMock.Setup(r => r.GetById(1)).Returns(_testClient);
            var result = _clientService.GetById(1);
            Assert.That(result, Is.EqualTo(_testClient));
        }

        [Test]
        public void GetAll_CallsRepositoryGetAll()
        {
            _clientService.GetAll();
            _clientRepositoryMock.Verify(r => r.GetAll(), Times.Once);
        }
    }
}