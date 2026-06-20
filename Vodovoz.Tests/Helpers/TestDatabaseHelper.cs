using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Text;
using Vodovoz.Data.Config;
using Vodovoz.Data.Infrastructure;
using Vodovoz.Data.Mappings;
using Vodovoz.Data.Services;

namespace Vodovoz.Tests.Helpers
{
    public static class TestDatabaseHelper
    {
        private static ISessionFactoryProvider? _provider;
        private static string? _connectionString;

        private static string GetConnectionString()
        {
            if (_connectionString == null)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: false)
                    .Build();

                _connectionString = configuration.GetConnectionString("TestConnection")
                    ?? throw new InvalidOperationException(
                        "Строка подключения 'TestConnection' не найдена в appsettings.Test.json");
            }
            return _connectionString;
        }

        public static ISessionFactoryProvider GetProvider()
        {
            if (_provider == null)
            {
                var dbConfig = new DatabaseConfig
                {
                    ConnectionString = GetConnectionString()
                };
                _provider = new SessionFactoryProvider(dbConfig);
            }
            return _provider;
        }

        public static ISession OpenSession() => GetProvider().OpenSession();

        public static void ClearAllTables()
        {
            using var session = OpenSession();
            using var tx = session.BeginTransaction();

            session.CreateSQLQuery("DELETE FROM Orders").ExecuteUpdate();
            session.CreateSQLQuery("DELETE FROM Clients").ExecuteUpdate();
            session.CreateSQLQuery("DELETE FROM Employees").ExecuteUpdate();

            tx.Commit();
        }
    }

}