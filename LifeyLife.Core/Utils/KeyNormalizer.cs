using System.Globalization;
using LifeyLife.Core.Models;

namespace LifeyLife.Core.Utils;

public static class KeyNormalizer
{
    public static string NormalizeName(string? userName)
    {
        return userName?.ToLower(CultureInfo.InvariantCulture) ?? string.Empty;
    }
    
    public static string NormalizeEmail(string? email)
    {
        return email?.ToLower(CultureInfo.InvariantCulture) ?? string.Empty;
    }
}