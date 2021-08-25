using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Terrasoft.Common;
using Terrasoft.Core;
using Terrasoft.Core.DB;
using Terrasoft.Core.Entities;
using Terrasoft.Web.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Terrasoft.Common;
using Terrasoft.Core;
using Terrasoft.Core.DB;
using Terrasoft.Core.Entities;
using Terrasoft.Web.Common;
using Terrasoft.Core.Process;
using Terrasoft.Core.Process.Configuration;
using System.Globalization;
using System.Threading;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using System.Web;
using Terrasoft;
using Terrasoft.Common;
using Terrasoft.Core;
using Terrasoft.Core.DB;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Store;
using Terrasoft.Core.Factories;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Terrasoft.Trace
{
    [DataContract]
    public class ProductStockBalance
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public double RequestedQuantity { get; set; }

        [DataMember]
        public bool TrcPersonalization { get; set; }

        [DataMember]
        public Guid ProductCategoryId { get; set; }

        [DataMember]
        public Guid ProductId { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public double TotalQuantity { get; set; }

        [DataMember]
        public double ReserveQuantity { get; set; }

        [DataMember]
        public double AvailableQuantity { get; set; }

        [DataMember]
        public Guid WarehouseId { get; set; }

        [DataMember]
        public string WarehouseName { get; set; }
    }

    [DataContract]
    public class ProductInfo
    {
        [DataMember]
        public double requestedQuantity { get; set; }

        [DataMember]
        public Dictionary<Guid, double> stockBalances { get; set; }
    }

    [DataContract]
    public class MessageParams
    {
        [DataMember]
        public string action { get; set; }

        [DataMember]
        public string message { get; set; }

        [DataMember]
        public Dictionary<string, object> parameters { get; set; }

        [DataMember]
        public Dictionary<Guid, double> availableProducts { get; set; }

        [DataMember]
        public Dictionary<Guid, double> availableProductsInClientCityStock { get; set; }

        [DataMember]
        public Dictionary<Guid, double> availableProductsInNotClientCityStock { get; set; }

        [DataMember]
        public Dictionary<Guid, double> notAvailableProductsInClientCityStock { get; set; }

        [DataMember]
        public Dictionary<Guid, double> notAvailableProductsInNotClientCityStock { get; set; }

        [DataMember]
        public Dictionary<Guid, double> availableProductsOnAnotherStock { get; set; }

        [DataMember]
        public Dictionary<Guid, double> notAvailableProducts { get; set; }

        [DataMember]
        public Dictionary<Guid, double> notAvailableStocksProducts { get; set; }

        [DataMember]
        public Dictionary<Guid, ProductInfo> productInfos { get; set; }
    }

    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class TrcPreOrderHelper
    {
        protected UserConnection UserConnection;

        public TrcPreOrderHelper()
        {
        	MainStocks = new List<Guid>()
	        {
	            TechnoParkId,
	            ProstorId,
	            MakkenziId
	        };
        }

        public TrcPreOrderHelper(UserConnection UserConnection)
        {
            this.UserConnection = UserConnection;
            
            MainStocks = new List<Guid>()
	        {
	            TechnoParkId,
	            ProstorId,
	            MakkenziId
	        };
        }

        Guid TechnoParkId = new Guid("9ebc0317-4f14-4205-99dc-1db208be78c2");
        Guid ProstorId = new Guid("80d8d1c8-ad69-4135-a030-d91fbdb4a980");
        Guid MakkenziId = new Guid("b8a5bee9-3244-4801-9bca-1c8bd0ab2459");

        List<Guid> MainStocks;

        Guid SpbKcMainId = new Guid("7ea0bedd-f738-43b3-88b2-4ea000310db3");
        Guid KcSokolMain = new Guid("f60921e0-6046-445b-8083-5a2428c4a631");

        Guid MoscowCityId = new Guid("1c0b6b13-e8bb-df11-b00f-001d60e938c6");
        Guid SpbCityId = new Guid("190b6b13-e8bb-df11-b00f-001d60e938c6");
        Guid ProcessingStatus = new Guid("0f48842d-eb6b-45f6-adab-0ead17253e38");


        Guid NonReconciliationStatusId = new Guid("14762f1e-6a4d-4feb-98ff-809f1bb398a0");

        Guid SparePartsCategoryId = new Guid("897c1bc6-36c9-4e5f-8998-216d4b5e81d5");

        List<string> NonUpdatableFieldNames = new List<string>()
        {
            "TrcFoilColorId",
            "TrcPersonalization",
            "TrcInitials",
            "TrcPromocode",
            "TrcSalesSourceId",
            "DiscountPercent",
            "TotalAmount",
            "PrimaryPrice",
            "Price",
            "PrimaryAmount",
            "Amount",
            "PrimaryDiscountAmount",
            "DiscountAmount",
            "DiscountPercent",
            "PrimaryTaxAmount",
            "TaxAmount",
            "PrimaryTotalAmount",
            "TotalAmount",
            "DiscountTax",
            "CurrencyRate"
        };

        List<string> FieldsToCleanUpOrder = new List<string>()
        {
            "TrcOrderDeliveryWayId",
            "TrcDeliveryDate",
            "TrcPaymentId",
            "TrcOrcerPaymentWayId",
            "TrcPromotionalCode",
            "TrcDeliveryCost",
            "TrcIntervalDeliveryId",
            "TrcDeliveryCompanyId",
            "TrcWarehouseForShippingOrderId",
            "TrcPVZId",
            "TrcServiceCenterId",
            "TrcDirectId",
            "TrcOrganizationId",
            "TrcDeliveryFromTime",
            "TrcDeliveryToTime",
            "TrcDesiredDeliveryDate"
        };
        
        object OrderCleanLock = new object();

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        public void ReserveOrderCart(string orderId, string warehouseId, bool reserve = true)
        {
            if (UserConnection == null)
            {
                UserConnection = (UserConnection)HttpContext.Current.Session["UserConnection"];
            }

            var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "OrderProduct");

            esq.AddAllSchemaColumns();

            esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Order", orderId));

            var orderProducts = esq.GetEntityCollection(UserConnection);

            foreach (var item in orderProducts)
            {
                var balanceEsq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "ProductStockBalance");

                balanceEsq.AddAllSchemaColumns();

                balanceEsq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Product", item.GetTypedColumnValue<Guid>("ProductId")));
                balanceEsq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Warehouse", orderId));

                var count = reserve ? item.GetTypedColumnValue<double>("Quantity") : item.GetTypedColumnValue<double>("Quantity") * (-1);

                var balanceItem = balanceEsq.GetEntityCollection(UserConnection).FirstOrDefault();

                if(balanceItem != null)
                {
                    balanceItem.SetColumnValue("ReserveQuantity", balanceItem.GetTypedColumnValue<double>("ReserveQuantity") + count);
                    balanceItem.SetColumnValue("AvailableQuantity", balanceItem.GetTypedColumnValue<double>("AvailableQuantity") - count);

                    balanceItem.Save();
                }
            }
        }

		[OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        public void ChangeStatus(string orderId, string statusId)
        {
        	if (UserConnection == null)
            {
                UserConnection = (UserConnection)HttpContext.Current.Session["UserConnection"];
            }
            
        	var update = new Update(UserConnection, "Order")
	            .Set("StatusId", Column.Parameter(Guid.Parse(statusId)))
	            .Where("Id").IsEqual(Column.Parameter(orderId));
	            
            update.Execute();
        }
        
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        public void CleanUpFieldsOnChange(string orderId, List<string> changedValues)
        {
        	if (UserConnection == null)
            {
                UserConnection = (UserConnection)HttpContext.Current.Session["UserConnection"];
            }
            
    		if(string.IsNullOrEmpty(orderId)) return;
    	
            var orderEntity = GetOrderEntityById(Guid.Parse(orderId));

            if (orderEntity.GetTypedColumnValue<bool>("TrcIsProcessing"))
            {
                return;
            }

            if (changedValues == null || changedValues.Any(e => !NonUpdatableFieldNames.Contains(e)))
            {
                if (orderEntity.GetTypedColumnValue<Guid>("Status_Id") != NonReconciliationStatusId)
                {
                    orderEntity.SetColumnValue("StatusId", NonReconciliationStatusId);

                    foreach (var item in FieldsToCleanUpOrder)
                    {
                        orderEntity.SetColumnValue(item, null);
                    }
                    
                    orderEntity.SetColumnValue("TrcPersonalizationService", false);

                    orderEntity.Save();
                    
                    var update = new Update(UserConnection, "OrderProduct")
	                    .Set("TrcSalesSourceId", Column.Parameter(Guid.Parse("d563fa49-1528-4bef-ba99-00b4a67db66f")))
	                    .Set("TrcPersonalization", Column.Parameter(false))
	                    .Set("TrcPromocode", Column.Parameter(string.Empty))
	                    .Set("TrcInitials", Column.Parameter(string.Empty))
	                    .Set("TrcFoilColorId", Column.Const(null))
	                    .Set("DiscountPercent", Column.Parameter(0d))
	                    .Where("OrderId").IsEqual(Column.Parameter(orderId));
                    
                    update.Execute();
                    
                    var message = new MessageParams()
                    {
                        action = "reloadEntity",
                        parameters = new Dictionary<string, object>()
                        {
                            { "currentCart", orderEntity.GetTypedColumnValue<string>("TrcReservedCart") }
                        }
                    };

                    SendMessage(message);
                }
            }
        }

        protected Entity GetOrderEntityById(Guid orderId)
        {
            var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "Order");

            esq.AddColumn("Id");
            esq.AddColumn("Status.Id");
            esq.AddAllSchemaColumns();

            var orderEntity = esq.GetEntity(UserConnection, orderId);

            return orderEntity;
        }

        protected Dictionary<Guid, double> GetOrderProductsById(Guid orderId)
        {
            var res = new Dictionary<Guid, double>();

            var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "OrderProduct");

            esq.AddColumn("Id");
            esq.AddColumn("Product.Id");
            esq.AddColumn("Quantity");
            esq.AddColumn("TrcReservedCart");

            esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Order", orderId));

            var orderProducts = esq.GetEntityCollection(UserConnection);

            foreach (var item in orderProducts)
            {
                var productId = item.GetTypedColumnValue<Guid>("Product_Id");
                var productQuantity = item.GetTypedColumnValue<double>("Quantity");

                if (!res.ContainsKey(productId))
                {
                    res.Add(productId, 0);
                }

                res[productId] += productQuantity;
            }

            return res;
        }

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateEntity(string orderId, List<string> changedValues)
        {
            if (UserConnection == null)
            {
                UserConnection = (UserConnection)HttpContext.Current.Session["UserConnection"];
            }

            var OrderId = Guid.Parse(orderId);

            if (changedValues == null || changedValues.Any(e => !NonUpdatableFieldNames.Contains(e)))
            {
                var orderEntity = GetOrderEntityById(OrderId);

                if (orderEntity.GetTypedColumnValue<bool>("TrcIsProcessing"))
                {
                    return;
                }

                try
                {	
                    if(!string.IsNullOrEmpty(orderEntity.GetTypedColumnValue<string>("TrcOrderCode1C")) && orderEntity.GetTypedColumnValue<bool>("TrcPreorder"))
                    {
                        var emptyMessage = new MessageParams()
                        {
                            action = "setWarehouseOnly"
                        };

                        SendMessage(emptyMessage);

                        return;
                    }

                	if(orderEntity.GetTypedColumnValue<Guid>("StatusId") != NonReconciliationStatusId)
                	{
                		orderEntity.SetColumnValue("StatusId", NonReconciliationStatusId);
                		foreach (var item in FieldsToCleanUpOrder)
	                    {
	                        orderEntity.SetColumnValue(item, null);
	                    }
                	}
                    
                    orderEntity.SetColumnValue("TrcIsProcessing", true);
                    orderEntity.Save();

                    var balances = GetProductStockBalances(OrderId, this.UserConnection);

					if(balances.Count < 1)
					{
						var emptyMessage = new MessageParams()
                        {
                            action = "emptyStocks"
                        };
                        
                        SendMessage(emptyMessage);
                        
                        return;
					}

                    var spareParts = balances.Where(e => e.ProductCategoryId == SparePartsCategoryId).ToList();
                    var sparePartsProducts = new Dictionary<Guid, double>();

                    foreach (var item in spareParts)
                    {
                        if(!sparePartsProducts.ContainsKey(item.ProductId))
                        {
                            sparePartsProducts.Add(item.ProductId, item.RequestedQuantity);
                        }
                    }

                    var nonSpareParts = balances.Where(e => e.ProductCategoryId != SparePartsCategoryId).ToList();
                    var nonSparePartsProducts = new Dictionary<Guid, double>();

                    foreach (var item in nonSpareParts)
                    {
                        if (!nonSparePartsProducts.ContainsKey(item.ProductId))
                        {
                            nonSparePartsProducts.Add(item.ProductId, item.RequestedQuantity);
                        }
                    }

                    var isFullCartAvailableInTechnoPark = IsFullCartAvailableInStock(balances, TechnoParkId);
                    var isFullCartAvailableInProstor = IsFullCartAvailableInStock(balances, ProstorId);

                    var areAllProductsSpareParts = AreAllProductsSpareParts(balances);
                    var hasAnyProductSparePart = HasAnyProductSparePart(balances);

                    var clientId = orderEntity.GetColumnValue("ContactId") == null ? orderEntity.GetTypedColumnValue<Guid>("AccountId") : orderEntity.GetTypedColumnValue<Guid>("ContactId");
                    var clientCityId = GetAddressCity(clientId, orderEntity.GetTypedColumnValue<string>("DeliveryAddress"));

                    var direct = GetDirectRetailStoresByCity(clientCityId).Where(e => IsFullCartAvailableInStock(balances, e.GetTypedColumnValue<Guid>("TrcWarehouseId")))
                            .Select(e => e.GetTypedColumnValue<Guid>("Id"))
                            .ToList();

                    var ascKc = GetAscKcByCity(clientCityId).Where(e => IsFullCartAvailableInStock(balances, e.GetTypedColumnValue<Guid>("TrcStockId")))
                            .Select(e => e.GetTypedColumnValue<Guid>("Id"))
                            .ToList();
                            
                    if (areAllProductsSpareParts || (hasAnyProductSparePart && !isFullCartAvailableInTechnoPark && !isFullCartAvailableInProstor))
                    {
                        var isFullCartAvailableInSpbKcMain = IsFullCartAvailableInStock(balances, SpbKcMainId);
                        var isFullCartAvailableInKcSokolMain = IsFullCartAvailableInStock(balances, KcSokolMain);

                        var message = new MessageParams()
                        {
                            action = "spareParts",
                            productInfos = GetProductInfos(balances),
                            parameters = new Dictionary<string, object>()
                            {
                                { "areAllProductsSpareParts", areAllProductsSpareParts },
                                { "hasAnyProductSparePart", hasAnyProductSparePart },
                                { "isFullCartAvailableInTechnoPark", isFullCartAvailableInTechnoPark },
                                { "isFullCartAvailableInProstor", isFullCartAvailableInProstor },
                                { "isFullCartAvailableInSpbKcMain", isFullCartAvailableInSpbKcMain },
                                { "isFullCartAvailableInKcSokolMain", isFullCartAvailableInKcSokolMain },
                                { "sparePartsProducts", sparePartsProducts },
                                { "nonSparePartsProducts", nonSparePartsProducts },
                                { "clientCityId", clientCityId }

                            }
                        };

                        SendMessage(message);

                        return;
                    }

                    var isPartCartAvailableInTechnoPark = IsPartCartAvailableInStock(balances, TechnoParkId);
                    var isPartCartAvailableInProstor = IsPartCartAvailableInStock(balances, ProstorId);
                    var isFullCartAvailableInStocks = IsFullCartAvailableInStocks(balances, TechnoParkId, ProstorId);
                    
                    var hasPersonality = balances.Any(e => e.TrcPersonalization);

                    Guid TrcWarehouseForShippingOrderId = Guid.Empty;

                    if (isFullCartAvailableInTechnoPark && isFullCartAvailableInProstor)
                    {
                        TrcWarehouseForShippingOrderId = clientCityId == MoscowCityId ? TechnoParkId : ProstorId;
                    }
                    else if (isFullCartAvailableInTechnoPark)
                    {
                        TrcWarehouseForShippingOrderId = TechnoParkId;
                    }
                    else if (isFullCartAvailableInProstor)
                    {
                        TrcWarehouseForShippingOrderId = ProstorId;
                    }

                    if (isFullCartAvailableInTechnoPark || isFullCartAvailableInProstor)
                    {
                        var message = new MessageParams()
                        {
                            action = "fullAvailable",
                            message = "Необходимо оформить услугу персонализации?",
                            productInfos = GetProductInfos(balances),
                            parameters = new Dictionary<string, object>()
                            {
                                { "warehouseForShippingOrderId", TrcWarehouseForShippingOrderId },
                                { "hasPersonality", hasPersonality },
                                { "isFullCartAvailableInTechnoPark", isFullCartAvailableInTechnoPark },
                                { "isFullCartAvailableInProstor", isFullCartAvailableInProstor },
                                { "isPartCartAvailableInTechnoPark", isPartCartAvailableInTechnoPark },
                                { "isPartCartAvailableInProstor", isPartCartAvailableInProstor },
                                { "isFullCartAvailableInStocks", isFullCartAvailableInStocks },
                                { "direct", direct },
                                { "ascKc", ascKc },
                            }
                        };

                        SendMessage(message);
                    }
                    else
                    {
                        List<Guid> WarehouseIds = new List<Guid>();

                        var stockByCity = clientCityId == MoscowCityId ? TechnoParkId : ProstorId;
                        var anotherStockByClient = clientCityId == MoscowCityId ? ProstorId : TechnoParkId;

                        if (!isPartCartAvailableInTechnoPark && isPartCartAvailableInProstor)
                        {
                            stockByCity = ProstorId;
                            WarehouseIds.Add(stockByCity);
                        }
                        else if (isPartCartAvailableInTechnoPark && !isPartCartAvailableInProstor)
                        {
                            stockByCity = TechnoParkId;
                            WarehouseIds.Add(stockByCity);
                        }
                        else if (isFullCartAvailableInStocks || (isPartCartAvailableInTechnoPark && isPartCartAvailableInProstor))
                        {
                            WarehouseIds.Add(stockByCity);
                            WarehouseIds.Add(anotherStockByClient);
                        }

                        var products = balances.Where(e => WarehouseIds.Contains(e.WarehouseId)).ToList();
						
                        var presentProducts = balances.Where(e => (e.WarehouseId == ProstorId || e.WarehouseId == TechnoParkId)
                            && e.AvailableQuantity > 0).ToList();

                        var absentProducts = balances.Where(e => (e.WarehouseId == ProstorId || e.WarehouseId == TechnoParkId)
                            && e.AvailableQuantity <= 0).ToList();

                        var availableProducts = products.Where(e => e.WarehouseId == stockByCity)
                            .Where(e => e.RequestedQuantity <= e.AvailableQuantity)
                            .ToDictionary(e => e.ProductId, e => e.RequestedQuantity - e.AvailableQuantity);

                        var availableProductsOnAnotherStock = products.Where(e => e.WarehouseId == anotherStockByClient)
                            .Where(e => e.RequestedQuantity <= e.AvailableQuantity)
                            .ToDictionary(e => e.ProductId, e => e.RequestedQuantity - e.AvailableQuantity);

                        var notAvailableProducts = products.Where(e => e.WarehouseId == stockByCity)
                            .Where(e => e.RequestedQuantity > e.AvailableQuantity)
                            .ToDictionary(e => e.ProductId, e => e.AvailableQuantity > 0 ? e.RequestedQuantity - e.AvailableQuantity : e.RequestedQuantity);

                        var allProductsInMainStocks = products.Where(e => e.WarehouseId == stockByCity || e.WarehouseId == anotherStockByClient);

                        var notAvailableStocksProducts = new Dictionary<Guid, double>();

                        foreach (var item in allProductsInMainStocks)
                        {
                            if (!notAvailableStocksProducts.ContainsKey(item.ProductId))
                            {
                                notAvailableStocksProducts.Add(item.ProductId, item.RequestedQuantity);
                            }

                            if (item.AvailableQuantity > 0)
                            {
                                notAvailableStocksProducts[item.ProductId] -= item.AvailableQuantity;
                            }
                        }

                        notAvailableStocksProducts = notAvailableStocksProducts.Where(e => e.Value > 0).ToDictionary(e => e.Key, e => e.Value);

                        var availableProductsInClientCityStock = products.Where(e => e.WarehouseId == (clientCityId == MoscowCityId ? TechnoParkId : ProstorId))
                            .Where(e => e.RequestedQuantity > 0 && e.AvailableQuantity > 0)
                            .ToDictionary(e => e.ProductId, e => e.RequestedQuantity > e.AvailableQuantity ? e.AvailableQuantity : e.RequestedQuantity);

                        var availableProductsInNotClientCityStock = products.Where(e => e.WarehouseId == (clientCityId != MoscowCityId ? TechnoParkId : ProstorId))
                            .Where(e => e.RequestedQuantity > 0 && e.AvailableQuantity > 0)
                            .ToDictionary(e => e.ProductId, e => e.RequestedQuantity > e.AvailableQuantity ? e.AvailableQuantity : e.RequestedQuantity);

                        var notAvailableProductsInClientCityStock = products.Where(e => e.WarehouseId == (clientCityId == MoscowCityId ? TechnoParkId : ProstorId))
                            .Where(e => e.RequestedQuantity > e.AvailableQuantity)
                            .ToDictionary(e => e.ProductId, e => e.RequestedQuantity - e.AvailableQuantity);

                        var notAvailableProductsInNotClientCityStock = products.Where(e => e.WarehouseId == (clientCityId != MoscowCityId ? TechnoParkId : ProstorId))
                            .Where(e => e.RequestedQuantity > e.AvailableQuantity)
                            .ToDictionary(e => e.ProductId, e => e.RequestedQuantity - e.AvailableQuantity);

                        var availableProductsString = GetProductsString(presentProducts, absentProducts, true);
                        var isFullCartAvailableInMakkenzi = IsFullCartAvailableInStock(balances, MakkenziId);
                        var isPartCartAvailableInMakkenzi = IsPartCartAvailableInStock(balances, MakkenziId, notAvailableProducts.Keys.ToList());

                        var isFullCartAvailableInDirect = direct.Count > 0;
                        var isFullCartAvailableInAscKc = ascKc.Count > 0;

                        var message = new MessageParams()
                        {
                            action = "partAvailable",
                            message = availableProductsString,
                            parameters = new Dictionary<string, object>()
                            {
                                { "warehouseId", WarehouseIds },
                                { "isFullCartAvailableInMakkenzi", isFullCartAvailableInMakkenzi },
                                { "isPartCartAvailableInMakkenzi", isPartCartAvailableInMakkenzi },
                                { "isFullCartAvailableInTechnoPark", isFullCartAvailableInTechnoPark },
                                { "isFullCartAvailableInProstor", isFullCartAvailableInProstor },
                                { "isPartCartAvailableInTechnoPark", isPartCartAvailableInTechnoPark },
                                { "isPartCartAvailableInProstor", isPartCartAvailableInProstor },
                                { "isFullCartAvailableInStocks", isFullCartAvailableInStocks },
                                { "isFullCartAvailableInDirect", isFullCartAvailableInDirect },
                                { "isFullCartAvailableInAscKc", isFullCartAvailableInAscKc },
                                { "direct", direct },
                                { "ascKc", ascKc },
                                { "hasPersonality", hasPersonality },
                                { "clientCityId", clientCityId }
                            },
                            availableProducts = availableProducts,
                            availableProductsOnAnotherStock = availableProductsOnAnotherStock,
                            notAvailableProducts = notAvailableProducts,
                            notAvailableStocksProducts = notAvailableStocksProducts,
                            productInfos = GetProductInfos(balances),
                            availableProductsInClientCityStock = availableProductsInClientCityStock,
                            availableProductsInNotClientCityStock = availableProductsInNotClientCityStock,
                            notAvailableProductsInClientCityStock = notAvailableProductsInClientCityStock,
                            notAvailableProductsInNotClientCityStock = notAvailableProductsInNotClientCityStock
                        };

                        SendMessage(message);
                    }
                }
                finally
                {
                    //orderEntity.SetColumnValue("TrcIsProcessing", false);
                    //orderEntity.Save();
                }
            }
        }

        public Dictionary<Guid, ProductInfo> GetProductInfos(List<ProductStockBalance> balances)
        {
            var res = new Dictionary<Guid, ProductInfo>();

            var productsDistinct = balances.Select(e => e.ProductId).Distinct();

            foreach (var productId in productsDistinct)
            {
                res.Add(productId, new ProductInfo()
                {
                    requestedQuantity = balances.First(e => e.ProductId == productId).RequestedQuantity,
                    stockBalances = balances.Where(e => e.ProductId == productId).ToDictionary(e => e.WarehouseId, e => e.AvailableQuantity)
                });
            }

            return res;
        }

        public void SendMessage(MessageParams message)
        {
            //Terrasoft.Configuration.MsgChannelUtilities.PostMessage(UserConnection, "TrcPreOrderHelper", JsonConvert.SerializeObject(message));
        }

        public Guid GetAddressCity(Guid clientId, string address)
        {
            var select = new Select(UserConnection)
                    .Column("CityId")
                    .From("VwClientAddress")
                    .Where("ClientId").IsEqual(Column.Parameter(clientId))
                    .And("Address").IsEqual(Column.Parameter(address)) as Select;

            return select.ExecuteScalar<Guid>();
        }

        /// <summary>
        /// Проверяет являются ли все текущие товары запчастями
        /// </summary>
        /// <param name="balances">Данные по продуктам</param>
        /// <returns>Признак того, что все товары относятся к запчастям</returns>
        public bool AreAllProductsSpareParts(List<ProductStockBalance> balances)
        {
            return balances.Select(e => e.ProductCategoryId).Distinct().Count(e => e != SparePartsCategoryId) < 1;
        }

        /// <summary>
        /// Проверяет есть ли среди текущих товаров запчасти
        /// </summary>
        /// <param name="balances">Данные по продуктам</param>
        /// <returns>Признак того, что есть товары - запчасти</returns>
        public bool HasAnyProductSparePart(List<ProductStockBalance> balances)
        {
            return balances.Select(e => e.ProductCategoryId).Distinct().Any(e => e == SparePartsCategoryId);
        }

        /// <summary> 
        /// Проверка доступности полной корзины продуктов на конкретном складе
        /// </summary>
        /// <param name="balances">Данные по продуктам</param>
        /// <param name="warehouseId">Идентификатор склада</param>
        /// <returns>Признак доступности</returns>
        public bool IsFullCartAvailableInStock(List<ProductStockBalance> balances, Guid warehouseId)
        {
            var products = balances.Select(e => e.ProductId).Distinct().ToList();

            var onWarehouse = balances.Where(e => e.WarehouseId == warehouseId).ToList();

            //return onWarehouse.Count >= 1 && onWarehouse.Count(e => e.GetTypedColumnValue<double>("AvailableQuantity") < 1) < 1;

            return onWarehouse.Count >= products.Count && onWarehouse.Count(e => e.RequestedQuantity > e.AvailableQuantity) < 1;
        }

        /// <summary>
        /// Проверка доступности части корзины продуктов на конкретном складе
        /// </summary>
        /// <param name="balances">Данные по продуктам</param>
        /// <param name="warehouseId">Идентификатор склада</param>
        /// <returns>Признак доступности</returns>
        public bool IsPartCartAvailableInStock(List<ProductStockBalance> balances, Guid warehouseId, List<Guid> listOnly = null)
        {
        	if(listOnly == null)
        	{
        		return balances.Where(e => e.WarehouseId == warehouseId)
                            .Any(e => e.AvailableQuantity > 0);
        	}
        	else
        	{
        		return balances.Where(e => e.WarehouseId == warehouseId && listOnly.Contains(e.ProductId))
                            .Any(e => e.AvailableQuantity > 0);
        	}
            
        }

        /// <summary>
        /// Определяет доступна ли корзина суммарно на нескольких складах
        /// </summary>
        /// <param name="stocks"></param>
        /// <returns></returns>
        public bool IsFullCartAvailableInStocks(List<ProductStockBalance> balances, params Guid[] stocks)
        {
            var onWarehouses = balances.Where(e => stocks.Contains(e.WarehouseId)).ToList();

            var data = new Dictionary<Guid, double>();

            foreach (var item in onWarehouses)
            {
                var productId = item.ProductId;

                if (data.ContainsKey(productId))
                {
                    data[productId] += item.AvailableQuantity;
                }
                else
                {
                    data.Add(productId, item.AvailableQuantity);
                }
            }

            return onWarehouses.Count > 0 && onWarehouses.All(e => data.ContainsKey(e.ProductId) && e.RequestedQuantity <= data[e.ProductId]);
        }

        /// <summary>
        /// Формирует строку с описанием наличия продуктов на конкретном складе
        /// </summary>
        /// <param name="balances">Данные по продуктам</param>
        /// <returns></returns>
        public string GetProductsString(List<ProductStockBalance> products, List<ProductStockBalance> notAvailableProducts, bool addWarehouse)
        {
            var res = "";

            var shownProducts = new List<string>();

            foreach (var item in products)
            {
                var productName = item.ProductName;

                shownProducts.Add(productName);

                res += string.Format("Продукт {0} в наличии в количестве {1}", productName, item.AvailableQuantity);
                if (addWarehouse) res += string.Format(" на складе {0} ", item.WarehouseName);
                res += "\n";
            }

            if (notAvailableProducts != null && notAvailableProducts.Count > 0)
            {
                res += "\n";

                foreach (var item in notAvailableProducts)
                {
                    var productName = item.ProductName;

                    if (shownProducts.Contains(productName))
                    {
                        continue;
                    }
                    else
                    {
                        shownProducts.Add(productName);
                    }

                    res += string.Format("Продукта {0} нет в наличии\n", productName);
                    res += "\n";
                }
            }

            return res;
        }

        /// <summary>
        /// Читает остатки на складах для продуктов заказа
        /// </summary>
        /// <returns>Данные по продуктам</returns>        
        public List<ProductStockBalance> GetProductStockBalances(Guid orderId, UserConnection UserConnection)
        {
            var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "OrderProduct");

            esq.AddColumn("Product.TrcPersonalization");
            esq.AddColumn("Product.Category.Id");
            esq.AddColumn("Product.Name");
            (esq.AddColumn("[ProductStockBalance:Product:Product].Warehouse.Id")).Name = "WarehouseId";
            (esq.AddColumn("[ProductStockBalance:Product:Product].Warehouse.Name")).Name = "WarehouseName";
            (esq.AddColumn("[ProductStockBalance:Product:Product].AvailableQuantity")).Name = "AvailableQuantity";
            (esq.AddColumn("[ProductStockBalance:Product:Product].ReserveQuantity")).Name = "ReserveQuantity";
            (esq.AddColumn("[ProductStockBalance:Product:Product].TotalQuantity")).Name = "TotalQuantity";

            esq.AddAllSchemaColumns();

            esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Order", orderId));

            var res = esq.GetEntityCollection(UserConnection).Select(e =>
                new ProductStockBalance()
                {
                    AvailableQuantity = e.GetTypedColumnValue<double>("AvailableQuantity"),
                    Id = e.GetTypedColumnValue<Guid>("Id"),
                    ProductCategoryId = e.GetTypedColumnValue<Guid>("Product_Category_Id"),
                    ProductId = e.GetTypedColumnValue<Guid>("ProductId"),
                    ProductName = e.GetTypedColumnValue<string>("Product_Name"),
                    RequestedQuantity = e.GetTypedColumnValue<double>("Quantity"),
                    ReserveQuantity = e.GetTypedColumnValue<double>("ReserveQuantity"),
                    TotalQuantity = e.GetTypedColumnValue<double>("TotalQuantity"),
                    TrcPersonalization = e.GetTypedColumnValue<bool>("Product_TrcPersonalization"),
                    WarehouseId = e.GetTypedColumnValue<Guid>("WarehouseId"),
                    WarehouseName = e.GetTypedColumnValue<string>("WarehouseName")
                }
            ).ToList();

            var emptyStocks = new List<ProductStockBalance>();

            foreach (var stock in MainStocks)
            {
                foreach (var balance in res)
                {
                    if(!res.Exists(e => e.WarehouseId == stock && e.ProductId == balance.ProductId) 
                        && !emptyStocks.Exists(e => e.WarehouseId == stock && e.ProductId == balance.ProductId))
                    {
                        emptyStocks.Add(new ProductStockBalance()
                        {
                            AvailableQuantity = 0,
                            ProductCategoryId = balance.ProductCategoryId,
                            ProductId = balance.ProductId,
                            ProductName = balance.ProductName,
                            RequestedQuantity = balance.RequestedQuantity,
                            ReserveQuantity = 0,
                            TotalQuantity = 0,
                            TrcPersonalization = balance.TrcPersonalization,
                            WarehouseId = stock
                        });
                    }
                }
            }

            foreach (var item in emptyStocks)
            {
                res.Add(item);
            }

            return res;
        }

        /// <summary>
        /// Получаем розничные магазины с типом Direct в заданном городе
        /// </summary>
        /// <param name="cityId">Город</param>
        /// <returns>Список точек</returns>
        public List<Entity> GetDirectRetailStoresByCity(Guid cityId)
        {
            var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "TrcRetailStores");

            esq.AddColumn("Id");

            esq.AddAllSchemaColumns();

            esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcCity", cityId));
            esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcStorType", new Guid("b3eed351-7ab1-441e-aa22-ea918ce3ea62")));

            var res = esq.GetEntityCollection(UserConnection).ToList();

            return res;
        }

        /// <summary>
        /// Получаем АСЦ и КЦ в заданном городе
        /// </summary>
        /// <param name="cityId">Город</param>
        /// <returns>Список точек</returns>
        public List<Entity> GetAscKcByCity(Guid cityId)
        {
            var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "TrcAscKc");

            esq.AddColumn("Id");

            esq.AddAllSchemaColumns();

            esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcCity", cityId));

            var res = esq.GetEntityCollection(UserConnection).ToList();

            return res;
        }
    }
}