using System.ComponentModel;
using System.Reflection;

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
}