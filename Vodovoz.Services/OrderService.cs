using System;
using System.Collections.Generic;
using System.Text;
using Vodovoz.Domain.Entities;
using Vodovoz.Domain.Exceptions;
using Vodovoz.Domain.Interfaces;

namespace Vodovoz.Services
{
    public class OrderService(IOrderRepository orderRepository) : IOrderService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;

        public Order? GetById(int id)
        {
            return _orderRepository.GetById(id);
        }

        public IEnumerable<Order> GetAll()
        {
            return _orderRepository.GetAll();
        }

        public void Save(Order order)
        {
            ValidateBeforeSave(order);
            _orderRepository.Save(order);
        }

        public void Delete(int id)
        {
            var order = _orderRepository.GetById(id) 
                ?? throw new InvalidOperationException($"Заказ с ID {id} не найден.");
            _orderRepository.Delete(order);
        }

        /// <summary>
        /// Валидация данных (проверка бизнес-правил) при сохранении заказа.
        /// </summary>
        private static void ValidateBeforeSave(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (order.Sum <= 0)
                throw new BusinessRuleException("Сумма заказа должна быть больше нуля.");

            if (order.Date == default)
                throw new BusinessRuleException("Дата заказа не может быть пустой.");

            if (order.Employee == null)
                throw new BusinessRuleException("Необходимо выбрать сотрудника для оформления заказа.");

            if (order.Client == null)
                throw new BusinessRuleException("Необходимо выбрать контрагента для оформления заказа.");
        }
    }
}