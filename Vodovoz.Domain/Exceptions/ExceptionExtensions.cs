using System;
using System.Collections.Generic;
using System.Text;

namespace Vodovoz.Domain.Exceptions
{
    /// <summary>
    /// Методы-расширения для работы с исключениями.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Собирает сообщения из всей цепочки исключений (включая InnerException).
        /// </summary>
        public static string GetAllMessages(this Exception ex)
        {
            var messages = new List<string>();
            var current = ex;
            int level = 0;

            while (current != null)
            {
                if (!string.IsNullOrWhiteSpace(current.Message))
                {
                    var indent = new string(' ', level * 2);
                    var indentStr = string.Empty;
                    if (level > 0)
                    {
                        indentStr = $"{indent} Inner exception {level}: ";
                    }

                    messages.Add($"{indentStr}{current.Message}");
                }
                current = current.InnerException;
                level++;
            }
            return string.Join("\n", messages);
        }
    }
}
