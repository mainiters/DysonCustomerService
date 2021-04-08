using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace DysonCustomerService
{
    public class ProductDataProvider : BaseEntityDataProvider
    {
        public ProductDataProvider(Guid Id, UserConnection UserConnection)
            : base("Product", Id, UserConnection)
        {

        }

        protected override void AddRelatedColumns(EntitySchemaQuery esq, List<RelatedEntitiesData> relatedEntitiesData)
        {
            esq.AddColumn("TrcCustomerSegment.Name");

            relatedEntitiesData.Add(new DysonCustomerService.RelatedEntitiesData()
            {
                Name = "AccountAddress",
                AdditionalColumns = new List<string>()
                {

                }
            });

            base.AddRelatedColumns(esq, relatedEntitiesData);
        }

        public override object GetEntityData(Guid EntityId)
        {
            var res = new ПакетНоменклатуры();

            res.ID_Pack = new Guid().ToString();

            // Данные Контрагента
            res.Tovar = new Номенклатура[]
            {
                new Номенклатура()
                {
                    Article = this.EntityObject.GetTypedColumnValue<string>("Code")
                }
            };

            return res;
        }
    }
}
