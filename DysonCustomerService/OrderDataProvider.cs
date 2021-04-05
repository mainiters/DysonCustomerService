using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace DysonCustomerService
{
    public class OrderDataProvider : BaseEntityDataProvider
    {
        public OrderDataProvider(Guid OrderId, UserConnection UserConnection)
            : base("Order", OrderId, UserConnection)
        {

        }

        protected override void AddRelatedColumns(EntitySchemaQuery esq)
        {
            base.AddRelatedColumns(esq);

            esq.AddColumn("TrcOrderState.Name");
            esq.AddColumn("TrcOrcerPaymentWay.Name");
            esq.AddColumn("TrcDeliveryCompany.Name");
            esq.AddColumn("Owner.Name");
        }

        public override object GetEntityData(Guid EntityId)
        {
            var res = new ПакетЗаказов();

            res.ID_Pack = new Guid().ToString();

            res.Order = new ЗаказКлиента[]
            {
                new ЗаказКлиента()
                {
                    CreateDate = this.EntityObject.GetTypedColumnValue<DateTime>("CreatedOn"),
                    deliveryDate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcDesiredDeliveryDate"),
                    TimeDeliveryFrom = this.EntityObject.GetTypedColumnValue<DateTime>("TrcDeliveryFromTime").Hour,
                    TimeDeliveryTo = this.EntityObject.GetTypedColumnValue<DateTime>("TrcDeliveryToTime").Hour,
                    TK_Plandate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcDeliveryDate"),
                    completeDate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcActualDeliveryDate"),
                    modifyDate = this.EntityObject.GetTypedColumnValue<DateTime>("ModifiedOn"),
                    orderID = this.EntityObject.GetTypedColumnValue<string>("Id"),
                    orderIdPublic = 0,  //this.EntityObject.GetTypedColumnValue<int>("TrcNumberForClient"),
                    OrderStatus = this.EntityObject.GetTypedColumnValue<string>("TrcOrderState_Name"),
                    TK_Track = this.EntityObject.GetTypedColumnValue<string>("TrcTrackNumber"),
                    TK = this.EntityObject.GetTypedColumnValue<string>("TrcDeliveryCompany_Name"),
                    PayType = this.EntityObject.GetTypedColumnValue<string>("TrcOrcerPaymentWay_Name"),
                    LogisticComment = this.EntityObject.GetTypedColumnValue<string>("TrcTransportDepartmentComment"),
                    OrderSumRUB = this.EntityObject.GetTypedColumnValue<decimal>("Amount"),
                    PayTransaction = 0, //this.EntityObject.GetTypedColumnValue<string>("TrcTransactionCodePayU"),
                    DataReleaseH = this.EntityObject.GetTypedColumnValue<DateTime>("TrcDateHoldingClientsFunds"),
                    DataReleaseM = this.EntityObject.GetTypedColumnValue<DateTime>("TrcActualWriteOffClientsFunds"),
                    PayTransactionODM = this.EntityObject.GetTypedColumnValue<string>("TrcTransactionsCodeOrangeData"),
                    DataCheckM = this.EntityObject.GetTypedColumnValue<DateTime>("TrcCheckBreakDateOrangeData"),
                    PayDate = this.EntityObject.GetTypedColumnValue<DateTime>("PaymentDate"),
                    DeliveryCost = this.EntityObject.GetTypedColumnValue<decimal>("TrcDeliveryCost"),
                    Comment = this.EntityObject.GetTypedColumnValue<string>("TrcCommentFrom1C"),
                    Manager = this.EntityObject.GetTypedColumnValue<string>("Owner_Name"),
                    PhoneNumber = this.EntityObject.GetTypedColumnValue<string>("ContactNumber"),
                    email = this.EntityObject.GetTypedColumnValue<string>("TrcClientEmail"),
                    CourierInfo = this.EntityObject.GetTypedColumnValue<string>("Comment"),
                    ShipDate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcShipmentDate")
                    // Деталь продуктов
                }
            };

            return res;
        }
    }
}
