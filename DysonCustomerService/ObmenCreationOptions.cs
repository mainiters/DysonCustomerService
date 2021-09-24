using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonCustomerService
{
    /// <summary>
    /// Класс для группировки параметров инициализации сервиса
    /// </summary>
    public class ObmenCreationOptions
    {
        /// <summary>
        /// Адрес сервиса
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Имя пользователя для авторизации
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль для авторизации
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Тайаут (время ожидания ответа от сервиса)
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Задержка перед повторными попытками отправки запроса в случае возникновения ошибки
        /// </summary>
        public int RetryDelay { get; set; }

        /// <summary>
        /// Количество повторных попыток отправки запроса в севрвис в случае возникновения ошибки
        /// </summary>
        public int RetryLimit { get; set; }

        /// <summary>
        /// Массив кодов ошибок, требующих повторной отправки данных в сервис
        /// </summary>
        public List<string> RetryErrorCodes { get; set; }
    }
}
