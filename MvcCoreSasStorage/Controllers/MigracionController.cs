using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using MvcCoreSasStorage.Helpers;
using MvcCoreSasStorage.Models;

namespace MvcCoreSasStorage.Controllers
{
    public class MigracionController : Controller
    {
        private HelperXML helper;

        public MigracionController(HelperXML helper)
        {
            this.helper = helper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string algo)
        {
            string azureKeys =
                "DefaultEndpointsProtocol=https;AccountName=storageaccountxiang;AccountKey=GYsOg9jYSu/rO2TrV9oCZd5uNFHELJttd1HZJ9Kknz3ntofI4AdRiLOjwTIP5MzHTCRFxp9Y+Yme+ASttcFqbA==;EndpointSuffix=core.windows.net";
            //string azureKeys =
            //    "UseDevelopmentStorage=true";
            TableServiceClient tableServiceClient =
                new TableServiceClient(azureKeys);
            TableClient tableClient =
                tableServiceClient.GetTableClient("alumnos");
            await tableClient.CreateIfNotExistsAsync();
            List<Alumno> alumnos = this.helper.GetAlumnosXML();
            foreach (Alumno alumn in alumnos)
            {
                await tableClient.AddEntityAsync<Alumno>(alumn);
            }
            ViewData["MENSAJE"] = "Migración de alumnos correcta";
            return View();
        }
    }
}
