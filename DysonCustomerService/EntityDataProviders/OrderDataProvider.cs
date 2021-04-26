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
            : base("Order", Id, UserConnection)
        {

        }

        protected override void AddRelatedColumns(EntitySchemaQuery esq, List<RelatedEntitiesData> relatedEntitiesData)
        {
            relatedEntitiesData.Add(new RelatedEntitiesData()
            {
                Name = "OrderProduct",
                AdditionalColumns = new List<string>()
                {
                    "Product.Code",
                    "PriceList.TrcCode"
                }
            });

            esq.AddColumn("TrcOrderState.Name");
            esq.AddColumn("TrcOrcerPaymentWay.Name");
            esq.AddColumn("TrcDeliveryCompany.Name");
            esq.AddColumn("Owner.Name");
            esq.AddColumn("TrcOrganization.Name");
            esq.AddColumn("TrcWarehouseForShippingOrder.Name");

            esq.AddColumn("Account.Trc1CAccountID");
            esq.AddColumn("Contact.Trc1CContactID");

            base.AddRelatedColumns(esq, relatedEntitiesData);
        }

        public override object GetEntityData(Guid EntityId)
        {
            var res = new ПакетЗаказов();

            res.ID_Pack = new Guid().ToString();

            string AccountId = this.EntityObject.GetTypedColumnValue<string>("Account_Trc1CAccountID");
            string ContactId = this.EntityObject.GetTypedColumnValue<string>("Contact_Trc1CContactID");

            var clientEntityName = string.IsNullOrEmpty(AccountId) ? "Contact" : "Account";

            var addresEntity = this.GetAddressData(clientEntityName, this.EntityObject.GetTypedColumnValue<Guid>(clientEntityName + "Id"), this.EntityObject.GetTypedColumnValue<string>("DeliveryAddress"));

            // Данные заказа
            res.Order = new ЗаказКлиента[]
            {
                new ЗаказКлиента()
                {
                    ID_Сlient = string.IsNullOrEmpty(AccountId) ? ContactId : AccountId,
                    CreateDate = this.EntityObject.GetTypedColumnValue<DateTime>("CreatedOn"),
                    deliveryDate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcDesiredDeliveryDate"),
                    TimeDeliveryFrom = this.EntityObject.GetTypedColumnValue<int>("TrcDeliveryFromTime"),
                    TimeDeliveryTo = this.EntityObject.GetTypedColumnValue<int>("TrcDeliveryToTime"),
                    TK_Plandate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcDeliveryDate"),
                    completeDate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcActualDeliveryDate"),
                    modifyDate = this.EntityObject.GetTypedColumnValue<DateTime>("ModifiedOn"),
                    orderID = this.EntityObject.GetTypedColumnValue<string>("TrcNumberForClient"),
                    orderIdPublic = int.Parse(new string(this.EntityObject.GetTypedColumnValue<string>("TrcNumberForClient").Where(c => char.IsDigit(c)).ToArray())),
                    OrderStatus = this.EntityObject.GetTypedColumnValue<string>("TrcOrderState_Name"),
                    TK_Track = this.EntityObject.GetTypedColumnValue<string>("TrcTrackNumber"),
                    TK = this.EntityObject.GetTypedColumnValue<string>("TrcDeliveryCompany_Name"),
                    PayType = this.EntityObject.GetTypedColumnValue<string>("TrcOrcerPaymentWay_Name"),
                    LogisticComment = this.EntityObject.GetTypedColumnValue<string>("TrcTransportDepartmentComment"),
                    OrderSumRUB = this.EntityObject.GetTypedColumnValue<decimal>("Amount"),
                    PayTransaction = this.EntityObject.GetTypedColumnValue<int>("TrcTransactionCodePayU"),
                    DataReleaseH = this.EntityObject.GetTypedColumnValue<DateTime>("TrcDateHoldingClientsFunds"),
                    DataReleaseM = this.EntityObject.GetTypedColumnValue<DateTime>("TrcActualWriteOffClientsFunds"),
                    PayTransactionODM = this.EntityObject.GetTypedColumnValue<string>("TrcTransactionsCodeOrangeData"),
                    DataCheckM = this.EntityObject.GetTypedColumnValue<DateTime>("TrcCheckBreakDateOrangeData"),
                    PayDate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcPaymentDate"),
                    DeliveryCost = this.EntityObject.GetTypedColumnValue<decimal>("TrcDeliveryCost"),
                    Comment = this.EntityObject.GetTypedColumnValue<string>("TrcCommentFrom1C"),
                    Manager = this.EntityObject.GetTypedColumnValue<string>("Owner_Name"),
                    ManagerPhone = this.EntityObject.GetTypedColumnValue<string>("ContactNumber"),
                    email = this.EntityObject.GetTypedColumnValue<string>("TrcClientEmail"),
                    CourierInfo = this.EntityObject.GetTypedColumnValue<string>("Comment"),
                    ShipDate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcShipmentDate"),
                    Organization = this.EntityObject.GetTypedColumnValue<string>("TrcOrganization_Name"),
                    WarehouseCode = this.EntityObject.GetTypedColumnValue<string>("TrcWarehouseForShippingOrder_Name"),
                    CommentTK = this.EntityObject.GetTypedColumnValue<string>("TrcCourierServiceComment"),
                    FIAS = this.EntityObject.GetTypedColumnValue<string>("DeliveryAddress"),
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
                    Lat = addresEntity == null ? string.Empty : addresEntity.GetTypedColumnValue<string>("GPSE"),
                    Lon = addresEntity == null ? string.Empty : addresEntity.GetTypedColumnValue<string>("GPSN")
                }
            };

            // Деталь продуктов
            if(this.RelatedEntitiesData.Count > 0)
            {
                var orderProducts = new List<ЗаказКлиентаTovars>();

                foreach (var item in this.RelatedEntitiesData.Where(e => e.Name == "OrderProduct").First().EntityCollection)
                {
                    orderProducts.Add(new ЗаказКлиентаTovars()
                    {
                        TovarCod = item.GetTypedColumnValue<string>("Product_Code"),
                        Kol = item.GetTypedColumnValue<decimal>("Quantity"),
                        TovarSum = item.GetTypedColumnValue<decimal>("TotalAmount"),
                        RRC = item.GetTypedColumnValue<decimal>("Price"),
                        ProductsCanceled = item.GetTypedColumnValue<bool>("TrcProductsCanceled"),
                        ReasonCancellation = item.GetTypedColumnValue<string>("TrcReasonCancellation"),
                        PROMOCODE = item.GetTypedColumnValue<string>("TrcPromocode"),
                        NDS = this.EntityObject.GetTypedColumnValue<decimal>("TrcVAT"),
                        NDS_S = this.EntityObject.GetTypedColumnValue<int>("TrcVATrate")
                    });
                }

                res.Order.First().Tovars = orderProducts.ToArray();
            }

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

            esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, mainSchemaName, clientId));
            esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Address", address));

            return esq.GetEntityCollection(this.UserConnection).FirstOrDefault();
        }
    }
}
