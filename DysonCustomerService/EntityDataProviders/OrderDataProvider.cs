using System;
using System.Collections.Generic;
using System.Linq;
using Terrasoft.Core;
using Terrasoft.Core.Entities;
using Terrasoft.Trace;

namespace DysonCustomerService.EntityDataProviders
{
    public class OrderDataProvider : BaseEntityDataProvider
    {
        public OrderDataProvider(Guid Id, UserConnection UserConnection)
            : base("Order", Id, UserConnection, "TrcOrderCode1C")
        {

        }

        protected override void AddRelatedColumns(EntitySchemaQuery esq, List<RelatedEntitiesData> relatedEntitiesData)
        {
            relatedEntitiesData.Add(new RelatedEntitiesData()
            {
                Name = "OrderProduct",
                AdditionalColumns = new List<string>()
                {
                    "Product.Trc1CProductID",
                    "PriceList.TrcCode",
                    "TrcSalesSource.TrcCode",
                    "TrcSerialNumber.TrcSerialNumber.Name",
                    "DsnBundle.Trc1CProductID",
                    "TrcFoilColor.TrcCode"
                }
            });

            esq.AddColumn("TrcOrderState.Id");
            esq.AddColumn("TrcOrderState.Description");
            esq.AddColumn("TrcOrcerPaymentWay.Description");
            esq.AddColumn("TrcDeliveryCompany.TrcCode");
            esq.AddColumn("Owner.Trc1CContactID");
            esq.AddColumn("TrcOrganization.Name");
            esq.AddColumn("TrcWarehouseForShippingOrder.TrcCode");

            esq.AddColumn("Account.Trc1CAccountID");
            esq.AddColumn("Contact.Trc1CContactID");

            esq.AddColumn("TrcASCAndKC.TrcCode");
            esq.AddColumn("TrcOrganization.Trc1CAccountID");
            esq.AddColumn("TrcStatusTK.Description");
            esq.AddColumn("SourceOrder.Description");
            esq.AddColumn("TrcDysonChannelCode.TrcCode");
            esq.AddColumn("TrcReasonCancelingOrder.TrcCode");

            base.AddRelatedColumns(esq, relatedEntitiesData);
        }

        public override object GetEntityData(Guid EntityId)
        {
            string AccountId = this.EntityObject.GetTypedColumnValue<string>("Account_Trc1CAccountID");
            string ContactId = this.EntityObject.GetTypedColumnValue<string>("Contact_Trc1CContactID");

            var clientEntityName = string.IsNullOrEmpty(AccountId) ? "Contact" : "Account";

            var addresEntity = this.GetAddressData(clientEntityName, this.EntityObject.GetTypedColumnValue<Guid>(clientEntityName + "Id"), this.EntityObject.GetTypedColumnValue<string>("DeliveryAddress"));

            decimal DiscountAmount = 0;

            if (this.RelatedEntitiesData.Count > 0)
            {
                foreach (var item in this.RelatedEntitiesData.Where(e => e.Name == "OrderProduct").First().EntityCollection)
                {
                    DiscountAmount += item.GetTypedColumnValue<decimal>("DiscountAmount");
                }
            }

            // Данные заказа
            var res = new ЗаказКлиента()
            {
                //PayDate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcPaymentDate"),

                ID_1С = this.EntityObject.GetTypedColumnValue<string>("TrcOrderCode1C"),
                createDate = this.EntityObject.GetTypedColumnValue<DateTime>("CreatedOn"),
                Date_Tk_Load = this.EntityObject.GetTypedColumnValue<DateTime>("TrcDateTKLoad"),
                deliveryDate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcDesiredDeliveryDate"),
                TimeDeliveryFrom = this.EntityObject.GetTypedColumnValue<int>("TrcDeliveryFromTime"),
                TimeDeliveryTo = this.EntityObject.GetTypedColumnValue<int>("TrcDeliveryToTime"),
                TK_Plandate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcDeliveryDate"),
                completeDate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcActualDeliveryDate"),
                orderIdPublic = int.Parse(new string(this.EntityObject.GetTypedColumnValue<string>("Number").Where(c => char.IsDigit(c)).ToArray())),
                orderID = this.EntityObject.GetTypedColumnValue<string>("TrcIOrderID"),
                OrderStatus = this.EntityObject.GetTypedColumnValue<string>("TrcOrderState_Id").ToUpper() != "32B5554A-8975-475D-BAA0-E8B47F1B9973" || true
                    ? this.EntityObject.GetTypedColumnValue<string>("TrcOrderState_Description")
                    : "ОтмененКлиентом",
                TK_Track = this.EntityObject.GetTypedColumnValue<string>("TrcTrackNumber"),
                TK = this.EntityObject.GetTypedColumnValue<string>("TrcDeliveryCompany_TrcCode"),
                Payment = this.EntityObject.GetTypedColumnValue<string>("TrcOrcerPaymentWay_Description"),
                LogisticComment = this.EntityObject.GetTypedColumnValue<string>("TrcTransportDepartmentComment"),
                PayTransaction = this.EntityObject.GetTypedColumnValue<string>("TrcTransactionCodePayU"),
                DataReleaseH = this.EntityObject.GetTypedColumnValue<DateTime>("TrcDateHoldingClientsFunds"),
                DataReleaseM = this.EntityObject.GetTypedColumnValue<DateTime>("TrcActualWriteOffClientsFunds"),
                PayTransactionODM = this.EntityObject.GetTypedColumnValue<string>("TrcTransactionsCodeOrangeData"),
                DataCheckM = this.EntityObject.GetTypedColumnValue<DateTime>("TrcCheckBreakDateOrangeData"),
                PayTransacrionOD = this.EntityObject.GetTypedColumnValue<string>("TrcPayTransacrionOD"),
                DataPayChekOD = this.EntityObject.GetTypedColumnValue<DateTime>("TrcDataPayChekOD"),
                RetTrasactionOD = this.EntityObject.GetTypedColumnValue<string>("TrcRetTrasactionOD"),
                DataRetCheckOD = this.EntityObject.GetTypedColumnValue<DateTime>("TrcDataRetCheckOD"),
                NewPrPay = this.EntityObject.GetTypedColumnValue<bool>("TrcNewPrPay"),
                DeliveryCost = this.EntityObject.GetTypedColumnValue<decimal>("TrcDeliveryCost"),
                Comment = this.EntityObject.GetTypedColumnValue<string>("TrcCommentFrom1C"),
                Manager = this.EntityObject.GetTypedColumnValue<string>("Owner_Trc1CContactID"),
                Account = string.IsNullOrEmpty(AccountId) ? ContactId : AccountId,
                PhoneNumber = this.EntityObject.GetTypedColumnValue<string>("ContactNumber"),
                email = this.EntityObject.GetTypedColumnValue<string>("TrcClientEmail"),
                CourierInfo = this.EntityObject.GetTypedColumnValue<string>("Comment"),
                ShipDate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcShipmentDate"),
                DysonChannelCode = this.EntityObject.GetTypedColumnValue<string>("TrcDysonChannelCode_TrcCode"),
                WarehouseCode = this.EntityObject.GetTypedColumnValue<string>("TrcWarehouseForShippingOrder_TrcCode"),
                Organization = this.EntityObject.GetTypedColumnValue<string>("TrcASCAndKC_TrcCode"),
                MarkDeletion = this.EntityObject.GetTypedColumnValue<bool>("TrcMarkDeletion"),
                SalesChannel = this.EntityObject.GetTypedColumnValue<string>("SourceOrder_Description"),
                CreateSystem = this.EntityObject.GetTypedColumnValue<string>("TrcCreateSystem"),

                FIAS = addresEntity == null ? string.Empty : addresEntity.GetTypedColumnValue<string>("TrcFiasCode"),
                Metro = addresEntity == null ? string.Empty : addresEntity.GetTypedColumnValue<string>("TrcMetroStation"),
                House = addresEntity == null ? string.Empty : addresEntity.GetTypedColumnValue<string>("TrcHouse"),
                Building = addresEntity == null ? string.Empty : addresEntity.GetTypedColumnValue<string>("TrcBuilding"),
                Korp = addresEntity == null ? string.Empty : addresEntity.GetTypedColumnValue<string>("TrcHousing"),
                Flat = addresEntity == null ? string.Empty : addresEntity.GetTypedColumnValue<string>("TrcApartment"),
                Entrance = addresEntity == null ? string.Empty : addresEntity.GetTypedColumnValue<string>("TrcEntrance"),
                FLOOR = addresEntity == null ? string.Empty : addresEntity.GetTypedColumnValue<string>("TrcFloor"),
                Intercom = addresEntity == null ? string.Empty : addresEntity.GetTypedColumnValue<string>("TrcIntercom"),
                City = addresEntity == null ? string.Empty : addresEntity.GetTypedColumnValue<string>("City_Name"),
                Area = addresEntity == null ? string.Empty : addresEntity.GetTypedColumnValue<string>("TrcArea_Name"),
                Lat = addresEntity == null ? string.Empty : addresEntity.GetTypedColumnValue<string>(string.IsNullOrEmpty(AccountId) ? "TrcGPSE" : "GPSE"),
                Lon = addresEntity == null ? string.Empty : addresEntity.GetTypedColumnValue<string>(string.IsNullOrEmpty(AccountId) ? "TrcGPSN" : "GPSN"),
                Region = addresEntity == null ? string.Empty : addresEntity.GetTypedColumnValue<string>("Region_Name"),
                Street = addresEntity == null ? string.Empty : addresEntity.GetTypedColumnValue<string>("TrcStreet"),
                Locality = addresEntity == null ? string.Empty : addresEntity.GetTypedColumnValue<string>("TrcSettlement_Name"),
                DistanceFromMKAD = addresEntity == null ? string.Empty : addresEntity.GetTypedColumnValue<string>("TrcDistanceFromMKAD"),
                Kladr = addresEntity == null ? string.Empty : addresEntity.GetTypedColumnValue<string>("TrcKladrCode"),
                DeliveryAddress = this.EntityObject.GetTypedColumnValue<string>("DeliveryAddress")
            };

            // Деталь продуктов
            var orderProducts = new List<ЗаказКлиентаProduct>();

            if (this.RelatedEntitiesData.Count > 0)
            {
                foreach (var item in this.RelatedEntitiesData.Where(e => e.Name == "OrderProduct").First().EntityCollection)
                {
                    orderProducts.Add(new ЗаказКлиентаProduct()
                    {
                        TovarCod = item.GetTypedColumnValue<string>("Product_Trc1CProductID"),
                        Kol = item.GetTypedColumnValue<decimal>("Quantity"),
                        TovarSum = item.GetTypedColumnValue<decimal>("TotalAmount"),
                        RRC = item.GetTypedColumnValue<decimal>("Price"),
                        SN = item.GetTypedColumnValue<string>("TrcSerialNumber_TrcSerialNumber_Name"),
                        NDS_S = this.EntityObject.GetTypedColumnValue<int>("TrcVATrate"),
                        NDS = this.EntityObject.GetTypedColumnValue<decimal>("TrcVAT"),
                        ProductsCanceled = item.GetTypedColumnValue<bool>("TrcProductsCanceled"),
                        ReasonCancellation = this.EntityObject.GetTypedColumnValue<string>("TrcReasonCancelingOrder_TrcCode"),
                        //ReasonCancellation = item.GetTypedColumnValue<string>("TrcReasonCancellation"),
                        Personalization = item.GetTypedColumnValue<bool>("TrcPersonalization"),
                        FoilColor = item.GetTypedColumnValue<string>("TrcFoilColor_TrcCode"),
                        Initials = item.GetTypedColumnValue<string>("TrcInitials"),
                        Bundle = item.GetTypedColumnValue<string>("DsnBundle_Trc1CProductID"),
                        PROMOCODE = item.GetTypedColumnValue<string>("TrcPromocode")
                    });
                }
            }

            res.Product = orderProducts.ToArray();

            return res;
        }

        protected Entity GetAddressData(string mainSchemaName, Guid clientId, string address)
        {
            EntitySchema schema = this.UserConnection.EntitySchemaManager.GetInstanceByName(mainSchemaName + "Address");

            EntitySchemaQuery esq = new EntitySchemaQuery(schema)
            {
                UseAdminRights = true,
                CanReadUncommitedData = true,
                IgnoreDisplayValues = true
            };

            esq.AddAllSchemaColumns();

            esq.AddColumn("City.Name");
            esq.AddColumn("TrcArea.Name");
            esq.AddColumn("TrcSettlement.Name");
            esq.AddColumn("Region.Name");

            esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, mainSchemaName, clientId));
            esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Address", address));

            return esq.GetEntityCollection(this.UserConnection).FirstOrDefault();
        }

        protected override void UpdateEntityIdFromPackage(ПакетОтвета response)
        {
            base.UpdateEntityIdFromPackage(response);

            if(response != null && !string.IsNullOrWhiteSpace(response.ID_Pack) && !string.IsNullOrEmpty(this.ExternalSystemId) 
                && string.IsNullOrEmpty(this.EntityObject.GetTypedColumnValue<string>("TrcOrderCode1C")))
            {
                var helper = new TrcPreOrderHelper(UserConnection);
                helper.ReserveOrderCart(this.EntityObject.GetTypedColumnValue<string>("Id"), this.EntityObject.GetTypedColumnValue<string>("TrcWarehouseForShippingOrderId"), false);
            }
        }
    }
}
