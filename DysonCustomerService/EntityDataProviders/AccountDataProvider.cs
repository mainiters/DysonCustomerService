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
    public class AccountDataProvider : BaseEntityDataProvider
    {
        public AccountDataProvider(Guid Id, UserConnection UserConnection)
            : base("Account", Id, UserConnection)
        {

        }

        protected override void AddRelatedColumns(EntitySchemaQuery esq, List<RelatedEntitiesData> relatedEntitiesData)
        {
            esq.AddColumn("TrcCustomerSegment.TrcCode");

            relatedEntitiesData.Add(new RelatedEntitiesData()
            {
                Name = "AccountAddress",
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
            var res = new Контрагенты();

            //res.ID_Pack = new Guid().ToString();

            var clientId = this.EntityObject.GetTypedColumnValue<Guid>("Id");
            var address = this.EntityObject.GetTypedColumnValue<string>("Address");

            var addressData = this.GetOrderAddressData(clientId, address);

            var fias = RelatedEntitiesData.Where(e => e.Name == "AccountAddress")
                .First().EntityCollection.Where(e => e.GetTypedColumnValue<bool>("Primary"))
                .FirstOrDefault()?.GetTypedColumnValue<string>("TrcFiasCode");

            // Данные Контрагента
            res = new Контрагенты()
            {
                Name = this.EntityObject.GetTypedColumnValue<string>("Name"),
                LegalPhoneNumber = this.EntityObject.GetTypedColumnValue<string>("Phone"),
                FIAS = fias ?? string.Empty,
                ObjectTypeList = this.EntityObject.GetTypedColumnValue<string>("TrcCustomerSegment_TrcCode"),
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
                FMCO = this.EntityObject.GetTypedColumnValue<bool>("TrcMarketingCall"),
                FMSMS = this.EntityObject.GetTypedColumnValue<bool>("TrcMarketingSMS"),
                FSR = this.EntityObject.GetTypedColumnValue<bool>("TrcIsServiceMailing"),
                FMR = this.EntityObject.GetTypedColumnValue<bool>("TrcIsMarketingMailing"),
                IDDepersonalizedClient = this.EntityObject.GetTypedColumnValue<string>("TrcIDDepersonalizedClient"),
                ID_1С = this.EntityObject.GetTypedColumnValue<string>("Trc1CAccountID"),

                Legal = true,
                FIO_F = string.Empty,
                MiddleName_F = string.Empty,
                Name_F = string.Empty,
                PhoneNumber = string.Empty,
                DysonChannelCode = string.Empty,
                StatusClient = string.Empty,

                // Тянем из детали Средств связи с типом Email
                Email = "12312312"
            };

            return res;
        }

        protected Guid GetOrderAddressData(Guid clientId, string address)
        {
            var select = new Terrasoft.Core.DB.Select(UserConnection)
                    .Column("CityId")
                    .From("VwClientAddress")
                    .Where("ClientId").IsEqual(Column.Parameter(clientId))
                    .And("Address").IsEqual(Column.Parameter(address)) as Select;

            return select.ExecuteScalar<Guid>();
        }
        public override string GetServiceMethodName()
        {
            return "PostClient";
        }
    }
}
