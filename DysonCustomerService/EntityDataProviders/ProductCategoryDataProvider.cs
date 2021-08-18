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

            var categories = this.GetCategories();

            if (categories != null)
            {
                res.Сategories = categories;
            }

            return res;
        }

        protected ВидыНоменклатурыCRMСategories[] GetCategories()
        {
            ВидыНоменклатурыCRMСategories[] res = null;

            var esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "ProductType");

            esq.AddAllSchemaColumns();

            esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcProductCategory", this.EntityId));

            var collection = esq.GetEntityCollection(UserConnection);

            if(collection != null && collection.Count > 0)
            {
                var notEmptyItems = collection.Where(e => !string.IsNullOrEmpty(e.GetTypedColumnValue<string>("TrcCode")));

                if (notEmptyItems.Count() > 0)
                {
                    res = notEmptyItems.Select(e => new ВидыНоменклатурыCRMСategories() { Сategorie = e.GetTypedColumnValue<string>("TrcCode") }).ToArray();
                }
            }

            return res;
        }
    }
}
