using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DysonCustomerService
{
    class Program
    {
        const string URL = @"http://31.13.35.34/dyson_share_111/ws/ObmenCreation.1cws";
        const string LOGIN = "yandex";
        const string PASSWORD = "a123";

        static void Main(string[] args)
        {
            ObmenCreationWrapper wrapper = new ObmenCreationWrapper(new ObmenCreationOptions()
            {
                Url = URL,
                Login = LOGIN,
                Password = PASSWORD
            });
        }
    }
}
