using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace DysonCustomerService
{
    public class RelatedEntitiesData
    {
        public string Name { get; set; }
        public string FilterFieldName { get; set; }
        public EntityCollection EntityCollection { get; set; }
        public List<string> AdditionalColumns { get; set; }
    }

    public abstract class BaseEntityDataProvider
    {
        public UserConnection UserConnection { get; protected set; }
        public string EntitySchemaName { get; protected set; }
        public Guid EntityId { get; protected set; }

        protected Entity EntityObject { get; set; }
        protected List<RelatedEntitiesData> RelatedEntitiesData { get; set; }

        public BaseEntityDataProvider(string EntitySchemaName, Guid EntityId, UserConnection UserConnection)
        {
            this.RelatedEntitiesData = new List<RelatedEntitiesData>();

            this.UserConnection = UserConnection;
            this.EntityId = EntityId;
            this.Initialize();
        }

        protected virtual void Initialize()
        {
            EntitySchema schema = this.UserConnection.EntitySchemaManager.GetInstanceByName(this.EntitySchemaName);

            EntitySchemaQuery esq = new EntitySchemaQuery(schema)
            {
                UseAdminRights = true,
                CanReadUncommitedData = true,
                IgnoreDisplayValues = true
            };

            esq.AddAllSchemaColumns();

            this.AddRelatedColumns(esq, this.RelatedEntitiesData);

            this.EntityObject = esq.GetEntity(this.UserConnection, this.EntityId);

            foreach (var item in RelatedEntitiesData)
            {
                EntitySchema relatedSchema = this.UserConnection.EntitySchemaManager.GetInstanceByName(item.Name);

                EntitySchemaQuery relatedEsq = new EntitySchemaQuery(relatedSchema)
                {
                    UseAdminRights = true,
                    CanReadUncommitedData = true,
                    IgnoreDisplayValues = true
                };

                relatedEsq.Filters.Add(relatedEsq.CreateFilterWithParameters(FilterComparisonType.Equal, string.IsNullOrEmpty(item.FilterFieldName) ?  this.EntitySchemaName : item.FilterFieldName, this.EntityId));

                relatedEsq.AddAllSchemaColumns();

                if(item.AdditionalColumns != null)
                {
                    foreach (var additionalColumn in item.AdditionalColumns)
                    {
                        relatedEsq.AddColumn(additionalColumn);
                    }
                }
                
                item.EntityCollection = relatedEsq.GetEntityCollection(this.UserConnection);
            }
        }

        protected virtual void AddRelatedColumns(EntitySchemaQuery esq, List<RelatedEntitiesData> relatedEntitiesData)
        {

        }

        public abstract object GetEntityData(Guid EntityId);
    }
}
