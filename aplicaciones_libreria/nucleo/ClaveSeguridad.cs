using System.Security.Cryptography;
using System.Text;

namespace aplicaciones_libreria.nucleo
{
    public static class ClaveSeguridad
    {
        public static byte[] Hash(string clave, byte[] sal)
        {
            var texto = clave + Encoding.UTF8.GetString(sal);
            return SHA256.HashData(Encoding.UTF8.GetBytes(texto));
        }

        public static bool Coinciden(byte[] guardado, byte[] calculado)
        {
            return guardado.Length == calculado.Length
                && CryptographicOperations.FixedTimeEquals(guardado, calculado);
        }
    }
}
