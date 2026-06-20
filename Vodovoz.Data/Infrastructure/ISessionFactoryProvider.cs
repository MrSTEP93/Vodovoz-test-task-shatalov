using NHibernate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vodovoz.Data.Services
{
    /// <summary>
    /// Интерфейс провайдера для получения фабрики сессий NHibernate и открытия сессий
    /// </summary>
    public interface ISessionFactoryProvider
    {
        /// <summary>
        /// Фабрика сессий NHibernate
        /// </summary>
        ISessionFactory SessionFactory { get; }

        /// <summary>
        /// Открыть новую сессию NHibernate для работы с БД
        /// </summary>
        ISession OpenSession();
    }
}
