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
            : base("Contact", Id, UserConnection, "Trc1CContactID")
        {

        }

        protected override void AddRelatedColumns(EntitySchemaQuery esq, List<RelatedEntitiesData> relatedEntitiesData)
        {
            esq.AddColumn("TrcContactCategory.TrcCode");

            relatedEntitiesData.Add(new RelatedEntitiesData()
            {
                Name = "ContactAddress",
                AdditionalColumns = new List<string>()
                {
                    "TrcFiasCode",
                    "Primary",
                    "Region.Name",
                    "TrcSettlement.Name",
                    "City.Name",
                    "TrcArea.Name"
                }
            });

            base.AddRelatedColumns(esq, relatedEntitiesData);
        }
        public override object GetEntityData(Guid EntityId)
        {
            var res = new Контрагенты();

            var clientId = this.EntityObject.GetTypedColumnValue<Guid>("Id");
            var address = this.EntityObject.GetTypedColumnValue<string>("Address");

            var addressData = this.GetOrderAddressData(clientId, address);

            var addressFactEntity = RelatedEntitiesData.Where(e => e.Name == "ContactAddress")
                .First().EntityCollection.Where(e => e.GetTypedColumnValue<bool>("Primary"))
                .FirstOrDefault();

            var addressFact = string.Empty;

            if (addressFactEntity != null)
            {
                addressFact = addressFactEntity.GetTypedColumnValue<string>("Address");
            }

            // Данные Контактов
            res = new Контрагенты()
            {
                FSSMS = this.EntityObject.GetTypedColumnValue<bool>("TrcServiceSMS"),
                FSE = this.EntityObject.GetTypedColumnValue<bool>("TrcServiceEmail"),
                FME = this.EntityObject.GetTypedColumnValue<bool>("TrcMarketingEmail"),
                FSCO = this.EntityObject.GetTypedColumnValue<bool>("TrcServiceCall"),
                FMCO = this.EntityObject.GetTypedColumnValue<bool>("TrcMarketingCall"),
                FMSMS = this.EntityObject.GetTypedColumnValue<bool>("TrcMarketingSMS"),
                FSR = this.EntityObject.GetTypedColumnValue<bool>("TrcIsServiceMailing"),
                FMR = this.EntityObject.GetTypedColumnValue<bool>("TrcIsMarketingMailing"),
                ID_1С = this.EntityObject.GetTypedColumnValue<string>("Trc1CContactID"),
                MarkDeletion = this.EntityObject.GetTypedColumnValue<bool>("TrcMarkDeletion"),
                Patronymic = this.EntityObject.GetTypedColumnValue<string>("MiddleName"),
                PhoneNumber = this.EntityObject.GetTypedColumnValue<string>("MobilePhone"),
                Email = this.EntityObject.GetTypedColumnValue<string>("Email"),
                ThereAreLK = this.EntityObject.GetTypedColumnValue<bool>("TrcIsLkLinkSend"),
                IDDepersonalizedClient = this.EntityObject.GetTypedColumnValue<string>("TrcIDDepersonalizedClient"),
                StatusClient = this.EntityObject.GetTypedColumnValue<string>("TrcContactCategory_TrcCode"),
                Surname = this.EntityObject.GetTypedColumnValue<string>("Surname"),
                Name_F = this.EntityObject.GetTypedColumnValue<string>("GivenName"),

                AddressFact = addressFact,

                FIAS = addressFactEntity == null ? string. Empty : addressFactEntity.GetTypedColumnValue<string>("TrcFiasCode"),
                Region = addressFactEntity == null ? string.Empty : addressFactEntity.GetTypedColumnValue<string>("Region_Name"),
                Locality = addressFactEntity == null ? string.Empty : addressFactEntity.GetTypedColumnValue<string>("TrcSettlement_Name"),
                Street = addressFactEntity == null ? string.Empty : addressFactEntity.GetTypedColumnValue<string>("TrcStreet"),
                Metro = addressFactEntity == null ? string.Empty : addressFactEntity.GetTypedColumnValue<string>("TrcMetroStation"),
                House = addressFactEntity == null ? string.Empty : addressFactEntity.GetTypedColumnValue<string>("TrcHouse"),
                Korp = addressFactEntity == null ? string.Empty : addressFactEntity.GetTypedColumnValue<string>("TrcBlock"),
                Flat = addressFactEntity == null ? string.Empty : addressFactEntity.GetTypedColumnValue<string>("TrcApartment"),
                Entrance = addressFactEntity == null ? string.Empty : addressFactEntity.GetTypedColumnValue<string>("TrcEntrance"),
                FLOOR = addressFactEntity == null ? string.Empty : addressFactEntity.GetTypedColumnValue<string>("TrcFloor"),
                Intercom = addressFactEntity == null ? string.Empty : addressFactEntity.GetTypedColumnValue<string>("TrcIntercom"),
                City = addressFactEntity == null ? string.Empty : addressFactEntity.GetTypedColumnValue<string>("City_Name"),
                Area = addressFactEntity == null ? string.Empty : addressFactEntity.GetTypedColumnValue<string>("TrcArea_Name"),
                Lat = addressFactEntity == null ? string.Empty : addressFactEntity.GetTypedColumnValue<string>("TrcGPSE"),
                Lon = addressFactEntity == null ? string.Empty : addressFactEntity.GetTypedColumnValue<string>("TrcGPSN"),
                
                PointSale = false,
                AddressLegal = string.Empty,
                Name = string.Empty,
                OPTINsent = new DateTime(),
                LegalPhoneNumber = string.Empty,
                ObjectTypeList = string.Empty,
                INN = string.Empty,
                KPP = string.Empty,
                FIOYO = string.Empty,
                PostYO = string.Empty,
                MobTelYO = string.Empty,
                EmailYO = string.Empty,
                FIOYD = string.Empty,
                PostYD = string.Empty,
                MobTelYD = string.Empty,
                EmailYD = string.Empty,
                Building = string.Empty,

                AreaL = string.Empty,
                FIASL = string.Empty,
                BuildingL = string.Empty,
                CityL = string.Empty,
                EntranceL = string.Empty,
                FlatL = string.Empty,
                FLOORL = string.Empty,
                FullName = string.Empty,
                HouseL = string.Empty,
                IntercomL = string.Empty,
                KorpL = string.Empty,
                LatL = string.Empty,
                LonL = string.Empty,
                MetroL = string.Empty,
                LocalityL = string.Empty,
                RegionL = string.Empty,
                StreetL = string.Empty,
                TypeLE = string.Empty
            };

            return res;
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
