namespace PasswordValidation;

public static class PasswordValidator
{
    private static bool[] BuildAsciiLookup(string chars)
    {
        var map = new bool[128];
        if (string.IsNullOrEmpty(chars)) return map;
        foreach (var ch in chars)
            if (ch < 128) map[ch] = true;
        return map;
    }

    public static PasswordValidationResult Validate(string password, PasswordRules rules)
    {
        var res = new PasswordValidationResult();

        if (password is null)
        {
            res.Errors |= PasswordError.TooShort;
            res.Messages.Add("Пароль не задан.");
            return res;
        }

        int len = password.Length;

        if (len < rules.MinLength)
            res.Errors |= PasswordError.TooShort;

        if (len > rules.MaxLength)
            res.Errors |= PasswordError.TooLong;

        // Lookup-таблицы
        var specials  = BuildAsciiLookup(rules.SpecialChars);
        var forbidden = BuildAsciiLookup(rules.ForbiddenChars);

        // Битовые флаги наличия категорий
        const int LOWER = 1 << 0;
        const int UPPER = 1 << 1;
        const int DIGIT = 1 << 2;
        const int SPEC  = 1 << 3;

        int have = 0;

        int maxRun = rules.MaxRunLength;
        int runLen = 1;
        char prev = '\0';

        for (int i = 0; i < len; i++)
        {
            char c = password[i];

            // Категории
            if (c >= 'a' && c <= 'z') have |= LOWER;
            else if (c >= 'A' && c <= 'Z') have |= UPPER;
            else if (c >= '0' && c <= '9') have |= DIGIT;
            else if (c < 128 && specials[c]) have |= SPEC;

            // Пробелы/whitespace
            if (rules.ForbidWhitespace && char.IsWhiteSpace(c))
                res.Errors |= PasswordError.HasWhitespace;

            // Запрещённые символы
            if (c < 128 && forbidden[c])
                res.Errors |= PasswordError.HasForbiddenChars;

            // Повторы подряд
            if (maxRun > 0)
            {
                if (i > 0 && c == prev) runLen++;
                else runLen = 1;

                if (runLen > maxRun)
                    res.Errors |= PasswordError.RunTooLong;

                prev = c;
            }
        }

        // Проверяем обязательные категории
        if (rules.RequireLower   && (have & LOWER) == 0) res.Errors |= PasswordError.NoLower;
        if (rules.RequireUpper   && (have & UPPER) == 0) res.Errors |= PasswordError.NoUpper;
        if (rules.RequireDigit   && (have & DIGIT) == 0) res.Errors |= PasswordError.NoDigit;
        if (rules.RequireSpecial && (have & SPEC ) == 0) res.Errors |= PasswordError.NoSpecial;

        // Сообщения
        if ((res.Errors & PasswordError.TooShort) != 0)       res.Messages.Add($"Длина меньше минимальной ({len} < {rules.MinLength}).");
        if ((res.Errors & PasswordError.TooLong) != 0)        res.Messages.Add($"Длина больше максимальной ({len} > {rules.MaxLength}).");
        if ((res.Errors & PasswordError.NoLower) != 0)        res.Messages.Add("Нет строчных букв.");
        if ((res.Errors & PasswordError.NoUpper) != 0)        res.Messages.Add("Нет заглавных букв.");
        if ((res.Errors & PasswordError.NoDigit) != 0)        res.Messages.Add("Нет цифр.");
        if ((res.Errors & PasswordError.NoSpecial) != 0)      res.Messages.Add("Нет спецсимволов.");
        if ((res.Errors & PasswordError.HasWhitespace) != 0)  res.Messages.Add("Содержит пробелы/пробельные символы.");
        if ((res.Errors & PasswordError.HasForbiddenChars)!=0)res.Messages.Add("Содержит запрещённые символы.");
        if ((res.Errors & PasswordError.RunTooLong) != 0 && maxRun > 0)
            res.Messages.Add($"Слишком длинные повторы подряд (>{maxRun}).");

        return res;
    }
}
