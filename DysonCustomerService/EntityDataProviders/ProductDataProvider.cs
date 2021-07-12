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
           // esq.AddColumn("Category.TrcCode");
            //esq.AddColumn("Type.TrcCode");
            //esq.AddColumn("TrcDeviceType.TrcCode");
            base.AddRelatedColumns(esq, relatedEntitiesData);
        }

        public override object GetEntityData(Guid EntityId)
        {
            var NozzleClumnNames = this.EntityObject.Schema.Columns.Where(e => e.Name.ToUpper().Contains("NOZZLE"));
            var NozzleList = NozzleClumnNames.Where(e => this.EntityObject.GetTypedColumnValue<bool>(e)).Select(e => e.Caption.Value).ToList();
            var NozzleString = string.Join("; ", NozzleList);

            var res = new Номенклатура()
            {
                AdditionalMode = this.EntityObject.GetTypedColumnValue<string>("TrcAdditionalMode_Name"),
                AddressProduction = this.EntityObject.GetTypedColumnValue<string>("TrcAddressProduction"),
                AirHumidificationType = this.EntityObject.GetTypedColumnValue<string>("TrcAirHumidificationType_Name"),
                AssemblyWeight = this.EntityObject.GetTypedColumnValue<decimal>("TrcAssemblyWeight"),
                CableLengthMeter = this.EntityObject.GetTypedColumnValue<decimal>("TrcCableLengthMeter"),
                Category = string.Empty,
                Article = this.EntityObject.GetTypedColumnValue<string>("Code"),
                ContainerVolume = this.EntityObject.GetTypedColumnValue<decimal>("TrcContainerVolume"),
                DeviceType = this.EntityObject.GetTypedColumnValue<string>("TrcDeviceType_Name"),
                Distribution = this.EntityObject.GetTypedColumnValue<string>("TrcDistribution_Name"),
                Guarantee = this.EntityObject.GetTypedColumnValue<string>("TrcGuarantee_Name"),
                HeatingElemenType = this.EntityObject.GetTypedColumnValue<string>("TrcHeatingElemenType_Name"),
                IssueYear = this.EntityObject.GetTypedColumnValue<string>("TrcIssueYear"),
                LifeTime = this.EntityObject.GetTypedColumnValue<string>("TrcLifeTime_Name"),
                MaxTemperature = this.EntityObject.GetTypedColumnValue<decimal>("TrcMaxTemperature"),
                MaxWaterConsumption = this.EntityObject.GetTypedColumnValue<decimal>("TrcMaxWaterConsumption"),
                MinTemperature = this.EntityObject.GetTypedColumnValue<decimal>("TrcMinTemperature"),
                Name = this.EntityObject.GetTypedColumnValue<string>("Name"),
                NoiseLevelWithoutPipeEco = this.EntityObject.GetTypedColumnValue<decimal>("TrcNoiseLevelWithoutPipeEco"),
                NoiseLevelWithoutPipeTurbo = this.EntityObject.GetTypedColumnValue<decimal>("TrcNoiseLevelWithoutPipeTurbo"),
                NoiseLevelWithPipeEco = this.EntityObject.GetTypedColumnValue<decimal>("TrcNoiseLevelWithPipeEco"),
                NoiseLevelWithPipeTurbo = this.EntityObject.GetTypedColumnValue<decimal>("TrcNoiseLevelWithPipeTurbo"),
                NumberOfAirFlowRates = this.EntityObject.GetTypedColumnValue<int>("TrcNumberOfAirFlowRates"),
                OptimalVisitingArea = this.EntityObject.GetTypedColumnValue<decimal>("TrcOptimalVisitingArea"),
                OriginCountry = this.EntityObject.GetTypedColumnValue<string>("TrcOriginCountry_Name"),
                PackingHeight = this.EntityObject.GetTypedColumnValue<decimal>("TrcPackingHeight"),
                PackingLength = this.EntityObject.GetTypedColumnValue<decimal>("TrcPackingLength"),
                PackingVolume = this.EntityObject.GetTypedColumnValue<decimal>("TrcPackingVolume"),
                PackingWidth = this.EntityObject.GetTypedColumnValue<decimal>("TrcPackingWidth"),
                Personalization = this.EntityObject.GetTypedColumnValue<bool>("TrcPersonalization"),
                PipeType = this.EntityObject.GetTypedColumnValue<string>("TrcPipeType_Name"),
                PowerSource = this.EntityObject.GetTypedColumnValue<string>("TrcPowerSource_Name"),
                PrintName = this.EntityObject.GetTypedColumnValue<string>("TrcPrintName"),
                ProductColor = this.EntityObject.GetTypedColumnValue<string>("TrcProductColor_Name"),
                ProductGrossWeightInPackaging = this.EntityObject.GetTypedColumnValue<decimal>("TrcProductGrossWeightInPackaging"),
                ProductModel = this.EntityObject.GetTypedColumnValue<string>("TrcProductModel_Name"),
                ProductPowerConsumption = this.EntityObject.GetTypedColumnValue<decimal>("TrcProductPowerConsumption"),
                ProductSubcategory = this.EntityObject.GetTypedColumnValue<string>("TrcProductSubcategory_Name"),
                SuctionPowerAuto = this.EntityObject.GetTypedColumnValue<decimal>("TrcSuctionPowerAuto"),
                SuctionPowerEco = this.EntityObject.GetTypedColumnValue<decimal>("TrcSuctionPowerEco"),
                SuctionPowerTurbo = this.EntityObject.GetTypedColumnValue<decimal>("TrcSuctionPowerTurbo"),
                TemperatureMode = this.EntityObject.GetTypedColumnValue<int>("TrcTemperatureMode"),
                TradeMark = this.EntityObject.GetTypedColumnValue<string>("TradeMark_Name"),
                MarkDeletion = this.EntityObject.GetTypedColumnValue<bool>("TrcMarkDeletion"),
                Type = string.Empty,
                UnitID = this.EntityObject.GetTypedColumnValue<string>("Unit_Name"),
                WaterTankVolume = this.EntityObject.GetTypedColumnValue<decimal>("TrcWaterTankVolume"),
                Weight = this.EntityObject.GetTypedColumnValue<decimal>("TrcWeight"),
                WorkName = this.EntityObject.GetTypedColumnValue<string>("TrcWorkName"),
                ID_1С = this.EntityObject.GetTypedColumnValue<string>("Trc1CProductID"),
                FilterTypes = this.EntityObject.GetTypedColumnValue<string>("TrcFilterType_Name"),
                Nozzle = NozzleString,
                //Category_1C = this.EntityObject.GetTypedColumnValue<string>("Category_TrcCode"),
                //DeviceType_1C = this.EntityObject.GetTypedColumnValue<string>("TrcDeviceType_TrcCode"),
                //TypeOfItem_1C = this.EntityObject.GetTypedColumnValue<string>("Type_TrcCode")
            };

            return res;
        }
    }
}