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
    /// Репозиторий для работы с контрагентами
    /// </summary>
    public class CounterpartyRepository : BaseRepository<Client>, IClientRepository
    {
        public CounterpartyRepository(ISessionFactoryProvider sessionProvider) : base(sessionProvider) { }

        public override IEnumerable<Client> GetAll()
        {
            using var session = SessionProvider.OpenSession();
            Client alias = null!;
            return session.QueryOver<Client>(() => alias)
                .JoinAlias(() => alias.Curator, () => alias!.Curator)
                .List<Client>();
        }

        public IEnumerable<Client> GetByCuratorId(int curatorId)
        {
            using var session = SessionProvider.OpenSession();
            Client alias = null!;
            return session.QueryOver<Client>(() => alias)
                .JoinAlias(() => alias.Curator, () => alias!.Curator)
                .Where(() => alias.Curator.Id == curatorId)
                .List<Client>();
        }
    }
}
