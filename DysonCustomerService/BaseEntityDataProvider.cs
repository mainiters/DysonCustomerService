using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace DysonCustomerService
{
    public abstract class BaseEntityDataProvider
    {
        public UserConnection UserConnection { get; protected set; }
        public string EntitySchemaName { get; protected set; }
        public Guid EntityId { get; protected set; }

        protected Entity EntityObject { get; set; }

        public BaseEntityDataProvider(string EntitySchemaName, Guid EntityId, UserConnection UserConnection)
        {
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

            this.AddRelatedColumns(esq);

            this.EntityObject = esq.GetEntity(this.UserConnection, this.EntityId);
        }

        protected virtual void AddRelatedColumns(EntitySchemaQuery esq)
        {

        }

        public abstract object GetEntityData(Guid EntityId);
    }
}
