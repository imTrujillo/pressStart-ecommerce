using System.Text.RegularExpressions;

namespace Shop.Application.Validators;

public static class DocumentValidator
{
    public static bool CheckDui(string dui)
    {
        var cleanDui = dui.Replace("-", "");
        var digits = cleanDui.Substring(0, 8);
        var checkLastDigit = int.Parse(cleanDui.Substring(8, 1));
        
        var multipliers = new int[] { 9, 8, 7, 6, 5, 4, 3, 2 };
        int sum = 0;
        
        for (int i = 0; i < 8; i++)
        {
            sum += int.Parse(digits[i].ToString()) * multipliers[i];
        }
        
        var remainder = sum % 11;
        var calculatedCheckDigit = remainder < 2 ? 0 : 11 - remainder;

        return calculatedCheckDigit == checkLastDigit;
    }

    public static bool CheckNit(string nit)
    {
        var cleanNit = nit.Replace("-", "");
        var digits = cleanNit.Substring(0, 13);
        var checkLastDigit = int.Parse(cleanNit.Substring(13, 1)); 
        
        var multipliers = new int[] { 3, 7, 13, 17, 19, 23, 29, 34, 37, 41, 43, 47, 53 };
        var sum = 0;

        for (int i = 0; i < 13; i++)
        {
            sum += int.Parse(digits[i].ToString()) * multipliers[i];
        }

        var remainder = sum % 11;
        var calculatedCheckDigit = remainder < 2 ? 0 : 11 - remainder;

        return calculatedCheckDigit == checkLastDigit;
    }

    public static bool CheckPhoneNumber(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return false;
        
        // Patrones válidos para El Salvador:
        // Teléfonos fijos: 2XXX-XXXX (comenzando con 2)
        // Móviles: 6XXX-XXXX, 7XXX-XXXX (comenzando con 6 o 7)
        // Con código de país: +503 XXXX-XXXX o 503 XXXX-XXXX
        
        var patterns = new[]
        {
            @"^[267][0-9]{3}-[0-9]{4}$",              // 7812-3456, 2234-5678, 6123-4567
            @"^\+503\s[267][0-9]{3}-[0-9]{4}$",       // +503 7812-3456
            @"^503[267][0-9]{7}$",                    // 50378123456
            @"^[267][0-9]{7}$"                        // 78123456
        };
        
        return patterns.Any(pattern => Regex.IsMatch(phone, pattern));
    }
}