using System;
using System.Collections.Generic;
using System.Text;
using Vodovoz.Data.Services;
using Vodovoz.Domain.Interfaces;

namespace Vodovoz.Data.Repositories
{
    /// <summary>
    /// Универсальный репозиторий — базовая реализация CRUD для любой сущности.
    /// Использует NHibernate сессии через ISessionFactoryProvider.
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    public class BaseRepository<T>(ISessionFactoryProvider sessionProvider) : IBaseRepository<T> where T : class
    {
        /// <summary>
        /// Провайдер сессий NHibernate.
        /// </summary>
        protected readonly ISessionFactoryProvider SessionProvider = sessionProvider;

        public virtual T? GetById(int id)
        {
            using var session = SessionProvider.OpenSession();
            return session.Get<T>(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            using var session = SessionProvider.OpenSession();
            return session.QueryOver<T>().List<T>();
        }

        public virtual void Save(T entity)
        {
            using var session = SessionProvider.OpenSession();
            using var transaction = session.BeginTransaction();
            try
            {
                session.SaveOrUpdate(entity);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public virtual void Delete(T entity)
        {
            using var session = SessionProvider.OpenSession();
            using var transaction = session.BeginTransaction();
            try
            {
                session.Delete(entity);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
