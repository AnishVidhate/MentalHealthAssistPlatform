using System.Security.Cryptography;
using System.Text;

public static class Utils
{
    public static string CalculateSignature(string input, string key)
    {
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
        {
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash); // Razorpay requires Base64 encoding
        }
    }
}
