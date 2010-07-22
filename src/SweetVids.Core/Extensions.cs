using System.Text;

namespace SweetVids.Core
{
    public static class Extensions
    {
        public static string ToGravatarHash(this string email)
        {
            var hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var hash = new StringBuilder();

            foreach (byte b in hasher.ComputeHash(Encoding.UTF8.GetBytes(email.ToLower())))
            {
                hash.Append(b.ToString("x2").ToLower());
            }

            return hash.ToString();
        }
    }
}