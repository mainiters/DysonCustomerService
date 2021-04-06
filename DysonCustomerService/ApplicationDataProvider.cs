using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace DysonCustomerService
{
    public class ApplicationDataProvider : BaseEntityDataProvider
    {
        public ApplicationDataProvider(Guid Id, UserConnection UserConnection)
            : base("TrcApplication", Id, UserConnection)
        {

        }

        protected override void AddRelatedColumns(EntitySchemaQuery esq, List<RelatedEntitiesData> relatedEntitiesData)
        {
            esq.AddColumn("TrcOrderState.Name");
            esq.AddColumn("TrcServiceCenter.Name");
            esq.AddColumn("TrcRepairWarehouse.TrcCode");
            esq.AddColumn("TrcRepairType.Name");
            esq.AddColumn("TrcProduct.Code");
            esq.AddColumn("SerialNumberHistory.Name");
            esq.AddColumn("Engineer.Name");
            esq.AddColumn("TrcWarrantyType.Name");

            relatedEntitiesData.Add(new DysonCustomerService.RelatedEntitiesData()
            {
                Name = "TrcExpertOpinion",
                FilterFieldName = "TrcRequest",
                AdditionalColumns = new List<string>()
            });

            relatedEntitiesData.Add(new DysonCustomerService.RelatedEntitiesData()
            {
                Name = "TrcClientDefectDetail",
                FilterFieldName = "TrcRequest",
                AdditionalColumns = new List<string>()
                {
                    "TrcDefect.Name"
                }
            });

            relatedEntitiesData.Add(new DysonCustomerService.RelatedEntitiesData()
            {
                Name = "TrcSparePart",
                FilterFieldName = "TrcRequest",
                AdditionalColumns = new List<string>()
            });

            relatedEntitiesData.Add(new DysonCustomerService.RelatedEntitiesData()
            {
                Name = "TrcService",
                FilterFieldName = "TrcRequest",
                AdditionalColumns = new List<string>()
                {
                    "TrcRequestService.Name"
                }
            });
            

            base.AddRelatedColumns(esq, relatedEntitiesData);
        }

        public override object GetEntityData(Guid EntityId)
        {
            var res = new ПакетЗаявок();

            res.ID_Pack = new Guid().ToString();

            // Данные заказа
            res.Request = new ЗаявкаНаРемонт[]
            {
                new ЗаявкаНаРемонт()
                {
                    DocumentNumber = this.EntityObject.GetTypedColumnValue<string>("TrcNumber"),
                    CreateDate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcCreationDate"),
                    Organization = this.EntityObject.GetTypedColumnValue<string>("TrcServiceCenter_Name"),
                    WarehouseCode = this.EntityObject.GetTypedColumnValue<string>("TrcRepairWarehouse_TrcCode"),
                    ServiceOption = this.EntityObject.GetTypedColumnValue<string>("TrcServiceOption"),
                    TypeRepair = this.EntityObject.GetTypedColumnValue<string>("TrcRepairType_Name"),
                    Article = this.EntityObject.GetTypedColumnValue<string>("TrcProduct_Code"),
                    SN = this.EntityObject.GetTypedColumnValue<string>("SerialNumberHistory_Name"),
                    DateDeparture = this.EntityObject.GetTypedColumnValue<DateTime>("TrcDepartureDate"),
                    Master = this.EntityObject.GetTypedColumnValue<string>("Engineer_Name"),
                    DatePurchase = this.EntityObject.GetTypedColumnValue<DateTime>("TrcPurchaseDate"),
                    TypeGuarantee = this.EntityObject.GetTypedColumnValue<string>("TrcWarrantyType_Name"),
                    RenewalWarranty = this.EntityObject.GetTypedColumnValue<bool>("TrcIsWarrantyRenewed"),
                    ActualDateRepairExecution = this.EntityObject.GetTypedColumnValue<DateTime>("TrcRepairDate"),
                    InformationClient = this.EntityObject.GetTypedColumnValue<string>("TrcAdditionalInformation"),
                    InternalComment = this.EntityObject.GetTypedColumnValue<string>("TrcInternalComment"),
                    ViolationOperation  = this.EntityObject.GetTypedColumnValue<bool>("TrcViolationOfExploitation")
                }
            };

            if (this.RelatedEntitiesData.Count > 0)
            {
                // Деталь Заключение специалиста
                var expertOpinions = new List<ЗаявкаНаРемонтDefects>();

                foreach (var item in this.RelatedEntitiesData.Where(e => e.Name == "TrcExpertOpinion").First().EntityCollection)
                {
                    expertOpinions.Add(new ЗаявкаНаРемонтDefects()
                    {
                        Defect = item.GetTypedColumnValue<string>("TrcDefect")
                    });
                }

                res.Request.First().Defects = expertOpinions.ToArray();

                // Дефекты со слов клиента
                var clientDefects = new List<ЗаявкаНаРемонтDefectsAccordingClient>();

                foreach (var item in this.RelatedEntitiesData.Where(e => e.Name == "TrcClientDefectDetail").First().EntityCollection)
                {
                    clientDefects.Add(new ЗаявкаНаРемонтDefectsAccordingClient()
                    {
                        DefectAccordingClient = item.GetTypedColumnValue<string>("TrcDefect_Name")
                    });
                }

                res.Request.First().DefectsAccordingClient = clientDefects.ToArray();

                // Запчасти
                var spareParts = new List<ЗаявкаНаРемонтSpares>();

                foreach (var item in this.RelatedEntitiesData.Where(e => e.Name == "TrcSparePart").First().EntityCollection)
                {
                    spareParts.Add(new ЗаявкаНаРемонтSpares()
                    {
                        Spare = item.GetTypedColumnValue<string>("TrcVendorCode"),
                        Required = item.GetTypedColumnValue<int>("TrcQuantity"),
                        Availability = item.GetTypedColumnValue<int>("TrcAvailability"),
                        Price = item.GetTypedColumnValue<decimal>("TrcPrice")
                    });
                }

                res.Request.First().Spares = spareParts.ToArray();

                // Услуги
                var services = new List<ЗаявкаНаРемонтServices>();

                foreach (var item in this.RelatedEntitiesData.Where(e => e.Name == "TrcSparePart").First().EntityCollection)
                {
                    services.Add(new ЗаявкаНаРемонтServices()
                    {
                        Service = item.GetTypedColumnValue<string>("TrcRequestService_Name"),
                        Kol = item.GetTypedColumnValue<int>("TrcQuantity"),
                        Amount = item.GetTypedColumnValue<decimal>("TrcCost"),
                        Paid = item.GetTypedColumnValue<bool>("TrcPaidService"),
                        Content = item.GetTypedColumnValue<string>("TrcContent"),
                        Price = item.GetTypedColumnValue<decimal>("TrcPrice")
                    });
                }

                res.Request.First().Services = services.ToArray();
            }

            return res;
        }
    }
}
