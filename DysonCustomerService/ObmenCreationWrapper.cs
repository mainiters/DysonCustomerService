using DysonCustomerService.EntityDataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace DysonCustomerService
{
    public class ObmenCreationWrapper
    {
        protected ObmenCreation service { get; set; }
        protected ObmenCreationOptions options { get; set; }
        protected UserConnection userConnection { get; set; }

        public ObmenCreationWrapper(ObmenCreationOptions options)
        {
            this.options = options;

            this.service = new ObmenCreation();

            service.Url = options.Url;

            service.Credentials = new NetworkCredential(options.Login, options.Password);
        }

        protected Dictionary<string, string> methodsToEntityNamesMap = null;

        protected Dictionary<string, string> MethodsToEntityNamesMap
        {
            get
            {
                if (methodsToEntityNamesMap == null)
                {
                    var esq = new EntitySchemaQuery(this.userConnection.EntitySchemaManager, "TrcEndpoint");

                    esq.AddAllSchemaColumns();

                    esq.Filters.Add(esq.CreateIsNotNullFilter("TrcEntityName"));

                    methodsToEntityNamesMap = esq.GetEntityCollection(this.userConnection).ToDictionary(e => e.GetTypedColumnValue<string>("Name"), e => e.GetTypedColumnValue<string>("TrcEntityName"));
                }

                return methodsToEntityNamesMap;
            }
        }

        protected BaseEntityDataProvider GetEntityDataProvider(string EntityName, Guid EntityId)
        {
            switch (EntityName)
            {
                case "TrcSerialNumberHistory":
                    return new SerialNumberHistoryDataProvider(EntityId, this.userConnection);
                case "SysAdminUnit":
                    return new SysAdminUnitDataProvider(EntityId, this.userConnection);
                case "Product":
                    return new ProductDataProvider(EntityId, this.userConnection);
                case "Account":
                    return new AccountDataProvider(EntityId, this.userConnection);
                case "Contact":
                    return new ContactDataProvider(EntityId, this.userConnection);
                case "TrcApplication":
                    return new ApplicationDataProvider(EntityId, this.userConnection);
                case "Order":
                    return new OrderDataProvider(EntityId, this.userConnection);
                case "TrcCustomerDefect":
                    return new CustomerDefectDataProvider(EntityId, this.userConnection);
            }

            throw new ArgumentException($"No DataProvider was found by EntityName {EntityName}");
        }

        protected string GetEntityNameByMethod(string methodName)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                throw new ArgumentException("The name of the method could not be empty or white space");
            }

            if (!MethodsToEntityNamesMap.ContainsKey(methodName))
            {
                throw new ArgumentException($"Method {methodName} was not found in TrcEndpoint");
            }

            return MethodsToEntityNamesMap[methodName];
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
                    Url = Terrasoft.Core.Configuration.SysSettings.GetValue<string>(userConnection, "TrcCreatioTo1cUrl", string.Empty),
                    Login = Terrasoft.Core.Configuration.SysSettings.GetValue<string>(userConnection, "TrcCreatioTo1cLogin", string.Empty),
                    Password = Terrasoft.Core.Configuration.SysSettings.GetValue<string>(userConnection, "TrcCreatioTo1cPassword", string.Empty),
                    RetryErrorCodes = Terrasoft.Core.Configuration.SysSettings.GetValue<string>(userConnection, "TrcCreatioTo1cRetryErrorCodes", string.Empty).Split(';').ToList(),
                    RetryDelay = Terrasoft.Core.Configuration.SysSettings.GetValue<int>(userConnection, "TrcCreatioTo1cRetryDelay", 60 * 1000),
                    RetryLimit = Terrasoft.Core.Configuration.SysSettings.GetValue<int>(userConnection, "TrcCreatioTo1cRetryLimit", 3),
                    Timeout = Terrasoft.Core.Configuration.SysSettings.GetValue<int>(userConnection, "TrcCreatioTo1cTimeout", 30000),
                };
            }

            InitService();
        }

        protected void InitService()
        {
            this.service = new ObmenCreation();

            this.service.Url = options.Url;

            this.service.Credentials = new NetworkCredential(options.Login, options.Password);
        }

        public async Task SendRequest(string methodName, Guid entityId)
        {
            var retryCount = 0;

            while (true)
            {
                try
                {
                    var entityName = GetEntityNameByMethod(methodName);

                    var dataProvider = GetEntityDataProvider(entityName, entityId);

                    service.PostData(dataProvider.GetServiceMethodName() ?? methodName, dataProvider.GetEntityData(entityId));
                }
                catch (ArgumentException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    if (this.options.RetryErrorCodes.Contains(e.Message) && retryCount < this.options.RetryLimit)
                    {
                        retryCount++;

                        await Task.Delay(this.options.RetryDelay);
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
        }
    }
}
