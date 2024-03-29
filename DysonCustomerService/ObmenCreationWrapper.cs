﻿using DysonCustomerService.EntityDataProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace DysonCustomerService
{
    /// <summary>
    /// Обертка над автоматически сгенерированным прокси-классом
    /// Связывает прокси-класс с логикой СРМ и используется для взаимодействия с сервисом
    /// </summary>
    public class ObmenCreationWrapper
    {
        /// <summary>
        /// Статический экземпляр службы
        /// (инициализируется один раз при первом запросе и далее используется без пересоздания)
        /// </summary>
        static protected ObmenCreation service { get; set; }

        /// <summary>
        /// Параметры инициализации службы
        /// </summary>
        protected ObmenCreationOptions options { get; set; }

        /// <summary>
        /// Соединение пользователя
        /// </summary>
        protected UserConnection userConnection { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="options">Параметры инициализации сервиса</param>
        public ObmenCreationWrapper(ObmenCreationOptions options)
        {
            this.options = options;

            if (service == null)
            {
                service = new ObmenCreation();
            }

            service.Url = options.Url;

            service.Credentials = new NetworkCredential(options.Login, options.Password);
        }

        /// <summary>
        /// Словарь, определяющий соответствие названий методов сервиса и сущностей, на базе которых строится запрос
        /// </summary>
        protected Dictionary<string, string> methodsToEntityNamesMap = null;

        /// <summary>
        /// Инициализатор словаря метод->сущность (использует справочник Метод из СРМ)
        /// </summary>
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

        /// <summary>
        /// Возвращает экземпляр провайдера данных в зависимости от наименования сущности СРМ (объекта интеграции)
        /// </summary>
        /// <param name="EntityName">Наименование сущности</param>
        /// <param name="EntityId">Идентификатор сущности</param>
        /// <returns></returns>
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
                case "ProductType":
                    return new ProductTypeDataProvider(EntityId, this.userConnection);
                case "ProductCategory":
                    return new ProductCategoryDataProvider(EntityId, this.userConnection);
                case "TrcProductSubcategory":
                    return new TrcProductSubcategoryDataProvider(EntityId, this.userConnection);
                case "TrcDeviceType":
                    return new TrcDeviceTypeDataProvider(EntityId, this.userConnection);
                case "Employee":
                    return new EmployeeDataProvider(EntityId, this.userConnection);
            }

            throw new ArgumentException($"No DataProvider was found by EntityName {EntityName}");
        }

        /// <summary>
        /// Обертка над словарем методов
        /// Возвращает название сущности по названию метода сервиса
        /// </summary>
        /// <param name="methodName">Название метода сервиса</param>
        /// <returns></returns>
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

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="userConnection">Соединение пользователя</param>
        /// <param name="options">Параметры инициализации сервиса (в случае их отсутствия, будут созданы на основании значений в системных натсройках конфигурации)</param>
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

            if (service == null)
            {
                InitService();
            }
        }

        /// <summary>
        /// Инициализация серсвиса
        /// </summary>
        protected void InitService()
        {
            service = new ObmenCreation();

            service.Url = options.Url;

            service.Credentials = new NetworkCredential(options.Login, options.Password);
        }

        /// <summary>
        /// Метод отправки запроса в сервис (передача интеграционной сущности)
        /// </summary>
        /// <param name="methodName">Название интеграционного метода</param>
        /// <param name="entityId">Идентификтаор сущности</param>
        /// <param name="requestStr">Выходной параметр, хранящий текст запроса</param>
        /// <param name="responseStr">Выходной параметр, хранящий текст ответа от сервиса</param>
        public void SendRequest(string methodName, Guid entityId, out string requestStr, out string responseStr)
        {
            var retryCount = 0;

            requestStr = string.Empty;
            responseStr = string.Empty;

            while (true)
            {
                try
                {
                    var entityName = GetEntityNameByMethod(methodName);

                    var dataProvider = GetEntityDataProvider(entityName, entityId);

                    var data = dataProvider.GetEntityData(entityId);

                    requestStr = ToXml(data);

                    var response = service.PostData(dataProvider.GetServiceMethodName() ?? methodName, data);

                    responseStr = ToXml(response);

                    dataProvider.ProcessResponse(response);

                    break;
                }
                catch (ArgumentException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    if (false && this.options.RetryErrorCodes.Contains(e.Message) && retryCount < this.options.RetryLimit)
                    {
                        retryCount++;

                        Thread.Sleep(this.options.RetryDelay);
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// Конвертация объекта запроса в XML представление
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected string ToXml(object data)
        {

            var removingNs = " xmlns=\"http://crm1ctest.dyson.ru\"";

            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true
            };
            var ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                var serializer = new XmlSerializer(data.GetType());
                serializer.Serialize(writer, data, ns);
                var res = stream.ToString();
                return res;
            }
        }
    }
}
