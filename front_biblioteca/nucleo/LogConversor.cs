namespace front_biblioteca.nucleo
{
    public static class LogConversor
    {
        public static void Log(Exception ex, IDictionary<string, object?> ViewData)
        {
            var mensaje = ex.Message;
            if (mensaje.Length > 100)
                mensaje = mensaje[..100] + "...";

            ViewData["Mensaje"] = mensaje;
        }
    }
}
