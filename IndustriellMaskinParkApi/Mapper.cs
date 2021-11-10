using IndustriellMaskinParkApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IndustriellMaskinParkApi
{
    public static class Mapper
    {
        public static DataTableEntity ToTableEntity(this IndustrialMachinepark machinpark)
        {
            return new DataTableEntity
            {
                MachineName = machinpark.MachineName,
                Status = machinpark.Status,
                Data = machinpark.Data,
                PartitionKey = "Data",
                RowKey = machinpark.MachineId
            };
        }

        public static IndustrialMachinepark ToMachine(this DataTableEntity dataTable)
        {
            return new IndustrialMachinepark
            {
                MachineId = dataTable.RowKey,
                MachineName = dataTable.MachineName,
                Status = dataTable.Status,
                Data = dataTable.Data
            };
        }
    }
}
