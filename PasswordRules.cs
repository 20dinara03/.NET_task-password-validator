namespace PasswordValidation;

public sealed class PasswordRules
{
    public int MinLength { get; init; } = 8;
    public int MaxLength { get; init; } = 64;

    public bool RequireLower   { get; init; } = true;
    public bool RequireUpper   { get; init; } = true;
    public bool RequireDigit   { get; init; } = true;
    public bool RequireSpecial { get; init; } = true;

    public bool ForbidWhitespace { get; init; } = true;

    // Разрешённые «спецсимволы»
    public string SpecialChars { get; init; } = "!@#$%^&*()-_=+[]{};:,.<>/?|\\~`";

    // Чёрный список символов (можно оставить пустым)
    public string ForbiddenChars { get; init; } = "";

    // Максимально допустимая длина повтора одного символа подряд (0 = не проверять)
    public int MaxRunLength { get; init; } = 2;
}
