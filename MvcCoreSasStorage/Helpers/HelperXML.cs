using MvcCoreSasStorage.Models;
using System.Xml.Linq;

namespace MvcCoreSasStorage.Helpers
{
    public class HelperXML
    {
        public List<Alumno> GetAlumnosXML()
        {
            string assemblyPath = 
                "MvcCoreSasStorage.Documents.alumnos_tables.xml";
            Stream stream =
                this.GetType().Assembly.GetManifestResourceStream(assemblyPath);
            XDocument document = XDocument.Load(stream);
            var consulta = from datos in document.Descendants("alumno")
                           select new Alumno
                           {
                               IdAlumno = 
                               int.Parse(datos.Element("idalumno").Value),
                               Nombre = datos.Element("nombre").Value,
                               Apellidos = datos.Element("apellidos").Value,
                               Curso = datos.Element("curso").Value,
                               Nota = 
                               int.Parse(datos.Element("nota").Value)
                           };
            return consulta.ToList();
        }
    }
}
