using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace DysonCustomerService
{
    public class CustomerDefectDataProvider : BaseEntityDataProvider
    {
        public CustomerDefectDataProvider(Guid Id, UserConnection UserConnection)
            : base("TrcCustomerDefect", Id, UserConnection)
        {

        }

        protected override void AddRelatedColumns(EntitySchemaQuery esq, List<RelatedEntitiesData> relatedEntitiesData)
        {
            base.AddRelatedColumns(esq, relatedEntitiesData);
        }

        public override object GetEntityData(Guid EntityId)
        {
            var res = new ПакетДефектыКлиента();

            res.ID_Pack = new Guid().ToString();

            // Данные Дефекты
            res.DefectClient = new ДефектыСоСловКлиента[]
            {
                new ДефектыСоСловКлиента()
                {
                    Name = this.EntityObject.GetTypedColumnValue<string>("Name")
                }
            };

            return res;
        }
    }
}
