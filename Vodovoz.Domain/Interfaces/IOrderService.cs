using System;
using System.Collections.Generic;
using System.Text;
using Vodovoz.Domain.Entities;

namespace Vodovoz.Domain.Interfaces
{
    public interface IOrderService
    {
        Order? GetById(int id);
        IEnumerable<Order> GetAll();
        void Save(Order order);
        void Delete(int id);
    }
}
