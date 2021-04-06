using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace DysonCustomerService
{
    public class AccountDataProvider : BaseEntityDataProvider
    {
        public AccountDataProvider(Guid Id, UserConnection UserConnection)
            : base("Account", Id, UserConnection)
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
            var res = new ПакетКонтрагентов();

            res.ID_Pack = new Guid().ToString();

            // Данные Контрагента
            res.Partner = new Контрагенты[]
            {
                new Контрагенты()
                {
                    Name = this.EntityObject.GetTypedColumnValue<string>("Name"),
                    LegalPhoneNumber = this.EntityObject.GetTypedColumnValue<string>("Phone"),
                    FIAS = this.EntityObject.GetTypedColumnValue<string>("Address"),
                    ObjectTypeList = this.EntityObject.GetTypedColumnValue<string>("TrcCustomerSegment_Name"),
                    INN = this.EntityObject.GetTypedColumnValue<string>("TrcInn"),
                    KPP = this.EntityObject.GetTypedColumnValue<string>("TrcKpp"),
                    FIOYO = this.EntityObject.GetTypedColumnValue<string>("TrcMainContactFIO"),
                    PostYO = this.EntityObject.GetTypedColumnValue<string>("TrcJobMainContact"),
                    MobTelYO = this.EntityObject.GetTypedColumnValue<string>("TrcMobilePhoneMainContact"),
                    EmailYO = this.EntityObject.GetTypedColumnValue<string>("TrcEmailMainContact"),
                    FIOYD = this.EntityObject.GetTypedColumnValue<string>("TrcFIOAdditionalContact"),
                    PostYD = this.EntityObject.GetTypedColumnValue<string>("TrcJobAdditionalContact"),
                    MobTelYD = this.EntityObject.GetTypedColumnValue<string>("TrcMobilePhoneAdditionalContact"),
                    EmailYD = this.EntityObject.GetTypedColumnValue<string>("TrcEmailAdditionalContact")
                }
            };

            return res;
        }
    }
}
