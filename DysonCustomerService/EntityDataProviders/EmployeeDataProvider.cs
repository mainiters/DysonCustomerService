using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.DB;
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
            esq.AddColumn("Contact.Id");
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
                Sex = this.EntityObject.GetTypedColumnValue<string>("Contact_Gender_Name"),
                MobTel = this.EntityObject.GetTypedColumnValue<string>("Contact_MobilePhone"),
            };

            return res;
        }
        protected override void UpdateEntityIdFromPackage(ПакетОтвета response)
        {
            base.UpdateEntityIdFromPackage(response);

            if (response != null && !string.IsNullOrWhiteSpace(response.ID_Pack))
            {
                var update = new Update(UserConnection, "Contact")
                        .Set("Trc1CContactID", Column.Parameter(response.ID_Pack))
                        .Where("Id").IsEqual(Column.Parameter(this.EntityObject.GetTypedColumnValue<string>("Contact_Id")));

                update.Execute();
            }
        }
    }
}
