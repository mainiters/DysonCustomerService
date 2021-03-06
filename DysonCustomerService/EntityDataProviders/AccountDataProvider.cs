﻿using System;
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
                    "AddressType"
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

            var addressLegal = RelatedEntitiesData.Where(e => e.Name == "AccountAddress")
                .First().EntityCollection.Where(e => e.GetTypedColumnValue<Guid>("AddressTypeId") == Guid.Parse("770bf68c-4b6e-df11-b988-001d60e938c6"))
                .FirstOrDefault()?.GetTypedColumnValue<string>("Address");

            var addressFact = RelatedEntitiesData.Where(e => e.Name == "AccountAddress")
                .First().EntityCollection.Where(e => e.GetTypedColumnValue<Guid>("AddressTypeId") == Guid.Parse("780bf68c-4b6e-df11-b988-001d60e938c6"))
                .FirstOrDefault()?.GetTypedColumnValue<string>("Address");

            if (string.IsNullOrEmpty(addressLegal))
            {
                addressLegal = string.Empty;
            }

            if (string.IsNullOrEmpty(addressFact))
            {
                addressFact = string.Empty;
            }

            // Данные Контрагента
            res = new Контрагенты()
            {
                Name = this.EntityObject.GetTypedColumnValue<string>("Name"),
                LegalPhoneNumber = this.EntityObject.GetTypedColumnValue<string>("Phone"),
                ObjectTypeList = this.EntityObject.GetTypedColumnValue<string>("TrcCustomerSegment_TrcCode"),
                INN = this.EntityObject.GetTypedColumnValue<string>("TrcInn"),
                KPP = this.EntityObject.GetTypedColumnValue<string>("TrcKpp"),

                AddressFact = addressFact,
                AddressLegal = addressLegal,

                FIOYO = this.EntityObject.GetTypedColumnValue<string>("PrimaryContact_Name"),
                PostYO = this.EntityObject.GetTypedColumnValue<string>("PrimaryContact_JobTitle"),
                MobTelYO = this.EntityObject.GetTypedColumnValue<string>("PrimaryContact_MobilePhone"),
                EmailYO = this.EntityObject.GetTypedColumnValue<string>("PrimaryContact_Email"),

                FIOYD = this.EntityObject.GetTypedColumnValue<string>("TrcAdditionalContact_Name"),
                PostYD = this.EntityObject.GetTypedColumnValue<string>("TrcAdditionalContact_JobTitle"),
                MobTelYD = this.EntityObject.GetTypedColumnValue<string>("TrcAdditionalContact_MobilePhone"),
                EmailYD = this.EntityObject.GetTypedColumnValue<string>("TrcAdditionalContact_Email"),

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

                ThereAreLK = this.EntityObject.GetTypedColumnValue<bool>("TrcIsRegisteredOnSite"),
                PointSale = this.EntityObject.GetTypedColumnValue<bool>("TrcPointSale"),
                MarkDeletion = this.EntityObject.GetTypedColumnValue<bool>("TrcMarkDeletion"),

                Email = this.GetEmail()
            };

            return res;
        }

        protected string GetEmail()
        {
            var select = new Terrasoft.Core.DB.Select(UserConnection)
                    .Column("Number")
                    .From("AccountCommunication")
                    .Where("AccountId").IsEqual(Column.Parameter(this.EntityId))
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
        public override string GetServiceMethodName()
        {
            return "PostClient";
        }
    }
}
