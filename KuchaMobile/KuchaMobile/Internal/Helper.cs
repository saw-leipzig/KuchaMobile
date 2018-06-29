using System.Security.Cryptography;
using System.Text;

namespace KuchaMobile.Internal
{
    public static class Helper
    {
        public static int ScreenHeight { get; set; }
        public static int ScreenWidth { get; set; }

        public static string GetMD5Hash(string TextToHash)
        {
            MD5 md5Hash = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(TextToHash));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}