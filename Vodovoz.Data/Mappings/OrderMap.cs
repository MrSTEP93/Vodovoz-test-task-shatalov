using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Text;
using Vodovoz.Domain.Entities;

namespace Vodovoz.Data.Mappings
{
    /// <summary>
    /// Маппинг NHibernate для модели Order
    /// </summary>
    public class OrderMap : ClassMap<Order>
    {
        public OrderMap()
        {
            Table("Orders");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Date).Not.Nullable();
            Map(x => x.Sum).Precision(18).Scale(2).Not.Nullable();
            References(x => x.Employee).Column("EmployeeId").Not.Nullable().Not.LazyLoad();
            References(x => x.Client).Column("ClientId").Not.Nullable().Not.LazyLoad();
        }
    }
}
