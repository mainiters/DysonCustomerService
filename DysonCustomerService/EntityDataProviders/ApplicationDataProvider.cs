using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace DysonCustomerService.EntityDataProviders
{
    public class ApplicationDataProvider : BaseEntityDataProvider
    {
        public ApplicationDataProvider(Guid Id, UserConnection UserConnection)
            : base("TrcApplication", Id, UserConnection)
        {

        }

        protected override void AddRelatedColumns(EntitySchemaQuery esq, List<RelatedEntitiesData> relatedEntitiesData)
        {
            esq.AddColumn("TrcServiceCenter.TrcName");
            esq.AddColumn("TrcRepairWarehouse.TrcCode");
            esq.AddColumn("TrcRepairType.Name");
            esq.AddColumn("TrcProduct.Code");
            esq.AddColumn("TrcSerialNumberHistory.TrcSerialNumber.Name");
            esq.AddColumn("TrcEngineer.Trc1CContactID");
            esq.AddColumn("TrcWarrantyType.TrcCode");
            esq.AddColumn("TrcShop.TrcCode");
            esq.AddColumn("TrcServiceOption.Name");

            esq.AddColumn("TrcAccount.Trc1CAccountID");
            esq.AddColumn("TrcContact.Trc1CContactID");

            esq.AddColumn("TrcASCAndKC.TrcCode");
            esq.AddColumn("TrcOrganization.Trc1CAccountID");

            relatedEntitiesData.Add(new RelatedEntitiesData()
            {
                Name = "TrcExpertOpinion",
                FilterFieldName = "TrcRequest",
                AdditionalColumns = new List<string>()
                {
                    "TrcDefectFromSpecialist.TrcCode"
                }
            });

            relatedEntitiesData.Add(new RelatedEntitiesData()
            {
                Name = "TrcClientDefectDetail",
                FilterFieldName = "TrcRequest",
                AdditionalColumns = new List<string>()
                {
                    "TrcDefect.TrcCode"
                }
            });

            relatedEntitiesData.Add(new RelatedEntitiesData()
            {
                Name = "TrcSparePart",
                FilterFieldName = "TrcRequest",
                AdditionalColumns = new List<string>()
                {
                    "TrcProduct.Code"
                }
            });

            relatedEntitiesData.Add(new RelatedEntitiesData()
            {
                Name = "TrcService",
                FilterFieldName = "TrcRequest",
                AdditionalColumns = new List<string>()
                {
                    "TrcRequestService.Code"
                }
            });
            

            base.AddRelatedColumns(esq, relatedEntitiesData);
        }

        public override object GetEntityData(Guid EntityId)
        {
            string AccountId = this.EntityObject.GetTypedColumnValue<string>("TrcAccount_Trc1CAccountID");
            string ContactId = this.EntityObject.GetTypedColumnValue<string>("TrcContact_Trc1CContactID");

            string AscAndKcCode = this.EntityObject.GetTypedColumnValue<string>("TrcASCAndKC_TrcCode");
            string OrganizationCode = this.EntityObject.GetTypedColumnValue<string>("TrcOrganization_Trc1CAccountID");

            // Данные заказа
            var res = new ЗаявкаНаРемонт
            {
                ID_Сlient = string.IsNullOrEmpty(AccountId) ? ContactId : AccountId,
                DocumentNumber = this.EntityObject.GetTypedColumnValue<string>("TrcNumber"),
                CreateDate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcCreationDate"),
                Organization = string.IsNullOrEmpty(AscAndKcCode) ? OrganizationCode : AscAndKcCode,
                WarehouseCode = this.EntityObject.GetTypedColumnValue<string>("TrcRepairWarehouse_TrcCode"),
                ServiceOption = this.EntityObject.GetTypedColumnValue<string>("TrcServiceOption_Name"),
                TypeRepair = this.EntityObject.GetTypedColumnValue<string>("TrcRepairType_Name"),
                Article = this.EntityObject.GetTypedColumnValue<string>("TrcProduct_Code"),
                SN = this.EntityObject.GetTypedColumnValue<string>("TrcSerialNumberHistory_TrcSerialNumber_Name"),
                DateDeparture = this.EntityObject.GetTypedColumnValue<DateTime>("TrcDepartureDate"),
                Master = this.EntityObject.GetTypedColumnValue<string>("TrcEngineer_Trc1CContactID"),
                DatePurchase = this.EntityObject.GetTypedColumnValue<DateTime>("TrcPurchaseDate"),
                TypeGuarantee = this.EntityObject.GetTypedColumnValue<string>("TrcWarrantyType_TrcCode"),
                RenewalWarranty = this.EntityObject.GetTypedColumnValue<bool>("TrcIsWarrantyRenewed"),
                ActualDateRepairExecution = this.EntityObject.GetTypedColumnValue<DateTime>("TrcRepairDate"),
                InformationClient = this.EntityObject.GetTypedColumnValue<string>("TrcAdditionalInformation"),
                InternalComment = this.EntityObject.GetTypedColumnValue<string>("TrcInternalComment"),
                ViolationOperation = this.EntityObject.GetTypedColumnValue<bool>("TrcViolationOfExploitation"),
                Shop = this.EntityObject.GetTypedColumnValue<string>("TrcShop_TrcCode"),

                CommentsResultRepair = 0,
                FeedbackClient = string.Empty
            };

            if (this.RelatedEntitiesData.Count > 0)
            {
                // Деталь Заключение специалиста
                var expertOpinions = new List<ЗаявкаНаРемонтDefects>();

                foreach (var item in this.RelatedEntitiesData.Where(e => e.Name == "TrcExpertOpinion").First().EntityCollection)
                {
                    expertOpinions.Add(new ЗаявкаНаРемонтDefects()
                    {
                        Defect = item.GetTypedColumnValue<string>("TrcDefectFromSpecialist_TrcCode")
                    });
                }

                res.Defects = expertOpinions.ToArray();

                // Дефекты со слов клиента
                var clientDefects = new List<ЗаявкаНаРемонтDefectsAccordingClient>();

                foreach (var item in this.RelatedEntitiesData.Where(e => e.Name == "TrcClientDefectDetail").First().EntityCollection)
                {
                    clientDefects.Add(new ЗаявкаНаРемонтDefectsAccordingClient()
                    {
                        DefectAccordingClient = item.GetTypedColumnValue<string>("TrcDefect_Name")
                    });
                }

                res.DefectsAccordingClient = clientDefects.ToArray();

                // Запчасти
                var spareParts = new List<ЗаявкаНаРемонтSpares>();

                foreach (var item in this.RelatedEntitiesData.Where(e => e.Name == "TrcSparePart").First().EntityCollection)
                {
                    spareParts.Add(new ЗаявкаНаРемонтSpares()
                    {
                        Spare = item.GetTypedColumnValue<string>("TrcProduct_Code"),
                        Required = item.GetTypedColumnValue<int>("TrcQuantity"),
                        Availability = item.GetTypedColumnValue<int>("TrcAvailability"),
                        Price = item.GetTypedColumnValue<decimal>("TrcPrice"),
                        Paid = this.EntityObject.GetTypedColumnValue<string>("TrcZIPPaymentMethodId").ToLower() == "c540283c-c811-4c16-9144-8ee555bcba8f",
                        SpareAmount = item.GetTypedColumnValue<decimal>("TrcAmount")
                    });
                }

                res.Spares = spareParts.ToArray();

                // Услуги
                var services = new List<ЗаявкаНаРемонтServices>();

                foreach (var item in this.RelatedEntitiesData.Where(e => e.Name == "TrcService").First().EntityCollection)
                {
                    services.Add(new ЗаявкаНаРемонтServices()
                    {
                        Service = item.GetTypedColumnValue<string>("TrcRequestService_Code"),
                        Kol = item.GetTypedColumnValue<int>("TrcQuantity"),
                        Amount = item.GetTypedColumnValue<decimal>("TrcCost"),
                        Paid = item.GetTypedColumnValue<bool>("TrcPaidService"),
                        Content = item.GetTypedColumnValue<string>("TrcContent"),
                        Price = item.GetTypedColumnValue<decimal>("TrcPrice")
                    });
                }

                res.Services = services.ToArray();
            }

            return res;
        }
    }
}
