using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table.Queryable;
using IndustriellMaskinParkApi.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;

namespace IndustriellMaskinParkApi
{
    [EnableCors]
    public static class MaskinParkApi
    {
        [EnableCors]
        [FunctionName("AddMachine")]
        public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous,"post", Route = "maskinpark")] HttpRequest req,
        [Table("maskinpark", Connection = "AzureWebJobsStorage")] IAsyncCollector<DataTableEntity> dataTable,
        ILogger log)
        {
            log.LogInformation("Add machine");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<AddMachineDTO>(requestBody);
            data.MachineId = Guid.NewGuid().ToString("n");

            if (data is null || string.IsNullOrWhiteSpace(data?.Data)) return new BadRequestResult();
            var imp = new IndustrialMachinepark {MachineId = data.MachineId, MachineName = data.MachineName, Data = data.Data, Status = data.Status};

            await dataTable.AddAsync(imp.ToTableEntity());

            return new OkObjectResult(imp);
        }
        [EnableCors]
        [FunctionName("Put")]
        public static async Task<IActionResult> Put(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "maskinpark/{MachineId}")] HttpRequest req,
            [Table("maskinpark", Connection = "AzureWebJobsStorage")] CloudTable machineTable,
            string machineid,
            ILogger log)
            {
            log.LogInformation("Update data by send command.");
            var query = new TableQuery<DataTableEntity>();
            var result = await machineTable.ExecuteQuerySegmentedAsync(query, null);
            var machine = result.Select(Mapper.ToMachine).Where(m => m.MachineId == machineid).FirstOrDefault();
      /*      var response = new IndustrialMachinepark { MachineName = machine.MachineName, Status = machine.Status  }*/;
      
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updateData = JsonConvert.DeserializeObject<IndustrialMachinepark>(requestBody);
            updateData.MachineName = machine.MachineName;
            updateData.Status = machine.Status;

            if (updateData is null || updateData.MachineId != machineid) return new BadRequestResult();

            var dataTable = updateData.ToTableEntity();
            dataTable.ETag = "*";
            var operation = TableOperation.Replace(dataTable);
            await machineTable.ExecuteAsync(operation);

            return new NoContentResult();
        }

        [EnableCors]
        [FunctionName("Get")]
        public static async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Maskinpark")] HttpRequest req,
            [Table("maskinpark", Connection = "AzureWebJobsStorage")] CloudTable machineTable,
            ILogger log)
            {
                log.LogInformation("Get all machines");

                var query = new TableQuery<DataTableEntity>();
                var result = await machineTable.ExecuteQuerySegmentedAsync(query, null);
                var response = new IndustrialMachines { Machines = result.Select(Mapper.ToMachine).ToList() };

                return new OkObjectResult(response);
            }
    }
}
