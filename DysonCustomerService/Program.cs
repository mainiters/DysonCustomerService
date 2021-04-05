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
            ObmenCreationWrapper wrapper = new ObmenCreationWrapper(new ObmenCreationOptions()
            {
                Url = @"http://31.13.35.34/dyson_share_111/ws/ObmenCreation.1cws",
                Login = "yandex",
                Password = "a123",
                RetryDelay = 3000,
                RetryLimit = 3,
                RetryErrorCodes = new List<string>()
            });

            var task = wrapper.SendRequest("PostTovars", new ПакетНоменклатуры()
            {
                
            });

            task.Wait();
        }
    }
}
