using System;
using System.Collections.Generic;
using System.Text;
using Vodovoz.Domain.Entities;
using Vodovoz.Domain.Exceptions;
using Vodovoz.Domain.Interfaces;

namespace Vodovoz.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IOrderRepository _orderRepository;

        public ClientService(IClientRepository clientRepository, IOrderRepository orderRepository)
        {
            _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public Client? GetById(int id)
        {
            return _clientRepository.GetById(id);
        }

        public IEnumerable<Client> GetAll()
        {
            return _clientRepository.GetAll();
        }

        public void Save(Client client)
        {
            ValidateBeforeSave(client);
            _clientRepository.Save(client);
        }

        public void Delete(int id)
        {
            var client = _clientRepository.GetById(id);

            if (client == null)
                throw new InvalidOperationException($"Контрагент с ID {id} не найден.");

            ValidateBeforeDelete(client);
            _clientRepository.Delete(client);
        }

        /// <summary>
        /// Валидация данных контрагента перед сохранением клиента.
        /// </summary>
        private void ValidateBeforeSave(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            if (string.IsNullOrWhiteSpace(client.Name))
                throw new BusinessRuleException("Наименование клиента обязательно для заполнения.");

            if (string.IsNullOrWhiteSpace(client.Inn))
                throw new BusinessRuleException("ИНН контрагента обязателен для заполнения");

            // Проверка длины ИНН (для ЮЛ 10 цифр, для ИП/ФЛ 12)
            if (client.Inn.Length != 10 && client.Inn.Length != 12)
                throw new BusinessRuleException("ИНН должен состоять из 10 или 12 цифр.");

            if (client.Curator == null)
                throw new BusinessRuleException("Необходимо выбрать куратора (сотрудника) для контрагента");
        }

        /// <summary>
        /// Проверка бизнес-правил перед удалением клиента.
        /// Запрещает удаление, если у клиента есть заказы.
        /// </summary>
        private void ValidateBeforeDelete(Client client)
        {
            bool hasOrders = _orderRepository.GetByClientId(client.Id).Any();

            if (hasOrders)
                throw new BusinessRuleException(
                    $"Нельзя удалить клиента \"{client.Name}\", т.к. у него есть связанные заказы.");
        }
    }
}
