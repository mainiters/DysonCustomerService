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
            : base("TrcApplication", Id, UserConnection, "Trc1CApplicationID")
        {

        }

        protected override void AddRelatedColumns(EntitySchemaQuery esq, List<RelatedEntitiesData> relatedEntitiesData)
        {
            esq.AddColumn("TrcServiceCenter.TrcName");
            esq.AddColumn("TrcRepairWarehouse.TrcCode");
            esq.AddColumn("TrcRepairType.Name");
            esq.AddColumn("TrcProduct.Code");
            esq.AddColumn("TrcSerialNumberHistory.TrcSerialNumber.Name");
            esq.AddColumn("TrcEngineer.Name");
            esq.AddColumn("TrcWarrantyType.TrcCode");
            esq.AddColumn("TrcShop.TrcCode");
            esq.AddColumn("TrcServiceOption.Name");

            esq.AddColumn("TrcAccount.Trc1CAccountID");
            esq.AddColumn("TrcContact.Trc1CContactID");

            esq.AddColumn("TrcServiceCenter.TrcCode");
            esq.AddColumn("TrcOrganization.Trc1CAccountID");
            esq.AddColumn("TrcRequestFailureReason.Name");
            esq.AddColumn("TrcRequestStatus1C.Description");

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
                    "TrcProduct.Trc1CProductID"
                }
            });

            relatedEntitiesData.Add(new RelatedEntitiesData()
            {
                Name = "TrcService",
                FilterFieldName = "TrcRequest",
                AdditionalColumns = new List<string>()
                {
                    "TrcRequestService.Trc1CProductID"
                }
            });
            

            base.AddRelatedColumns(esq, relatedEntitiesData);
        }

        public override object GetEntityData(Guid EntityId)
        {
            string AccountId = this.EntityObject.GetTypedColumnValue<string>("TrcAccount_Trc1CAccountID");
            string ContactId = this.EntityObject.GetTypedColumnValue<string>("TrcContact_Trc1CContactID");

            string AscAndKcCode = this.EntityObject.GetTypedColumnValue<string>("TrcServiceCenter_TrcCode");
            string OrganizationCode = this.EntityObject.GetTypedColumnValue<string>("TrcOrganization_Trc1CAccountID");

            // Данные заявки
            var res = new ЗаявкаНаРемонт
            {
                Account = string.IsNullOrEmpty(AccountId) ? ContactId : AccountId,
                ID_1С = this.EntityObject.GetTypedColumnValue<string>("Trc1CApplicationID"),
                CreateDate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcCreationDate"),
                Organization = string.IsNullOrEmpty(AscAndKcCode) ? OrganizationCode : AscAndKcCode,
                WarehouseCode = this.EntityObject.GetTypedColumnValue<string>("TrcRepairWarehouse_TrcCode"),
                ServiceOption = this.EntityObject.GetTypedColumnValue<string>("TrcServiceOption_Name"),
                TypeRepair = this.EntityObject.GetTypedColumnValue<string>("TrcRepairType_Name"),
                Article = this.EntityObject.GetTypedColumnValue<string>("TrcProduct_Code"),
                SN = this.EntityObject.GetTypedColumnValue<string>("TrcSerialNumberHistory_TrcSerialNumber_Name"),
                DateDeparture = this.EntityObject.GetTypedColumnValue<DateTime>("TrcDepartureDate"),
                Master = string.IsNullOrEmpty(this.EntityObject.GetTypedColumnValue<string>("TrcEngineer_Name")) ? string.Empty : this.EntityObject.GetTypedColumnValue<string>("TrcEngineer_Name"),
                DatePurchase = this.EntityObject.GetTypedColumnValue<DateTime>("TrcPurchaseDate"),
                TypeGuarantee = this.EntityObject.GetTypedColumnValue<string>("TrcWarrantyType_TrcCode"),
                RenewalWarranty = this.EntityObject.GetTypedColumnValue<bool>("TrcIsWarrantyRenewed"),
                ActualDateRepairExecution = this.EntityObject.GetTypedColumnValue<DateTime>("TrcRepairDate"),
                InformationClient = this.EntityObject.GetTypedColumnValue<string>("TrcAdditionalInformation"),
                InternalComment = this.EntityObject.GetTypedColumnValue<string>("TrcInternalComment"),
                ViolationOperation = this.EntityObject.GetTypedColumnValue<bool>("TrcViolationOfExploitation"),
                Shop = this.EntityObject.GetTypedColumnValue<string>("TrcShop_TrcCode"),
                MarkDeletion = this.EntityObject.GetTypedColumnValue<bool>("TrcMarkDeletion"),
                CommentsResultRepair = this.EntityObject.GetTypedColumnValue<string>("TrcResultComment"),
                InterviewStatus = this.EntityObject.GetTypedColumnValue<string>("TrcInterviewStatus"),

                CustomerNote = this.EntityObject.GetTypedColumnValue<string>("TrcCustomerNote"),
                NPSProductIndex = this.EntityObject.GetTypedColumnValue<int>("TrcNPSProductIndex"),
                NPSServiceIndex = this.EntityObject.GetTypedColumnValue<int>("TrcNPSServiceIndex"),
                ProductDescription = this.EntityObject.GetTypedColumnValue<string>("TrcProductDescription"),
                ReplacementEquipmentRequired = this.EntityObject.GetTypedColumnValue<bool>("TrcReplacementEquipmentRequired"),
                RequestFailureReason = this.EntityObject.GetTypedColumnValue<string>("TrcRequestFailureReason_Name"),
                RequestStatus = this.EntityObject.GetTypedColumnValue<string>("TrcRequestStatus1C_Description"),
                ServiceDescription = this.EntityObject.GetTypedColumnValue<string>("TrcServiceDescription")
            };

            // Деталь Заключение специалиста
            var expertOpinions = new List<ЗаявкаНаРемонтRequestOpinion>();

            // Дефекты со слов клиента
            var clientDefects = new List<ЗаявкаНаРемонтClientDefect>();

            // Запчасти
            var spareParts = new List<ЗаявкаНаРемонтSparePart>();

            // Услуги
            var services = new List<ЗаявкаНаРемонтService>();

            if (this.RelatedEntitiesData.Count > 0)
            {
                foreach (var item in this.RelatedEntitiesData.Where(e => e.Name == "TrcExpertOpinion").First().EntityCollection)
                {
                    expertOpinions.Add(new ЗаявкаНаРемонтRequestOpinion()
                    {
                        Defect = item.GetTypedColumnValue<string>("TrcDefectFromSpecialist_TrcCode")
                    });
                }

                foreach (var item in this.RelatedEntitiesData.Where(e => e.Name == "TrcClientDefectDetail").First().EntityCollection)
                {
                    clientDefects.Add(new ЗаявкаНаРемонтClientDefect()
                    {
                        DefectsAccordingClient = item.GetTypedColumnValue<string>("TrcDefect_TrcCode")
                    });
                }

                foreach (var item in this.RelatedEntitiesData.Where(e => e.Name == "TrcSparePart").First().EntityCollection)
                {
                    spareParts.Add(new ЗаявкаНаРемонтSparePart()
                    {
                        Spare = item.GetTypedColumnValue<string>("TrcProduct_Trc1CProductID"),
                        Required = item.GetTypedColumnValue<int>("TrcQuantity"),
                        Availability = item.GetTypedColumnValue<int>("TrcAvailability"),
                        Price = item.GetTypedColumnValue<decimal>("TrcPrice"),
                        Paid = this.EntityObject.GetTypedColumnValue<string>("TrcZIPPaymentMethodId").ToLower() == "c540283c-c811-4c16-9144-8ee555bcba8f",
                        SpareAmount = item.GetTypedColumnValue<decimal>("TrcAmount")
                    });
                }

                foreach (var item in this.RelatedEntitiesData.Where(e => e.Name == "TrcService").First().EntityCollection)
                {
                    services.Add(new ЗаявкаНаРемонтService()
                    {
                        Service = item.GetTypedColumnValue<string>("TrcRequestService_Trc1CProductID"),
                        Kol = item.GetTypedColumnValue<int>("TrcQuantity"),
                        Amount = item.GetTypedColumnValue<decimal>("TrcCost"),
                        Paid = item.GetTypedColumnValue<bool>("TrcPaidService"),
                        Content = item.GetTypedColumnValue<string>("TrcContent"),
                        Price = item.GetTypedColumnValue<decimal>("TrcPrice")
                    });
                }

            }

            res.RequestOpinion = expertOpinions.ToArray();
            res.ClientDefect = clientDefects.ToArray();
            res.SparePart = spareParts.ToArray();
            res.Service = services.ToArray();

            return res;
        }
    }
}
