using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace DysonCustomerService
{
    public class ContactDataProvider : BaseEntityDataProvider
    {
        public ContactDataProvider(Guid Id, UserConnection UserConnection)
            : base("Contact", Id, UserConnection)
        {

        }

        protected override void AddRelatedColumns(EntitySchemaQuery esq, List<RelatedEntitiesData> relatedEntitiesData)
        {
            esq.AddColumn("TrcCustomerSegment.Name");

            relatedEntitiesData.Add(new DysonCustomerService.RelatedEntitiesData()
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

            var clientId = this.EntityObject.GetTypedColumnValue<Guid>(this.EntitySchemaName);
            var address = this.EntityObject.GetTypedColumnValue<string>("DeliveryAddress");

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
                    EmailYD = this.EntityObject.GetTypedColumnValue<string>("TrcEmailAdditionalContact"),
                    FSSMS = this.EntityObject.GetTypedColumnValue<bool>("TrcServiceSMS"),
                    FSE = this.EntityObject.GetTypedColumnValue<bool>("TrcServiceEmail"),
                    FME = this.EntityObject.GetTypedColumnValue<bool>("TrcMarketingEmail"),
                    FSCO = this.EntityObject.GetTypedColumnValue<bool>("TrcServiceCall"),
                    FMCO = this.EntityObject.GetTypedColumnValue<bool>("TrcMarketingCal"),
                    FMSMS = this.EntityObject.GetTypedColumnValue<bool>("TrcMarketingSMS"),
                    FSR = this.EntityObject.GetTypedColumnValue<bool>("TrcIsServiceMailing"),
                    FMR = this.EntityObject.GetTypedColumnValue<bool>("TrcIsMarketingMailing")
                }
            };

            return res;
        }

        protected Entity GetOrderAddressData(Guid clientId, string address)
        {
            EntitySchema relatedSchema = this.UserConnection.EntitySchemaManager.GetInstanceByName("VwClientAddress");

            EntitySchemaQuery relatedEsq = new EntitySchemaQuery(relatedSchema)
            {
                UseAdminRights = true,
                CanReadUncommitedData = true,
                IgnoreDisplayValues = true
            };

            relatedEsq.Filters.Add(relatedEsq.CreateFilterWithParameters(FilterComparisonType.Equal, "Client", clientId));
            relatedEsq.Filters.Add(relatedEsq.CreateFilterWithParameters(FilterComparisonType.Equal, "Address", address));

            relatedEsq.AddAllSchemaColumns();

            return relatedEsq.GetEntityCollection(this.UserConnection).FirstOrDefault();
        }
    }
}
