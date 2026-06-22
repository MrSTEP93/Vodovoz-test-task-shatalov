using System;
using System.Collections.Generic;
using System.Text;
using Vodovoz.Domain.Entities;
using Vodovoz.Domain.Exceptions;
using Vodovoz.Domain.Interfaces;

namespace Vodovoz.Services
{
    public class ClientService(
        IClientRepository clientRepository, 
        IOrderRepository orderRepository) : IClientService
    {
        private readonly IClientRepository _clientRepository = clientRepository;
        private readonly IOrderRepository _orderRepository = orderRepository;

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
                throw new InvalidOperationException($"Клиент с ID {id} не найден.");

            ValidateBeforeDelete(client);
            _clientRepository.Delete(client);
        }

        /// <summary>
        /// Валидация данных клиента перед сохранением.
        /// </summary>
        private void ValidateBeforeSave(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            if (string.IsNullOrWhiteSpace(client.Name))
                throw new BusinessRuleException("Наименование клиента обязательно для заполнения.");

            if (string.IsNullOrWhiteSpace(client.Inn))
                throw new BusinessRuleException("ИНН клиента обязателен для заполнения.");

            // Проверка длины ИНН (для юр. лиц 10, для ИП/физ. лиц 12)
            if (client.Inn.Length != 10 && client.Inn.Length != 12)
                throw new BusinessRuleException("ИНН должен состоять из 10 или 12 цифр.");

            if (client.Curator == null)
                throw new BusinessRuleException("Необходимо выбрать куратора (сотрудника) для клиента.");
        }

        /// <summary>
        /// Проверка бизнес-правил перед удалением клиента.
        /// Запрещает удаление, если у клиента есть активные заказы.
        /// </summary>
        private void ValidateBeforeDelete(Client client)
        {
            // Проверяем наличие заказов через репозиторий заказов
            // Важно: здесь нужен метод GetByClientId в IOrderRepository
            bool hasOrders = _orderRepository.GetByClientId(client.Id).Any();

            if (hasOrders)
                throw new BusinessRuleException(
                    $"Нельзя удалить клиента \"{client.Name}\", так как у него есть связанные заказы.");
        }
    }
}
