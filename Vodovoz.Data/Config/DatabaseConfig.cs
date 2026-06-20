using System;
using System.Collections.Generic;
using System.Text;

namespace Vodovoz.Data.Config
{
    /// <summary>
    /// Rонфигурация базы данных.
    /// Хранит строку подключения
    /// </summary>
    public class DatabaseConfig
    {
        public string ConnectionString { get; set; } = string.Empty;
    }
}
