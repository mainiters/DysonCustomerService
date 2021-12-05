using System;
using System.Net;
using Terrasoft.Core;
//using Terrasoft.Configuration;

namespace DysonPromocodeService
{
    public class ObmenCreationWrapper
    {
        static protected PromocodeStatus service { get; set; }
        protected ObmenCreationOptions options { get; set; }
        protected UserConnection userConnection { get; set; }

        public ObmenCreationWrapper(ObmenCreationOptions options)
        {
            this.options = options;

            if (service == null)
            {
                service = new PromocodeStatus();
            }

            service.Url = options.Url;

            service.Credentials = new NetworkCredential(options.Login, options.Password);
        }

        public ObmenCreationWrapper(UserConnection userConnection, ObmenCreationOptions options = null)
        {
            this.userConnection = userConnection;

            if (options != null)
            {
                this.options = options;
            }
            else
            {
                this.options = new ObmenCreationOptions()
                {
                    Url = Terrasoft.Core.Configuration.SysSettings.GetValue<string>(userConnection, "Promocode1CServiceURL", string.Empty),
                    Login = Terrasoft.Core.Configuration.SysSettings.GetValue<string>(userConnection, "Promocode1CServiceUsername", string.Empty),
                    Password = Terrasoft.Core.Configuration.SysSettings.GetValue<string>(userConnection, "Promocode1CServicePassword", string.Empty)
                };
            }

            if (service == null)
            {
                InitService();
            }
        }

        protected void InitService()
        {
            service = new PromocodeStatus();

            service.Url = options.Url;

            service.Credentials = new NetworkCredential(options.Login, options.Password);
        }

        public string SendPromocode(string promocode, string articles)
        {
            try
            {
                var response = service.GetStatus(promocode, articles);
                response = clearResponse(response);
                return response;
            }
            catch (ArgumentException e)
            {
                throw e;
            }
        }

        private static string clearResponse(string source)
        {
            string cut = source.Replace("\n\t\t", "");
            cut = cut.Replace("\n\t", "");
            cut = cut.Replace("\n", "");
            cut = cut.Replace("<? xml version =\"1.0\"?>", "");
            int IndexFirst = cut.IndexOf("<Code>");
            cut = cut.Replace("\\n", "");
            return cut;
        }
    }
}
