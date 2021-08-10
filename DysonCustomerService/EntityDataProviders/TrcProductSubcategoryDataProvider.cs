using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace DysonCustomerService.EntityDataProviders
{
    public class TrcProductSubcategoryDataProvider : BaseEntityDataProvider
    {
        public TrcProductSubcategoryDataProvider(Guid Id, UserConnection UserConnection)
            : base("TrcProductSubcategory", Id, UserConnection, "TrcCode")
        {

        }

        protected override void AddRelatedColumns(EntitySchemaQuery esq, List<RelatedEntitiesData> relatedEntitiesData)
        {
            base.AddRelatedColumns(esq, relatedEntitiesData);
        }

        public override object GetEntityData(Guid EntityId)
        {
            var res = new КодОборудования()
            {
                Name = this.EntityObject.GetTypedColumnValue<string>("Name"),
                Description = this.EntityObject.GetTypedColumnValue<string>("Description"),
                MarkDeletion = this.EntityObject.GetTypedColumnValue<bool>("TrcMarkDeletion"),
                ID_1С = this.EntityObject.GetTypedColumnValue<string>("TrcCode"),
            };

            return res;
        }
    }
}
