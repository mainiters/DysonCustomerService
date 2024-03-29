﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace DysonCustomerService.EntityDataProviders
{
    public class ProductTypeDataProvider : BaseEntityDataProvider
    {
        public ProductTypeDataProvider(Guid Id, UserConnection UserConnection)
            : base("ProductType", Id, UserConnection, "TrcCode")
        {

        }

        protected override void AddRelatedColumns(EntitySchemaQuery esq, List<RelatedEntitiesData> relatedEntitiesData)
        {
            esq.AddColumn("TrcProductSubcategory.TrcCode");

            base.AddRelatedColumns(esq, relatedEntitiesData);
        }

        public override object GetEntityData(Guid EntityId)
        {
            var res = new Категории()
            {
                Name = this.EntityObject.GetTypedColumnValue<string>("Name"),
                Description = this.EntityObject.GetTypedColumnValue<string>("Description"),
                MarkDeletion = this.EntityObject.GetTypedColumnValue<bool>("TrcMarkDeletion"),
                ID_1С = this.EntityObject.GetTypedColumnValue<string>("TrcCode"),
            };

            var equipCode = this.EntityObject.GetTypedColumnValue<string>("TrcProductSubcategory_TrcCode");

            if(!string.IsNullOrEmpty(equipCode))
            {
                res.EquipCode = new КатегорииEquipCode[]
                {
                    new КатегорииEquipCode() { EquipmentCode = equipCode }
                };
            }

            return res;
        }
    }
}
