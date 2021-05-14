using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace DysonCustomerService.EntityDataProviders
{
    public class SerialNumberHistoryDataProvider : BaseEntityDataProvider
    {
        public SerialNumberHistoryDataProvider(Guid Id, UserConnection UserConnection)
            : base("TrcSerialNumberHistory", Id, UserConnection)
        {

        }

        protected override void AddRelatedColumns(EntitySchemaQuery esq, List<RelatedEntitiesData> relatedEntitiesData)
        {
            esq.AddColumn("TrcSerialNumber.Name");
            esq.AddColumn("TrcProduct.Code");
            esq.AddColumn("TrcWarrantyType.TrcCode");
            esq.AddColumn("TrcRetailStore.TrcCode");
            esq.AddColumn("TrcAccount.Trc1CAccountID");
            esq.AddColumn("TrcContact.Trc1CContactID");

            base.AddRelatedColumns(esq, relatedEntitiesData);
        }

        public override object GetEntityData(Guid EntityId)
        {
            string AccountId = this.EntityObject.GetTypedColumnValue<string>("TrcAccount_Trc1CAccountID");
            string ContactId = this.EntityObject.GetTypedColumnValue<string>("TrcContact_Trc1CContactID");

            // Данные История серийных номеров
            var res = new РегистрацияСерийныхНомеровКлиентов
            {
                ID_Сlient = string.IsNullOrEmpty(AccountId) ? ContactId : AccountId,

                SN = this.EntityObject.GetTypedColumnValue<string>("TrcSerialNumber_Name"),
                CreateDate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcRegistrationDate"),
                Article = this.EntityObject.GetTypedColumnValue<string>("TrcProduct_Code"),
                TypeGuarantee = this.EntityObject.GetTypedColumnValue<string>("TrcWarrantyType_TrcCode"),
                Shop = this.EntityObject.GetTypedColumnValue<string>("TrcRetailStore_TrcCode"),
                DatePurchase = this.EntityObject.GetTypedColumnValue<DateTime>("TrcPurchaseDate"),
                Comment = this.EntityObject.GetTypedColumnValue<string>("TrcComment"),
                MarkDeletion = this.EntityObject.GetTypedColumnValue<bool>("TrcMarkDeletion")
            };

            return res;
        }
    }
}
