using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Protocols;

namespace DysonCustomerService.EntityDataProviders
{
    public class SoapMessageLogger : SoapExtension
    {
        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return null;
        }

        public override object GetInitializer(Type serviceType)
        {
            return null;
        }

        public override void Initialize(object initializer)
        {
           
        }

        public override void ProcessMessage(SoapMessage message)
        {
            switch (message.Stage)
            {
                case SoapMessageStage.BeforeSerialize:
                    break;
                case SoapMessageStage.AfterSerialize:
                    Console.WriteLine(message);
                    break;
                case SoapMessageStage.BeforeDeserialize:
                    Console.WriteLine(message);
                    break;
                case SoapMessageStage.AfterDeserialize:
                    break;
            }
        }
    }
}
