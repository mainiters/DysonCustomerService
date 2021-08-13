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
            : base("Account", Id, UserConnection, "Trc1CAccountID")
        {

        }

        protected override void AddRelatedColumns(EntitySchemaQuery esq, List<RelatedEntitiesData> relatedEntitiesData)
        {
            esq.AddColumn("TrcCustomerSegment.TrcCode");
            
            esq.AddColumn("PrimaryContact.Email");
            esq.AddColumn("PrimaryContact.Name");
            esq.AddColumn("TrcTypeLE.Name");
            
            esq.AddColumn("PrimaryContact.MobilePhone");
            esq.AddColumn("PrimaryContact.JobTitle");

            esq.AddColumn("TrcAdditionalContact.Email");
            esq.AddColumn("TrcAdditionalContact.Name");
            esq.AddColumn("TrcAdditionalContact.MobilePhone");
            esq.AddColumn("TrcAdditionalContact.JobTitle");

            relatedEntitiesData.Add(new RelatedEntitiesData()
            {
                Name = "AccountAddress",
                AdditionalColumns = new List<string>()
                {
                    "TrcFiasCode",
                    "Primary",
                    "AddressType",
                    "Region.Name",
                    "TrcSettlement.Name",
                    "City.Name",
                    "Area.Name"
                }
            });

            relatedEntitiesData.Add(new RelatedEntitiesData()
            {
                Name = "AccountBillingInfo",
                AdditionalColumns = new List<string>()
                {
                    "TrcCurrency.Name"
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

            var addressLegalEntity = RelatedEntitiesData.Where(e => e.Name == "AccountAddress")
                .First().EntityCollection.Where(e => e.GetTypedColumnValue<Guid>("AddressTypeId") == Guid.Parse("770bf68c-4b6e-df11-b988-001d60e938c6"))
                .FirstOrDefault();

            var addressFactEntity = RelatedEntitiesData.Where(e => e.Name == "AccountAddress")
                .First().EntityCollection.Where(e => e.GetTypedColumnValue<Guid>("AddressTypeId") == Guid.Parse("780bf68c-4b6e-df11-b988-001d60e938c6"))
                .FirstOrDefault();

            var addressLegal = string.Empty;
            var addressFact = string.Empty;

            if (addressLegalEntity != null)
            {
                addressLegal = addressLegalEntity.GetTypedColumnValue<string>("Address");
            }

            if (addressFactEntity != null)
            {
                addressFact = addressFactEntity.GetTypedColumnValue<string>("Address");
            }

            // Данные Контрагента
            res = new Контрагенты()
            {
                ID_1С = this.EntityObject.GetTypedColumnValue<string>("Trc1CAccountID"),
                Name = this.EntityObject.GetTypedColumnValue<string>("TrcShortName"),
                FullName = this.EntityObject.GetTypedColumnValue<string>("TrcWorkingTitle"),
                TypeLE = this.EntityObject.GetTypedColumnValue<string>("TrcTypeLE_Name"),
                LegalPhoneNumber = this.EntityObject.GetTypedColumnValue<string>("Phone"),
                FSRL = this.EntityObject.GetTypedColumnValue<bool>("TrcIsServiceMailing"),
                FMRL = this.EntityObject.GetTypedColumnValue<bool>("TrcIsMarketingMailing"),
                ObjectTypeList = this.EntityObject.GetTypedColumnValue<string>("TrcCustomerSegment_TrcCode"),
                INN = this.EntityObject.GetTypedColumnValue<string>("TrcInn"),
                KPP = this.EntityObject.GetTypedColumnValue<string>("TrcKpp"),
                FIOYO = this.EntityObject.GetTypedColumnValue<string>("PrimaryContact_Name"),
                PostYO = this.EntityObject.GetTypedColumnValue<string>("PrimaryContact_JobTitle"),
                MobTelYO = this.EntityObject.GetTypedColumnValue<string>("PrimaryContact_MobilePhone"),
                EmailYO = this.EntityObject.GetTypedColumnValue<string>("PrimaryContact_Email"),
                FIOYD = this.EntityObject.GetTypedColumnValue<string>("TrcAdditionalContact_Name"),
                PostYD = this.EntityObject.GetTypedColumnValue<string>("TrcAdditionalContact_JobTitle"),
                MobTelYD = this.EntityObject.GetTypedColumnValue<string>("TrcAdditionalContact_MobilePhone"),
                EmailYD = this.EntityObject.GetTypedColumnValue<string>("TrcAdditionalContact_Email"),
                PointSale = this.EntityObject.GetTypedColumnValue<bool>("TrcPointSale"),
                HeadCP = this.EntityObject.GetTypedColumnValue<bool>("TrcHeadCP"),

                AddressFact = addressFact,
                FIAS = addressFactEntity.GetTypedColumnValue<string>("TrcFiasCode"),
                Region = addressFactEntity.GetTypedColumnValue<string>("Region_Name"),
                Locality = addressFactEntity.GetTypedColumnValue<string>("TrcSettlement_Name"),
                Street = addressFactEntity.GetTypedColumnValue<string>("TrcStreet"),
                Metro = addressFactEntity.GetTypedColumnValue<string>("TrcMetroStation"),
                House = addressFactEntity.GetTypedColumnValue<string>("TrcHouse"),
                Korp = addressFactEntity.GetTypedColumnValue<string>("TrcBlock"),
                Flat = addressFactEntity.GetTypedColumnValue<string>("TrcApartment"),
                Entrance = addressFactEntity.GetTypedColumnValue<string>("TrcEntrance"),
                FLOOR = addressFactEntity.GetTypedColumnValue<string>("TrcFloor"),
                Intercom = addressFactEntity.GetTypedColumnValue<string>("TrcIntercom"),
                City = addressFactEntity.GetTypedColumnValue<string>("City_Name"),
                Area = addressFactEntity.GetTypedColumnValue<string>("TrcArea_Name"),
                Lat = addressFactEntity.GetTypedColumnValue<string>("TrcGPSE"),
                Lon = addressFactEntity.GetTypedColumnValue<string>("TrcGPSN"),

                AddressLegal = addressLegal,
                FIASL = addressLegalEntity.GetTypedColumnValue<string>("TrcFiasCode"),
                RegionL = addressLegalEntity.GetTypedColumnValue<string>("Region_Name"),
                LocalityL = addressLegalEntity.GetTypedColumnValue<string>("TrcSettlement_Name"),
                StreetL = addressLegalEntity.GetTypedColumnValue<string>("TrcStreet"),
                MetroL = addressLegalEntity.GetTypedColumnValue<string>("TrcMetroStation"),
                HouseL = addressLegalEntity.GetTypedColumnValue<string>("TrcHouse"),
                KorpL = addressLegalEntity.GetTypedColumnValue<string>("TrcBlock"),
                FlatL = addressLegalEntity.GetTypedColumnValue<string>("TrcApartment"),
                EntranceL = addressLegalEntity.GetTypedColumnValue<string>("TrcEntrance"),
                FLOORL = addressLegalEntity.GetTypedColumnValue<string>("TrcFloor"),
                IntercomL = addressLegalEntity.GetTypedColumnValue<string>("TrcIntercom"),
                CityL = addressLegalEntity.GetTypedColumnValue<string>("City_Name"),
                AreaL = addressLegalEntity.GetTypedColumnValue<string>("TrcArea_Name"),
                LatL = addressLegalEntity.GetTypedColumnValue<string>("TrcGPSE"),
                LonL = addressLegalEntity.GetTypedColumnValue<string>("TrcGPSN"),

                IDDepersonalizedClient = string.Empty,
                PhoneNumber = string.Empty,
                StatusClient = string.Empty,
                Building = string.Empty,
                BuildingL = string.Empty,
                OPTINsent = new DateTime(),
                Patronymic = string.Empty,
                Surname = string.Empty,
                Name_F = string.Empty,

                MarkDeletion = this.EntityObject.GetTypedColumnValue<bool>("TrcMarkDeletion"),

                Email = this.GetEmail(),
                
            };

            var billinginfo = new List<КонтрагентыBillinginfo>();

            if (this.RelatedEntitiesData.Count > 0)
            {
                foreach (var item in this.RelatedEntitiesData.Where(e => e.Name == "AccountBillingInfo").First().EntityCollection)
                {
                    billinginfo.Add(new КонтрагентыBillinginfo()
                    {
                        AccountNumber = item.GetTypedColumnValue<string>("TrcAccountNumber"),
                        BIK = item.GetTypedColumnValue<string>("TrcBik"),
                        Currency = item.GetTypedColumnValue<string>("TrcCurrency_Name"),
                    });
                }
            }

            res.Billinginfo = billinginfo.ToArray();

            return res;
        }

        protected string GetEmail()
        {
            var select = new Terrasoft.Core.DB.Select(UserConnection)
                    .Column("Number")
                    .From("AccountCommunication")
                    .Where("AccountId").IsEqual(Column.Parameter(this.EntityId))
                    .And("CommunicationTypeId").IsEqual(Column.Parameter("EE1C85C3-CFCB-DF11-9B2A-001D60E938C6"))
                    .And("Primary").IsEqual(Column.Const(true)) as Select;

            return select.ExecuteScalar<string>();
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
    }
}
