using System.Collections.Generic;

namespace PasswordValidation;

public sealed class PasswordValidationResult
{
    public bool IsValid => Errors == PasswordError.None;
    public PasswordError Errors { get; set; } = PasswordError.None;
    public List<string> Messages { get; } = new();
}
