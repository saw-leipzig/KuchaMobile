using System.Security.Cryptography;
using System.Text;

namespace KuchaMobile.Internal
{
    public static class Helper
    {
        public static int ScreenHeight { get; set; }
        public static int ScreenWidth { get; set; }

        public static string GetMD5Hash(string input)
        {            
            //Source: https://msdn.microsoft.com/de-de/library/system.security.cryptography.md5cryptoserviceprovider%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396

            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

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