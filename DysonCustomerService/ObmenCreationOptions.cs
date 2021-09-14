using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonCustomerService
{
    public class ObmenCreationOptions
    {
        //а где происходит чтение\заполнение этих настроек?
        public string Url { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int Timeout { get; set; }
        public int RetryDelay { get; set; }
        public int RetryLimit { get; set; }
        public List<string> RetryErrorCodes { get; set; }
    }
}
