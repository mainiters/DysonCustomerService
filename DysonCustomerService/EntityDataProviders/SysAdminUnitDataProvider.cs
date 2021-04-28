using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace DysonCustomerService.EntityDataProviders
{
    public class SysAdminUnitDataProvider : BaseEntityDataProvider
    {
        public SysAdminUnitDataProvider(Guid Id, UserConnection UserConnection)
            : base("Contact", Id, UserConnection)
        {

        }

        protected override void AddRelatedColumns(EntitySchemaQuery esq, List<RelatedEntitiesData> relatedEntitiesData)
        {
            base.AddRelatedColumns(esq, relatedEntitiesData);
        }

        public override object GetEntityData(Guid EntityId)
        {
            var res = new Пользователи();

            // Данные Пользователей
            res = new Пользователи()
            {
                Name = this.EntityObject.GetTypedColumnValue<string>("Name")
            };

            return res;
        }
    }
}
