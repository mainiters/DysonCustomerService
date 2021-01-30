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
        static void Main(string[] args)
        {
            var service = new ObmenCreation();

            service.Url = @"http://31.13.35.34/dyson_share_111/ws/ObmenCreation.1cws";

            service.Credentials = new NetworkCredential("yandex", "a123");

            service.PostTovars(new ПакетНоменклатуры()
            {
                
            });
        }
    }
}
