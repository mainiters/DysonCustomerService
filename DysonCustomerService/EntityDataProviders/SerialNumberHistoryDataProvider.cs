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
            esq.AddColumn("TrcProduct.Name");
            esq.AddColumn("TrcWarrantyType.Name");
            esq.AddColumn("TrcRetailStore.Name");
            
            base.AddRelatedColumns(esq, relatedEntitiesData);
        }

        public override object GetEntityData(Guid EntityId)
        {
            // Данные История серийных номеров
            var res = new РегистрацияСерийныхНомеровКлиентов
            {
                SN = this.EntityObject.GetTypedColumnValue<string>("TrcSerialNumber_Name"),
                CreateDate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcRegistrationDate"),
                Article = this.EntityObject.GetTypedColumnValue<string>("TrcProduct_Name"),
                TypeGuarantee = this.EntityObject.GetTypedColumnValue<string>("TrcWarrantyType_Name"),
                Shop = this.EntityObject.GetTypedColumnValue<string>("TrcRetailStore_Name"),
                DatePurchase = this.EntityObject.GetTypedColumnValue<DateTime>("TrcPurchaseDate"),
                Comment = this.EntityObject.GetTypedColumnValue<string>("TrcComment")
            };

            return res;
        }
    }
}
