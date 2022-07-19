using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Linq;

namespace TulipInfo.Net.EFCore
{
    public interface IDbContextTableMapping
    {
        IEntityTableMapping GetEntityTableMapping<EntityType>();
    }

    public class DbContextTableMapping :IDbContextTableMapping
    {
        static readonly ConcurrentDictionary<string, IEntityTableMapping> TableMapping = new ConcurrentDictionary<string, IEntityTableMapping>();
        
        DbContext _dbContext;
        string _keyPrefix;
        public DbContextTableMapping(DbContext dbContext)
        {
            _dbContext = dbContext;
            _keyPrefix = dbContext.GetType().FullName;
        }

        public EntityTableMapping<EntityType> GetEntityTableMapping<EntityType>()
        {
            Type typeOfEntity = typeof(EntityType);
            string entityFullName = typeOfEntity.FullName;
            string key = _keyPrefix + "_" + entityFullName;
            if (TableMapping.ContainsKey(key))
            {
                return TableMapping[key] as EntityTableMapping<EntityType>;
            }
            else
            {
                var entityType = _dbContext.Model.FindEntityType(entityFullName);
                if (entityType == null)
                {
                    throw new NotSupportedException($"NonEntity:{entityFullName}");
                }

                var tableMapping = entityType.GetTableMappings().FirstOrDefault();
                if (tableMapping == null)
                {
                    throw new NotSupportedException($"NonEntityTableMapping:{entityFullName},do not have table mapping");
                }

                EntityTableMapping<EntityType> mapping = new EntityTableMapping<EntityType>(tableMapping);

                TableMapping.TryAdd(key, mapping);

                return mapping;
            }
        }

        IEntityTableMapping IDbContextTableMapping.GetEntityTableMapping<EntityType>()
        {
            return GetEntityTableMapping<EntityType>();
        }
    }
}
