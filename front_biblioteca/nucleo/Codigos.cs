namespace front_biblioteca.nucleo
{
    // Códigos que el sistema espera encontrar en las tablas Roles y Estados.
    // Están aquí para no repetirlos como texto en cada página.
    public static class Codigos
    {
        public const string RolAdmin = "ADMIN";
        public const string RolBibliotecario = "BIBLIO";
        public const string RolLector = "LECTOR";
        public const string RolesOperador = RolAdmin + "," + RolBibliotecario;

        public const string EstadoActivo = "ACT";
        public const string EstadoInactivo = "INA";
        public const string EstadoPrestado = "PRE";
        public const string EstadoDisponible = "DIS";
        public const string EstadoMantenimiento = "MAN";
        public const string EstadoDevuelto = "DEV";
        public const string EstadoAtrasado = "ATR";
        public const string EstadoPagada = "PAG";
    }
}
