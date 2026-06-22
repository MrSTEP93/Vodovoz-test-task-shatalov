using System;
using System.Collections.Generic;
using System.Text;
using Vodovoz.Domain.Entities;

namespace Vodovoz.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с заказами.
    /// Наследует общие CRUD-операции от IBaseRepository
    /// </summary>
    public interface IOrderRepository : IBaseRepository<Order>
    {
        IEnumerable<Order> GetByClientId(int id);
    }

}
