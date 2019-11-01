using System;
using System.Collections.Generic;
using System.Text;

namespace Vt.Platform.AzureDataTables.Models
{
    public class TestContactTable : BaseTable
    {
        //PartitionId will be "Contact"
        //RowId will be the Code
        public string Name { get; set; }
        public string EmailToken { get; set; }
        public string EmailHash { get; set; }
    }
}
