using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace TulipInfo.Net.EFCore
{
    public interface IEntityTableMapping
    {
        string EntityFullName { get; }
        string TableFullName { get; }
        IColumn GetColumn(string propertyName);
    }
    public class EntityTableMapping<EntityType> : IEntityTableMapping
    {
        public EntityTableMapping(
            ITableMapping tableMapping)
        {
            this.EntityFullName =typeof(EntityType).FullName;

            //table name
            string tableName = "";
            var table = tableMapping.Table;
            if (!string.IsNullOrWhiteSpace(table.Schema))
            {
                tableName += table.Schema + ".";
            }
            tableName += table.Name;
            this.TableFullName = tableName;

            //columns
            this.Columns = new Dictionary<string, IColumn>();
            foreach (var colMapping in tableMapping.ColumnMappings)
            {
                this.Columns.Add(colMapping.Property.Name.ToLower(),
                    colMapping.Column);
            }
        }

        public string EntityFullName { get;private set; }
        public string TableFullName { get;private set; }

        public IDictionary<string, IColumn> Columns { get;private set; }

        public IColumn GetColumn(string propertyName)
        {
            string lowerPropertyName = propertyName.ToLower();
            if (Columns.ContainsKey(lowerPropertyName))
            {
                return Columns[lowerPropertyName];
            }
            else
            {
                throw new KeyNotFoundException($"NonColumn:{propertyName}");
            }
        }
    }
}
