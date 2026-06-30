using System;
using System.Collections.Generic;
using System.Text;

namespace Vodovoz.Domain.Exceptions
{
    /// <summary>
    /// Класс исключений, сообщающих об ошибке при инициализации базы данных
    /// </summary>
    public class DBInitializationException : Exception
    {
        public DBInitializationException(string message, Exception innerException) : base(message, innerException)
        { 
        }

        /// <summary>
        /// Полное сообщение об ошибке, включая все вложенные исключения
        /// </summary>
        public string DetailedMessage => this.GetAllMessages();
    }
}
