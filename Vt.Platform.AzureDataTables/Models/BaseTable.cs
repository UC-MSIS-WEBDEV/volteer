using System;
using Vt.Platform.Utils;
using Microsoft.WindowsAzure.Storage.Table;

namespace Vt.Platform.AzureDataTables.Models
{
    public abstract class BaseTable : TableEntity, IAuditable
    {
        protected BaseTable()
        {
            Version = 1;
            SchemaVersion = 1;
            Created = DateTime.UtcNow;
            Modified = DateTime.UtcNow;
        }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int Version { get; set; }
        public int SchemaVersion { get; set; }
    }
}
