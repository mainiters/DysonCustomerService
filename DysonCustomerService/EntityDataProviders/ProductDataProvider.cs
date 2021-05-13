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
            : base("Product", Id, UserConnection)
        {

        }

        protected override void AddRelatedColumns(EntitySchemaQuery esq, List<RelatedEntitiesData> relatedEntitiesData)
        {
            esq.AddColumn("TrcAdditionalMode.Name");
            esq.AddColumn("AirHumidificationType.Name");
            esq.AddColumn("Category.Name");
            esq.AddColumn("TrcDeviceType.Name");
            esq.AddColumn("TrcDistribution.Name");
            esq.AddColumn("TrcGuarantee.Name");

            relatedEntitiesData.Add(new RelatedEntitiesData()
            {
                Name = "AccountAddress",
                AdditionalColumns = new List<string>()
                {

                }
            });

            base.AddRelatedColumns(esq, relatedEntitiesData);
        }

        public override object GetEntityData(Guid EntityId)
        {
            var res = new Номенклатура()
            {
                AdditionalMode = this.EntityObject.GetTypedColumnValue<string>("TrcAdditionalMode_Name"),
                AddressProduction = this.EntityObject.GetTypedColumnValue<string>("TrcAddressProduction"),
                AirHumidificationType = this.EntityObject.GetTypedColumnValue<string>("TrcAirHumidificationType_Name"),
                AssemblyWeight = this.EntityObject.GetTypedColumnValue<decimal>("TrcAssemblyWeight"),
                CableLengthMeter = this.EntityObject.GetTypedColumnValue<decimal>("TrcCableLengthMeter"),
                Category = this.EntityObject.GetTypedColumnValue<string>("Category_Name"),
                Code = this.EntityObject.GetTypedColumnValue<string>("Code"),
                ContainerVolume = this.EntityObject.GetTypedColumnValue<decimal>("TrcContainerVolume"),
                DeviceType = this.EntityObject.GetTypedColumnValue<string>("TrcDeviceType_Name"),
                Distribution = this.EntityObject.GetTypedColumnValue<string>("TrcDistribution_Name"),
                Guarantee = this.EntityObject.GetTypedColumnValue<string>("TrcGuarantee_Name"),
                HeatingElemenType = this.EntityObject.GetTypedColumnValue<string>("TrcHeatingElemenType_Name"),

                FilterTypes = string.Empty,
            };

            return res;
        }
    }
}
