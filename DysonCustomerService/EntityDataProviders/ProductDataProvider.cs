using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace DysonCustomerService.EntityDataProviders
{
    public class ProductDataProvider : BaseEntityDataProvider
    {
        public ProductDataProvider(Guid Id, UserConnection UserConnection)
            : base("Product", Id, UserConnection, "Trc1CProductID")
        {

        }

        protected override void AddRelatedColumns(EntitySchemaQuery esq, List<RelatedEntitiesData> relatedEntitiesData)
        {
            esq.AddColumn("TrcAdditionalMode.Name");
            esq.AddColumn("TrcAirHumidificationType.Name");
            esq.AddColumn("Category.Name");
            esq.AddColumn("TrcDeviceType.Name");
            esq.AddColumn("TrcDistribution.Name");
            esq.AddColumn("TrcGuarantee.Name");
            esq.AddColumn("TrcHeatingElemenType.Name");
            esq.AddColumn("TrcLifeTime.Name");
            esq.AddColumn("TrcOriginCountry.Name");
            esq.AddColumn("TrcPipeType.Name");
            esq.AddColumn("TrcPowerSource.Name");
            esq.AddColumn("TrcProductModel.Name");
            esq.AddColumn("TrcProductSubcategory.TrcCode");
            esq.AddColumn("TradeMark.Name");
            esq.AddColumn("Type.Name");
            esq.AddColumn("TrcProductColor.Name");
            esq.AddColumn("Unit.Name");
            esq.AddColumn("TrcFilterType.Name");
            esq.AddColumn("Category.TrcCode");
            esq.AddColumn("Type.TrcCode");
            esq.AddColumn("TrcDeviceType.TrcCode");

            relatedEntitiesData.Add(new RelatedEntitiesData()
            {
                Name = "DsnProductInBundle",
                FilterFieldName = "DsnBundleProduct",
                AdditionalColumns = new List<string>()
                {
                    "DsnProduct.Trc1CProductID"
                }
            });

            base.AddRelatedColumns(esq, relatedEntitiesData);
        }

        public override object GetEntityData(Guid EntityId)
        {
            var NozzleClumnNames = this.EntityObject.Schema.Columns.Where(e => e.Name.ToUpper().Contains("NOZZLE"));
            var NozzleList = NozzleClumnNames.Where(e => this.EntityObject.GetTypedColumnValue<bool>(e)).Select(e => e.Caption.Value).ToList();
            var NozzleString = string.Join("; ", NozzleList);

            var res = new Номенклатура()
            {
                ID_1С = this.EntityObject.GetTypedColumnValue<string>("Trc1CProductID"),
                WorkName = this.EntityObject.GetTypedColumnValue<string>("TrcWorkName"),
                PrintName = this.EntityObject.GetTypedColumnValue<string>("TrcPrintName"),
                MarkDeletion = this.EntityObject.GetTypedColumnValue<bool>("TrcMarkDeletion"),
                Article = this.EntityObject.GetTypedColumnValue<string>("Code"),
                TradeMark = this.EntityObject.GetTypedColumnValue<string>("TradeMark_Name"),
                CRM_Category = this.EntityObject.GetTypedColumnValue<string>("Type_TrcCode"),
                CRM_DeviceType = this.EntityObject.GetTypedColumnValue<string>("Category_TrcCode"),
                CRM_TypeItem = this.EntityObject.GetTypedColumnValue<string>("TrcDeviceType_TrcCode"),
                ProductSubcategory = this.EntityObject.GetTypedColumnValue<string>("TrcProductSubcategory_TrcCode"),
                UnitID = this.EntityObject.GetTypedColumnValue<string>("Unit_Name"),
                WeightNet = this.EntityObject.GetTypedColumnValue<decimal>("TrcWeight"),
                Height = this.EntityObject.GetTypedColumnValue<decimal>("TrcPackingHeight"),
                Width = this.EntityObject.GetTypedColumnValue<decimal>("TrcPackingWidth"),
                Length = this.EntityObject.GetTypedColumnValue<decimal>("TrcPackingLength"),
                Volume = this.EntityObject.GetTypedColumnValue<decimal>("TrcPackingVolume"),
                WeightGross = this.EntityObject.GetTypedColumnValue<decimal>("TrcProductGrossWeightInPackaging"),
                BundleCode = this.EntityObject.GetTypedColumnValue<string>("TrcBundleCode"),
                Bundle = this.EntityObject.GetTypedColumnValue<bool>("TrcBundle"),
                BundleDate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcBundleExpirationDate")
            };

            var DsnProductInBundle = new List<НоменклатураBundleSet>();

            if (this.RelatedEntitiesData.Count > 0)
            {

                foreach (var item in this.RelatedEntitiesData.Where(e => e.Name == "DsnProductInBundle").First().EntityCollection)
                {
                    DsnProductInBundle.Add(new НоменклатураBundleSet()
                    {
                        Kol = item.GetTypedColumnValue<decimal>("DsnCount"),
                        PartBundlePrice = item.GetTypedColumnValue<decimal>("DsnCost"),
                        TovarCod = item.GetTypedColumnValue<string>("DsnProduct_Trc1CProductID")
                    });
                }

                if (DsnProductInBundle.Count > 0)
                {
                    res.BundleSet = DsnProductInBundle.ToArray();
                }
            }

            return res;
        }
    }
}