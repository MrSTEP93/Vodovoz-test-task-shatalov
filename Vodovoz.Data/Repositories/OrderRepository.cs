using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Text;
using Vodovoz.Data.Services;
using Vodovoz.Domain.Entities;
using Vodovoz.Domain.Interfaces;

namespace Vodovoz.Data.Repositories
{
    /// <summary>
    /// Репозиторий для работы с заказами
    /// </summary>
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ISessionFactoryProvider sessionProvider) : base(sessionProvider) { }

        public override IEnumerable<Order> GetAll()
        {
            using var session = SessionProvider.OpenSession();
            Order alias = null!;
            return session.QueryOver<Order>(() => alias)
                .JoinAlias(() => alias.Employee, () => alias!.Employee)
                .JoinAlias(() => alias.Client, () => alias!.Client)
                .List<Order>();
        }
    }
}
