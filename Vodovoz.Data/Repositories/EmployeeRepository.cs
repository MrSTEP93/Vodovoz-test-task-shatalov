using System;
using System.Collections.Generic;
using System.Text;
using Vodovoz.Data.Services;
using Vodovoz.Domain.Entities;
using Vodovoz.Domain.Interfaces;

namespace Vodovoz.Data.Repositories
{
    /// <summary>
    /// Репозиторий для работы с сотрудниками
    /// </summary>
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ISessionFactoryProvider sessionProvider) : base(sessionProvider) { }

        public override IEnumerable<Employee> GetAll()
        {
            using var session = SessionProvider.OpenSession();
            return session.QueryOver<Employee>().List<Employee>();
        }
    }
}
