using System.Security.Cryptography;
using System.Text;

namespace Predictor.Domain.Extensions;

public static class StringExtensions
{
    public static int StringHash(this string str)
    {
        var hashed = MD5.HashData(Encoding.UTF8.GetBytes(str));
        return BitConverter.ToInt32(hashed, 0);
    }

    public static string Between(this string str, string firstString, string lastString)
    {
        if (string.IsNullOrEmpty(str))
        {
            return string.Empty;
        }

        var posOne = str.IndexOf(firstString, StringComparison.Ordinal) + firstString.Length;
        var modString = str[posOne..];
        var posTwo = modString.IndexOf(lastString, StringComparison.Ordinal);
        var finalString = modString[..posTwo];
        return finalString;
    }
}