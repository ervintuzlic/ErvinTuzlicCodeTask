namespace IntusCodeTaskErvinTuzlic.Shared.Extensions;

public static class StringExtensions
{
    public static bool IsNullOrWhitespace(this string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    public static int ParseToInt(this string value, int defaultValue = 0)
    {
        if (value.IsNullOrWhitespace())
        {
            return defaultValue;
        }

        if (int.TryParse(value, out int result))
        {
            return result;
        }


        return defaultValue;
    }

    public static T ParseToEnum<T>(this string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static bool ParseToBool(this string? value, bool defaultValue = false)
    {
        if (value.IsNullOrWhitespace())
        {
            return defaultValue;
        }

        if (bool.TryParse(value, out bool result))
        {
            return result;
        }

        return defaultValue;
    }

    public static bool IsAnyOf(this string valuee, params string[] valuesToCompare)
    {
        return valuesToCompare.Any(value => valuee.Equals(value, StringComparison.OrdinalIgnoreCase));
    }

    public static string? NullWhenEmpty(this string? value)
    {
        return string.IsNullOrEmpty(value) ? null : value;
    }
}