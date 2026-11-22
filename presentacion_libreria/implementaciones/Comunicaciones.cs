using System.Text;
using Newtonsoft.Json;
using presentacion_libreria.interfaces;
using presentacion_libreria.nucleo;

namespace presentacion_libreria.implementaciones
{
    public class Comunicaciones : IComunicaciones
    {
        private readonly HttpClient cliente;

        public Comunicaciones(string urlBase = DatosGenerales.UrlApi)
        {
            this.cliente = new HttpClient
            {
                BaseAddress = new Uri(urlBase),
                Timeout = TimeSpan.FromMinutes(4)
            };
        }

        public async Task<T> Ejecutar<T>(string url, object? entidad = null)
        {
            var cuerpo = new StringContent(
                JsonConvert.SerializeObject(entidad ?? new object()),
                Encoding.UTF8,
                "application/json");

            var respuesta = await cliente.PostAsync(url, cuerpo);
            var texto = await respuesta.Content.ReadAsStringAsync();

            if (!respuesta.IsSuccessStatusCode)
            {
                var mensaje = texto;
                try
                {
                    var simple = JsonConvert.DeserializeObject<aplicaciones_libreria.nucleo.RespuestaSimple>(texto);
                    if (simple is { mensaje.Length: > 0 })
                        mensaje = simple.mensaje;
                }
                catch (JsonException)
                {
                    // El cuerpo no era JSON; se muestra tal cual.
                }

                throw new ApiExcepcion((int)respuesta.StatusCode, $"Error {(int)respuesta.StatusCode} en {url}: {mensaje}");
            }

            return JsonConvert.DeserializeObject<T>(texto)!;
        }
    }
}
