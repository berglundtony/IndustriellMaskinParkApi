using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace IndustriellMaskinParkApi.Models
{
    public class DataTableEntity: TableEntity
    {
        public string MachineId { get; set; }
        public string MachineName { get; set; }
        public bool Status { get; set; }
        public string Data { get; set; }
        public bool Completed { get; set; }
    }
}
