using System.Security.Cryptography;
using System.Text;

namespace RestaurantOrderAPI.Application.Helpers;

public static class PasswordHelper
{
    public static string Hash(string password) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password))).ToLower();

    public static bool Verify(string password, string hash) =>
        Hash(password) == hash;
}
