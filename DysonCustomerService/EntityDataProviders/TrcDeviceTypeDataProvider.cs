using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace DysonCustomerService.EntityDataProviders
{
    public class TrcDeviceTypeDataProvider : BaseEntityDataProvider
    {
        public TrcDeviceTypeDataProvider(Guid Id, UserConnection UserConnection)
            : base("TrcDeviceType", Id, UserConnection, "TrcCode")
        {

        }

        protected override void AddRelatedColumns(EntitySchemaQuery esq, List<RelatedEntitiesData> relatedEntitiesData)
        {
            esq.AddColumn("TrcProductSubcategory.TrcCode");

            base.AddRelatedColumns(esq, relatedEntitiesData);
        }

        public override object GetEntityData(Guid EntityId)
        {
            var res = new ТипыУстройств()
            {
                Name = this.EntityObject.GetTypedColumnValue<string>("Name"),
                MarkDeletion = this.EntityObject.GetTypedColumnValue<bool>("TrcMarkDeletion"),
                ID_1С = this.EntityObject.GetTypedColumnValue<string>("TrcCode"),
                EquipmentCode = this.EntityObject.GetTypedColumnValue<string>("TrcProductSubcategory_TrcCode")
            };

            return res;
        }
    }
}
