using System.ComponentModel;
using System.Reflection;
using System.Security.Cryptography;

namespace CreateUsers;

public class Utils
{
    public static string EnumGetDescriptionConverter(Enum value)
    {
        FieldInfo? field = value.GetType().GetField(value.ToString());

        if (field == null)
            return value.ToString();

        DescriptionAttribute? attribute =
            field.GetCustomAttribute<DescriptionAttribute>();

        return attribute?.Description ?? value.ToString();
    }

    public static string GeneratePassword(int length = 10)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";

        Span<char> result = stackalloc char[length];
        Span<byte> buffer = stackalloc byte[length];

        RandomNumberGenerator.Fill(buffer);

        for (var i = 0; i < length; i++)
        {
            result[i] = chars[buffer[i] % chars.Length];
        }

        return new string(result);
    }
}