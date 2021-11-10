using System;
using System.Collections.Generic;
using System.Text;

namespace IndustriellMaskinParkApi.Models
{
    public class AddMachineDTO
    {
        public string MachineId { get; set; }
        public string MachineName { get; set; }
        public bool Status { get; set; }
        public string Data { get; set; }
    }
}
