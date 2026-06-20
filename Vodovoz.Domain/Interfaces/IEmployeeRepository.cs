using System;
using System.Collections.Generic;
using System.Text;
using Vodovoz.Domain.Entities;

namespace Vodovoz.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с сотрудниками.
    /// Наследует общие CRUD-операции от IBaseRepository
    /// </summary>
    public interface IEmployeeRepository : IBaseRepository<Employee> { }

}
