using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Text;
using Vodovoz.Data.Config;
using Vodovoz.Data.Mappings;

namespace Vodovoz.Data.Services
{
    /// <summary>
    /// Реализация провайдера фабрики сессий.
    /// Конфигурирует NHibernate через FluentNHibernate:
    /// подключение к MySQL, загрузка маппингов из текущей сборки,
    /// автоматическое обновление схемы БД (SchemaUpdate).
    /// </summary>
    public class SessionFactoryProvider : ISessionFactoryProvider
    {
        private readonly ISessionFactory _sessionFactory;

        /// <summary>
        /// Создать провайдер, настроив подключение и маппинги NHibernate.
        /// При создании автоматически экспортирует схему БД.
        /// </summary>
        /// <param name="config">Конфигурация базы данных (строка подключения).</param>
        public SessionFactoryProvider(DatabaseConfig config)
        {
            _sessionFactory = Fluently.Configure()
                .Database(MySQLConfiguration.Standard.ConnectionString(config.ConnectionString))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<EmployeeMap>())
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, false))
                .BuildSessionFactory();
        }

        /// <inheritdoc/>
        public ISessionFactory SessionFactory => _sessionFactory;

        /// <inheritdoc/>
        public ISession OpenSession() => _sessionFactory.OpenSession();
    }
}
