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
    public class ContactDataProvider : BaseEntityDataProvider
    {
        public ContactDataProvider(Guid Id, UserConnection UserConnection)
            : base("Contact", Id, UserConnection)
        {

        }

        protected override void AddRelatedColumns(EntitySchemaQuery esq, List<RelatedEntitiesData> relatedEntitiesData)
        {
            relatedEntitiesData.Add(new RelatedEntitiesData()
            {
                Name = "ContactAddress",
                AdditionalColumns = new List<string>()
                {
                    "TrcFiasCode",
                    "Primary"
                }
            });

            base.AddRelatedColumns(esq, relatedEntitiesData);
        }
        public override object GetEntityData(Guid EntityId)
        {
            var res = new ПакетКонтрагентов();

            res.ID_Pack = new Guid().ToString();

            var clientId = this.EntityObject.GetTypedColumnValue<Guid>("Id");
            var address = this.EntityObject.GetTypedColumnValue<string>("Address");

            var addressData = this.GetOrderAddressData(clientId, address);

            var fias = RelatedEntitiesData.Where(e => e.Name == "ContactAddress")
                .First().EntityCollection.Where(e => e.GetTypedColumnValue<bool>("Primary"))
                .FirstOrDefault()?.GetTypedColumnValue<string>("TrcFiasCode");

            // Данные Контактов
            res.Partner = new []
            {
                new Контрагенты()
                {
                    Name = this.EntityObject.GetTypedColumnValue<string>("Name"),
                    LegalPhoneNumber = this.EntityObject.GetTypedColumnValue<string>("Phone"),
                    FIAS = fias,
                    FSSMS = this.EntityObject.GetTypedColumnValue<bool>("TrcServiceSMS"),
                    FSE = this.EntityObject.GetTypedColumnValue<bool>("TrcServiceEmail"),
                    FME = this.EntityObject.GetTypedColumnValue<bool>("TrcMarketingEmail"),
                    FSCO = this.EntityObject.GetTypedColumnValue<bool>("TrcServiceCall"),
                    FMCO = this.EntityObject.GetTypedColumnValue<bool>("TrcMarketingCall"),
                    FMSMS = this.EntityObject.GetTypedColumnValue<bool>("TrcMarketingSMS"),
                    FSR = this.EntityObject.GetTypedColumnValue<bool>("TrcIsServiceMailing"),
                    FMR = this.EntityObject.GetTypedColumnValue<bool>("TrcIsMarketingMailing")
                }
            };

            return res;
        }

        public override string GetServiceMethodName()
        {
            return "PostPartners";
        }

        protected Guid GetOrderAddressData(Guid clientId, string address)
        {
            var select = new Select(UserConnection)
                    .Column("CityId")
                    .From("VwClientAddress")
                    .Where("ClientId").IsEqual(Column.Parameter(clientId))
                    .And("Address").IsEqual(Column.Parameter(address)) as Select;

            return select.ExecuteScalar<Guid>();
        }
    }
}
