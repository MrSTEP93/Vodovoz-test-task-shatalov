using System;
using System.Collections.Generic;
using System.Text;
using Vodovoz.Domain.Enums;

namespace Vodovoz.Domain.Entities
{
    /// <summary>
    /// Модель сотрудника организации
    /// </summary>
    public class Employee
    {
        public virtual int Id { get; set; }

        public virtual string Surname { get; set; } = string.Empty;

        public virtual string Name { get; set; } = string.Empty;

        public virtual string Patronymic { get; set; } = string.Empty;

        public virtual Position Position { get; set; }

        public virtual DateTime BirthDate { get; set; }
    }
}
