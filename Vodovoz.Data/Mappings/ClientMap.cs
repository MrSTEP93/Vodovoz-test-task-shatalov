using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Text;
using Vodovoz.Domain.Entities;

namespace Vodovoz.Data.Mappings
{
    /// <summary>
    /// Маппинг NHibernate для модели Client
    /// </summary>
    public class ClientMap : ClassMap<Client>
    {
        public ClientMap() 
        {
            Table("Clients");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name).Length(255).Not.Nullable();
            Map(x => x.Inn).Length(12).Not.Nullable();
            References(x => x.Curator).Column("CuratorId").Not.Nullable();
        }
    }
}
