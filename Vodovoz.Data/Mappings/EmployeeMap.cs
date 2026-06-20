using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Text;
using Vodovoz.Domain.Entities;

namespace Vodovoz.Data.Mappings
{
    /// <summary>
    /// Маппинг NHibernate для модели Employee
    /// </summary>
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Table("Employees");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name).Length(50).Not.Nullable();
            Map(x => x.Surname).Length(50).Not.Nullable();
            Map(x => x.Patronymic).Length(50);
            Map(x => x.Position).CustomType<int>().Not.Nullable();
            Map(x => x.BirthDate).Not.Nullable();
        }
    }
}
