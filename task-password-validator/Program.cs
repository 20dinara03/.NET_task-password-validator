using PasswordValidation;

class Program
{
    static void Main()
    {
        var rules = new PasswordRules
        {
            MinLength = 8,
            MaxLength = 64,
            RequireLower = true,
            RequireUpper = true,
            RequireDigit = true,
            RequireSpecial = true,
            ForbidWhitespace = true,
            SpecialChars = "!@#$%^&*()-_=+[]{};:,.<>/?|\\~`",
            ForbiddenChars = " ",
            MaxRunLength = 2
        };

        var samples = new[]
        {
            "Qwerty12",          // ок, но без спецсимвола
            "Qw12!!",            // короткий
            "Qw12!!  ",          // пробелы в конце
            "qqqqQQ11!!",        // длинные повторы
            "Aa1!goodPwd"        // должно пройти
        };

        foreach (var s in samples)
        {
            var v = PasswordValidator.Validate(s, rules);
            Console.WriteLine($"{s,-15}  valid={v.IsValid}  errors={v.Errors}");
            if (!v.IsValid && v.Messages.Count > 0)
                Console.WriteLine("  - " + string.Join("; ", v.Messages));
        }
    }
}
