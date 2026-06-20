using System;
using System.Collections.Generic;
using System.Text;

namespace Vodovoz.Domain.Entities
{
    /// <summary>
    /// Модель данных, представляющая заказ.
    /// Связывает сотрудника, контрагента, дату и сумму заказа.
    /// </summary>
    public class Order
    {
        public virtual int Id { get; set; }

        public virtual DateTime Date { get; set; }

        public virtual decimal Sum { get; set; }

        public virtual Employee Employee { get; set; } = null!;

        public virtual Client Client { get; set; } = null!;
    }
}
