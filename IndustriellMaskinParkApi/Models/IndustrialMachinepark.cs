using System;
using System.Collections.Generic;
using System.Text;

namespace IndustriellMaskinParkApi.Models
{
    public class IndustrialMachinepark
    {
        public string MachineName { get; set; }
        public string MachineId { get; set; }
        public bool Status { get; set; }
        public string Data { get; set; }

        public IndustrialMachinepark()
        {
            MachineId = Guid.NewGuid().ToString("n");
        }
    }

}
