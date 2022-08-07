namespace IM.IdentityService.Business.InternalServices;

public static class PhoneHelper
{
    public static bool IsValidPhone(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        input = NormalizePhone(input);

        return input.Any() && input.Length is > 9 and < 16 && input.All(c =>
            char.IsDigit(c) ||
            char.IsWhiteSpace(c));
    }

    public static string NormalizePhone(this string input)
    {
        return string.Concat(
            (input ?? throw new ArgumentNullException(nameof(input)))
            .Where(char.IsDigit));
    }
}
