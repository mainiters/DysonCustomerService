using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.DB;
using Terrasoft.Core.Entities;

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
                    "TrcSerialNumber.TrcSerialNumber.Name"
                }
            });

            esq.AddColumn("TrcOrderState.Description");
            esq.AddColumn("TrcOrcerPaymentWay.Name");
            esq.AddColumn("TrcDeliveryCompany.TrcCode");
            esq.AddColumn("Owner.Trc1CContactID");
            esq.AddColumn("TrcOrganization.Name");
            esq.AddColumn("TrcWarehouseForShippingOrder.TrcCode");

            esq.AddColumn("Account.Trc1CAccountID");
            esq.AddColumn("Contact.Trc1CContactID");

            esq.AddColumn("TrcASCAndKC.TrcCode");
            esq.AddColumn("TrcOrganization.Trc1CAccountID");
            esq.AddColumn("TrcStatusTK.Description");

            base.AddRelatedColumns(esq, relatedEntitiesData);
        }

        public override object GetEntityData(Guid EntityId)
        {
            string AccountId = this.EntityObject.GetTypedColumnValue<string>("Account_Trc1CAccountID");
            string ContactId = this.EntityObject.GetTypedColumnValue<string>("Contact_Trc1CContactID");

            string AscAndKcCode = this.EntityObject.GetTypedColumnValue<string>("TrcASCAndKC_TrcCode");
            string OrganizationCode = this.EntityObject.GetTypedColumnValue<string>("TrcOrganization_Trc1CAccountID");

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
                Account = string.IsNullOrEmpty(AccountId) ? ContactId : AccountId,
                ID_1С = this.EntityObject.GetTypedColumnValue<string>("TrcOrderCode1C"),
                CreateDate = this.EntityObject.GetTypedColumnValue<DateTime>("CreatedOn"),
                TimeDeliveryFrom = this.EntityObject.GetTypedColumnValue<int>("TrcDeliveryFromTime"),
                TimeDeliveryTo = this.EntityObject.GetTypedColumnValue<int>("TrcDeliveryToTime"),
                TK_Plandate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcDeliveryDate"),
                completeDate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcActualDeliveryDate"),
                modifyDate = this.EntityObject.GetTypedColumnValue<DateTime>("ModifiedOn"),
                orderID = string.Empty,
                orderIdPublic = int.Parse(new string(this.EntityObject.GetTypedColumnValue<string>("Number").Where(c => char.IsDigit(c)).ToArray())),
                OrderStatus = this.EntityObject.GetTypedColumnValue<string>("TrcOrderState_Description"),
                TK_Track = this.EntityObject.GetTypedColumnValue<string>("TrcTrackNumber"),
                TK = this.EntityObject.GetTypedColumnValue<string>("TrcDeliveryCompany_TrcCode"),
                PayType = this.EntityObject.GetTypedColumnValue<string>("TrcOrcerPaymentWay_Name"),
                LogisticComment = this.EntityObject.GetTypedColumnValue<string>("TrcTransportDepartmentComment"),
                OrderSumRUB = this.EntityObject.GetTypedColumnValue<decimal>("Amount"),
                PayTransaction = this.EntityObject.GetTypedColumnValue<string>("TrcTransactionCodePayU"),
                DataReleaseH = this.EntityObject.GetTypedColumnValue<DateTime>("TrcDateHoldingClientsFunds"),
                DataReleaseM = this.EntityObject.GetTypedColumnValue<DateTime>("TrcActualWriteOffClientsFunds"),
                PayTransactionODM = this.EntityObject.GetTypedColumnValue<string>("TrcTransactionsCodeOrangeData"),
                DataCheckM = this.EntityObject.GetTypedColumnValue<DateTime>("TrcCheckBreakDateOrangeData"),
                //PayDate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcPaymentDate"),
                DeliveryCost = this.EntityObject.GetTypedColumnValue<decimal>("TrcDeliveryCost"),
                Comment = this.EntityObject.GetTypedColumnValue<string>("TrcCommentFrom1C"),
                Manager = this.EntityObject.GetTypedColumnValue<string>("Owner_Trc1CContactID"),
                ManagerPhone = this.EntityObject.GetTypedColumnValue<string>("ContactNumber"),
                email = this.EntityObject.GetTypedColumnValue<string>("TrcClientEmail"),
                CourierInfo = this.EntityObject.GetTypedColumnValue<string>("Comment"),
                ShipDate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcShipmentDate"),
                Organization = string.IsNullOrEmpty(AscAndKcCode) ? OrganizationCode : AscAndKcCode,
                WarehouseCode = this.EntityObject.GetTypedColumnValue<string>("TrcWarehouseForShippingOrder_TrcCode"),
                CommentTK = this.EntityObject.GetTypedColumnValue<string>("TrcCourierServiceComment"),
                MarkDeletion = this.EntityObject.GetTypedColumnValue<bool>("TrcMarkDeletion"),
                StatusTK = this.EntityObject.GetTypedColumnValue<string>("TrcStatusTK_Description"),

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
                kladr = addresEntity == null ? string.Empty : addresEntity.GetTypedColumnValue<string>("TrcKladrCode"),
                Region = addresEntity == null ? string.Empty : addresEntity.GetTypedColumnValue<string>("Region_Name"),
                Street = addresEntity == null ? string.Empty : addresEntity.GetTypedColumnValue<string>("TrcStreet"),
                Locality = addresEntity == null ? string.Empty : addresEntity.GetTypedColumnValue<string>("TrcSettlement_Name"),

                CreditStatus = string.Empty,
                CreditSumm = 0,
                paper_id = string.Empty,
                InCash = 0,
                OnLinePaid =0,
                PanType = string.Empty,
                DistanceFromMKAD = string.Empty,

                DysonChannelCode = this.RelatedEntitiesData.Count > 0 && this.RelatedEntitiesData.Where(e => e.Name == "OrderProduct").Count() > 0 ? 
                                    this.RelatedEntitiesData.Where(e => e.Name == "OrderProduct").First().EntityCollection.First().GetTypedColumnValue<string>("TrcSalesSource_TrcCode") :
                                    string.Empty,
                
                NumberDeparture = string.Empty,
                OrderType = 0,

                PromoSum = DiscountAmount,
                
                SOCR_Area = string.Empty,
                SOCR_City = string.Empty,
                SOCR_Locality = string.Empty,
                SOCR_Region = string.Empty,
                SOCR_Street = string.Empty
            };

            // Деталь продуктов
            var orderProducts = new List<ЗаказКлиентаTovars>();

            if (this.RelatedEntitiesData.Count > 0)
            {
                foreach (var item in this.RelatedEntitiesData.Where(e => e.Name == "OrderProduct").First().EntityCollection)
                {
                    orderProducts.Add(new ЗаказКлиентаTovars()
                    {
                        TovarCod = item.GetTypedColumnValue<string>("Product_Trc1CProductID"),
                        Kol = item.GetTypedColumnValue<decimal>("Quantity"),
                        TovarSum = item.GetTypedColumnValue<decimal>("TotalAmount"),
                        RRC = item.GetTypedColumnValue<decimal>("Price"),
                        ProductsCanceled = item.GetTypedColumnValue<bool>("TrcProductsCanceled"),
                        ReasonCancellation = item.GetTypedColumnValue<string>("TrcReasonCancellation"),
                        PROMOCODE = item.GetTypedColumnValue<string>("TrcPromocode"),
                        NDS = this.EntityObject.GetTypedColumnValue<decimal>("TrcVAT"),
                        NDS_S = this.EntityObject.GetTypedColumnValue<int>("TrcVATrate"),
                        SN = item.GetTypedColumnValue<string>("TrcSerialNumber_TrcSerialNumber_Name")
                    });
                }
            }

            res.Tovars = orderProducts.ToArray();

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
    }
}
