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
            //        string azureKeys =
            //"DefaultEndpointsProtocol=https;AccountName=storageaccountpaco;AccountKey=A5IdMIsgAdfHuO46U8QdbruPy0PXfILvLTW29uTxTth7bqOHiOrRuTYCdQrW+OC48J0hO41t059P+AStEBO4mA==;EndpointSuffix=core.windows.net";
            string azureKeys =
                "UseDevelopmentStorage=true";
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
