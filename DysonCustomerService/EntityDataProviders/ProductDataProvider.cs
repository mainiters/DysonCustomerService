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
            esq.AddColumn("TrcProductSubcategory.Name");
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
                FilterFieldName = "DsnProduct",
                AdditionalColumns = new List<string>()
                {
                    "DsnProduct.Code"
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
                Article = this.EntityObject.GetTypedColumnValue<string>("Code"),
                PrintName = this.EntityObject.GetTypedColumnValue<string>("TrcPrintName"),
                ProductSubcategory = this.EntityObject.GetTypedColumnValue<string>("TrcProductSubcategory_Name"),
                TradeMark = this.EntityObject.GetTypedColumnValue<string>("TradeMark_Name"),
                MarkDeletion = this.EntityObject.GetTypedColumnValue<bool>("TrcMarkDeletion"),
                UnitID = this.EntityObject.GetTypedColumnValue<string>("Unit_Name"),
                ID_1С = this.EntityObject.GetTypedColumnValue<string>("Trc1CProductID"),
                WorkName = this.EntityObject.GetTypedColumnValue<string>("TrcWorkName"),

                BundleCode = this.EntityObject.GetTypedColumnValue<string>("TrcBundleCode"),
                Bundle = this.EntityObject.GetTypedColumnValue<bool>("TrcBundle"),
                BundleDate = this.EntityObject.GetTypedColumnValue<DateTime>("TrcBundleExpirationDate"),
                CRM_Category = this.EntityObject.GetTypedColumnValue<string>("Category_TrcCode"),
                CRM_DeviceType = this.EntityObject.GetTypedColumnValue<string>("TrcDeviceType_TrcCode"),
                CRM_TypeItem = this.EntityObject.GetTypedColumnValue<string>("Type_TrcCode")

                //AdditionalMode = this.EntityObject.GetTypedColumnValue<string>("TrcAdditionalMode_Name"),
                //AddressProduction = this.EntityObject.GetTypedColumnValue<string>("TrcAddressProduction"),
                //AirHumidificationType = this.EntityObject.GetTypedColumnValue<string>("TrcAirHumidificationType_Name"),
                //AssemblyWeight = this.EntityObject.GetTypedColumnValue<decimal>("TrcAssemblyWeight"),
                //CableLengthMeter = this.EntityObject.GetTypedColumnValue<decimal>("TrcCableLengthMeter"),
                //Category = string.Empty,
                //ContainerVolume = this.EntityObject.GetTypedColumnValue<decimal>("TrcContainerVolume"),
                //DeviceType = this.EntityObject.GetTypedColumnValue<string>("TrcDeviceType_Name"),
                //Distribution = this.EntityObject.GetTypedColumnValue<string>("TrcDistribution_Name"),
                //Guarantee = this.EntityObject.GetTypedColumnValue<string>("TrcGuarantee_Name"),
                //HeatingElemenType = this.EntityObject.GetTypedColumnValue<string>("TrcHeatingElemenType_Name"),
                //IssueYear = this.EntityObject.GetTypedColumnValue<string>("TrcIssueYear"),
                //LifeTime = this.EntityObject.GetTypedColumnValue<string>("TrcLifeTime_Name"),
                //MaxTemperature = this.EntityObject.GetTypedColumnValue<decimal>("TrcMaxTemperature"),
                //MaxWaterConsumption = this.EntityObject.GetTypedColumnValue<decimal>("TrcMaxWaterConsumption"),
                //MinTemperature = this.EntityObject.GetTypedColumnValue<decimal>("TrcMinTemperature"),
                //Name = this.EntityObject.GetTypedColumnValue<string>("Name"),
                //NoiseLevelWithoutPipeEco = this.EntityObject.GetTypedColumnValue<decimal>("TrcNoiseLevelWithoutPipeEco"),
                //NoiseLevelWithoutPipeTurbo = this.EntityObject.GetTypedColumnValue<decimal>("TrcNoiseLevelWithoutPipeTurbo"),
                //NoiseLevelWithPipeEco = this.EntityObject.GetTypedColumnValue<decimal>("TrcNoiseLevelWithPipeEco"),
                //NoiseLevelWithPipeTurbo = this.EntityObject.GetTypedColumnValue<decimal>("TrcNoiseLevelWithPipeTurbo"),
                //NumberOfAirFlowRates = this.EntityObject.GetTypedColumnValue<int>("TrcNumberOfAirFlowRates"),
                //OptimalVisitingArea = this.EntityObject.GetTypedColumnValue<decimal>("TrcOptimalVisitingArea"),
                //OriginCountry = this.EntityObject.GetTypedColumnValue<string>("TrcOriginCountry_Name"),
                //PackingHeight = this.EntityObject.GetTypedColumnValue<decimal>("TrcPackingHeight"),
                //PackingLength = this.EntityObject.GetTypedColumnValue<decimal>("TrcPackingLength"),
                //PackingVolume = this.EntityObject.GetTypedColumnValue<decimal>("TrcPackingVolume"),
                //PackingWidth = this.EntityObject.GetTypedColumnValue<decimal>("TrcPackingWidth"),
                //Personalization = this.EntityObject.GetTypedColumnValue<bool>("TrcPersonalization"),
                //PipeType = this.EntityObject.GetTypedColumnValue<string>("TrcPipeType_Name"),
                //PowerSource = this.EntityObject.GetTypedColumnValue<string>("TrcPowerSource_Name"),
                //ProductColor = this.EntityObject.GetTypedColumnValue<string>("TrcProductColor_Name"),
                //ProductGrossWeightInPackaging = this.EntityObject.GetTypedColumnValue<decimal>("TrcProductGrossWeightInPackaging"),
                //ProductModel = this.EntityObject.GetTypedColumnValue<string>("TrcProductModel_Name"),
                //ProductPowerConsumption = this.EntityObject.GetTypedColumnValue<decimal>("TrcProductPowerConsumption"),
                //SuctionPowerAuto = this.EntityObject.GetTypedColumnValue<decimal>("TrcSuctionPowerAuto"),
                //SuctionPowerEco = this.EntityObject.GetTypedColumnValue<decimal>("TrcSuctionPowerEco"),
                //SuctionPowerTurbo = this.EntityObject.GetTypedColumnValue<decimal>("TrcSuctionPowerTurbo"),
                //TemperatureMode = this.EntityObject.GetTypedColumnValue<int>("TrcTemperatureMode"),
                //Type = string.Empty,
                //WaterTankVolume = this.EntityObject.GetTypedColumnValue<decimal>("TrcWaterTankVolume"),
                //Weight = this.EntityObject.GetTypedColumnValue<decimal>("TrcWeight"),
                //FilterTypes = this.EntityObject.GetTypedColumnValue<string>("TrcFilterType_Name"),
                //Nozzle = NozzleString,
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
                        TovarCod = item.GetTypedColumnValue<string>("DsnProduct_Code")
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