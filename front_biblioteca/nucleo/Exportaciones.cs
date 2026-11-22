using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace front_biblioteca.nucleo
{
    public static class Exportaciones
    {
        public static FileContentResult Csv(
            string nombreArchivo,
            IEnumerable<string> encabezados,
            IEnumerable<IEnumerable<object?>> filas)
        {
            var texto = new StringBuilder();
            texto.AppendLine(string.Join(";", encabezados.Select(Escapar)));

            foreach (var fila in filas)
                texto.AppendLine(string.Join(";", fila.Select(x => Escapar(x?.ToString() ?? ""))));

            var contenido = Encoding.UTF8.GetBytes(texto.ToString());
            var bytes = Encoding.UTF8.GetPreamble().Concat(contenido).ToArray();

            return new FileContentResult(bytes, "text/csv; charset=utf-8")
            {
                FileDownloadName = nombreArchivo
            };
        }

        private static string Escapar(string valor)
        {
            var texto = valor.Replace("\"", "\"\"");
            return $"\"{texto}\"";
        }
    }
}
