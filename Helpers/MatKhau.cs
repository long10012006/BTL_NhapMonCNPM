using System;
using System.Security.Cryptography;
namespace SanLichWeb.Helpers
{
    public static class MatKhau
    {
        private const int SoVong = 120000;
        public static string Bam(string password)
        {
            byte[] salt = new byte[16];
            using (var random = RandomNumberGenerator.Create()) random.GetBytes(salt);
            using (var derive = new Rfc2898DeriveBytes(password, salt, SoVong, HashAlgorithmName.SHA256))
            {
                return "PBKDF2-SHA256$" + SoVong + "$" + Convert.ToBase64String(salt) + "$" + Convert.ToBase64String(derive.GetBytes(32));
            }
        }
        public static bool Dung(string password, string stored)
        {
            try
            {
                string[] parts = stored.Split('$');
                if (parts.Length != 4 || parts[0] != "PBKDF2-SHA256") return false;
                byte[] expected = Convert.FromBase64String(parts[3]);
                using (var derive = new Rfc2898DeriveBytes(password, Convert.FromBase64String(parts[2]), int.Parse(parts[1]), HashAlgorithmName.SHA256))
                {
                    byte[] actual = derive.GetBytes(expected.Length);
                    int diff = 0;
                    for (int i = 0; i < actual.Length; i++) diff |= actual[i] ^ expected[i];
                    return diff == 0;
                }
            }
            catch (FormatException) { return false; }
        }
    }
}
