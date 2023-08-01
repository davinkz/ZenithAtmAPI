namespace ZenithBankATM.API.Utils;

public static class StringUtils
{
    public static string GenerateRandomDigits(int length)
    {
        var random = new Random();
        string s = string.Empty;
        for (int i = 0; i < length; i++)
            s = String.Concat(s, random.Next(10).ToString());
        return s;
    }
}
