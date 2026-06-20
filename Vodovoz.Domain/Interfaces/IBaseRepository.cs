using System;
using System.Collections.Generic;
using System.Text;

namespace Vodovoz.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс обобщенного репозитория для работы с сущностями.
    /// Определяет базовые CRUD-операции: получение по Id, получение всех,
    /// сохранение и удаление.
    /// </summary>
    /// <typeparam name="T">Тип сущности.</typeparam>
    public interface IBaseRepository<T> where T : class
    {
        T? GetById(int id);

        IEnumerable<T> GetAll();

        void Save(T entity);

        void Delete(T entity);
    }
}
