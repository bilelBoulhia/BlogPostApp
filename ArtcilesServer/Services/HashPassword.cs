using System.Security.Cryptography;
using System.Text;

namespace ArtcilesServer.Services
{
    public class HashPassword
    {
        public string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        public string GenerateHash(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] CB = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashB = sha256.ComputeHash(CB);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashB.Length; i++)
                {

                    sb.Append(hashB[i].ToString("x2"));


                }
                return sb.ToString();

            }

        }
    }
}

