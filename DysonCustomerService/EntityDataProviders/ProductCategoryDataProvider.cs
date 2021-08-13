using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace DysonCustomerService.EntityDataProviders
{
    public class ProductCategoryDataProvider : BaseEntityDataProvider
    {
        public ProductCategoryDataProvider(Guid Id, UserConnection UserConnection)
            : base("ProductCategory", Id, UserConnection, "TrcCode")
        {
        }

        protected override void AddRelatedColumns(EntitySchemaQuery esq, List<RelatedEntitiesData> relatedEntitiesData)
        {
            base.AddRelatedColumns(esq, relatedEntitiesData);
        }

        public override object GetEntityData(Guid EntityId)
        {
            var res = new ВидыНоменклатурыCRM()
            {
                Name = this.EntityObject.GetTypedColumnValue<string>("Name"),
                MarkDeletion = this.EntityObject.GetTypedColumnValue<bool>("TrcMarkDeletion"),
                ID_1С = this.EntityObject.GetTypedColumnValue<string>("TrcCode"),
            };

            return res;
        }
    }
}
