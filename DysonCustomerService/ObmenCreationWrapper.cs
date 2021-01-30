using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DysonCustomerService
{
    public class ObmenCreationWrapper
    {
        protected ObmenCreation service { get; set; }
        protected ObmenCreationOptions options { get; set; }

        public ObmenCreationWrapper(ObmenCreationOptions options)
        {
            this.options = options;

            this.service = new ObmenCreation();

            service.Url = options.Url;

            service.Credentials = new NetworkCredential(options.Login, options.Password);
        }
    }
}
