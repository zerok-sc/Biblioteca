namespace aplicaciones_libreria.nucleo
{
    public class PedidoCrearUsuario
    {
        public string nombre_usuario { get; set; } = "";
        public string correo { get; set; } = "";
        public string clave { get; set; } = "";
        public int rol { get; set; }
        public int persona { get; set; }
        public bool activo { get; set; } = true;
    }
}
