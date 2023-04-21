using Azure.Data.Tables;
using MvcCoreSasStorage.Models;
using Newtonsoft.Json.Linq;
using System.Net;

namespace MvcCoreSasStorage.Services
{
    public class ServiceAzureAlumnos
    {
        //NECESITAMOS LA TABLA DE ALUMNOS
        private TableClient tablaAlumnos;
        //NECESITAMOS LA URL DEL API PARA EL TOKEN
        private string UrlApi;

        public ServiceAzureAlumnos(IConfiguration configuration)
        {
            //EN CONFIGURATION TENDREMOS LA URL DEL API
            this.UrlApi = configuration.GetValue<string>
                ("ApiUrls:ApiTableTokens");
        }

        public async Task<List<Alumno>> GetAlumnosAsync(string token)
        {
            Uri uriToken = new Uri(token);
            //SE ACCEDE A LA TABLA CON DICHO TOKEN
            this.tablaAlumnos = new TableClient(uriToken);
            //LEEMOS LOS DATOS DE LOS ALUMNOS
            List<Alumno> alumnos = new List<Alumno>();
            var consulta = this.tablaAlumnos.QueryAsync<Alumno>
                (filter: "");
            await foreach (Alumno al in consulta)
            {
                alumnos.Add(al);
            }
            return alumnos;
        }

        //NECESITAMOS UN METODO PARA PEDIR EL TOKEN DE ACCESO
        //MEDIANTE EL CURSO
        public async Task<string> GetTokenAsync(string curso)
        {
            using (WebClient client = new WebClient())
            {
                string request =
                    "api/tabletoken/generatetoken/" + curso;
                client.Headers["content-type"] = "application/json";
                Uri uri = new Uri(this.UrlApi + request);
                string data = await client.DownloadStringTaskAsync(uri);
                //NECESITAMOS MANEJAR EL JSON DE FORMA MANUAL
                //DEBIDO A QUE NO TENEMOS NINGUN MODELO
                JObject objetoJSON = JObject.Parse(data);
                string token = objetoJSON.GetValue("token").ToString();
                return token;
            }
        }

    }
}

