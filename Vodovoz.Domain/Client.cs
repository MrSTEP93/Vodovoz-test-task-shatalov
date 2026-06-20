using System;
using System.Collections.Generic;
using System.Text;

namespace Vodovoz.Domain
{
    /// <summary>
    /// Модель данных, представляющая клиента (контрагента).
    /// Каждый контрагент привязан к одному сотруднику - куратору.
    /// </summary>
    public class Counterparty
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; } = string.Empty;

        public virtual string Inn { get; set; } = string.Empty;

        public virtual Employee Curator { get; set; } = null!;
    }
}
