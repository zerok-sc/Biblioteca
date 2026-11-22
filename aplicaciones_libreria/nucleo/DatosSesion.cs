namespace aplicaciones_libreria.nucleo
{
    public class PedidoLogin
    {
        public string usuario { get; set; } = "";
        public string clave { get; set; } = "";
    }

    public class PedidoFoto
    {
        public int id { get; set; }
        public string? imagen { get; set; }
        public string? tipo { get; set; }
    }

    public class DatosFoto
    {
        public string? imagen { get; set; }
    }

    public class RespuestaSimple
    {
        public string mensaje { get; set; } = "";
    }

    public class DatosSesion
    {
        public int id { get; set; }
        public string nombre_usuario { get; set; } = "";
        public string correo { get; set; } = "";
        public string rol_codigo { get; set; } = "";
        public string rol_nombre { get; set; } = "";
        public string persona_nombre { get; set; } = "";
        public string? imagen { get; set; }
        public string? imagen_tipo { get; set; }
    }
}
