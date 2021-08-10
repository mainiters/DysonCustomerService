using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace DysonCustomerService.EntityDataProviders
{
    public class EmployeeDataProvider : BaseEntityDataProvider
    {
        public EmployeeDataProvider(Guid Id, UserConnection UserConnection)
            : base("Employee", Id, UserConnection, "TrcCode")
        {

        }

        protected override void AddRelatedColumns(EntitySchemaQuery esq, List<RelatedEntitiesData> relatedEntitiesData)
        {
            esq.AddColumn("Contact.Gender.Name");
            esq.AddColumn("Contact.Email");
            esq.AddColumn("Contact.MobilePhone");

            base.AddRelatedColumns(esq, relatedEntitiesData);
        }

        public override object GetEntityData(Guid EntityId)
        {
            var res = new ФизическиеЛица()
            {
                Name = this.EntityObject.GetTypedColumnValue<string>("Name"),
                MarkDeletion = this.EntityObject.GetTypedColumnValue<bool>("TrcMarkDeletion"),
                ID_1С = this.EntityObject.GetTypedColumnValue<string>("TrcCode"),
                Email = this.EntityObject.GetTypedColumnValue<string>("Contact_Email"),
                SEX = this.EntityObject.GetTypedColumnValue<string>("Contact_Gender_Name"),
                TEL = this.EntityObject.GetTypedColumnValue<string>("Contact_MobilePhone"),
            };

            return res;
        }
    }
}
