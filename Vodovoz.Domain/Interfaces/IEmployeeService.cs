using System;
using System.Collections.Generic;
using System.Text;
using Vodovoz.Domain.Entities;

namespace Vodovoz.Domain.Interfaces
{
    public interface IEmployeeService
    {
        Employee? GetById(int id);
        IEnumerable<Employee> GetAll();
        void Save(Employee employee);
        void Delete(int id);
    }
}
