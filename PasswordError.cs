using System;

namespace PasswordValidation;

[Flags]
public enum PasswordError
{
    None                = 0,
    TooShort            = 1 << 0,
    TooLong             = 1 << 1,
    NoLower             = 1 << 2,
    NoUpper             = 1 << 3,
    NoDigit             = 1 << 4,
    NoSpecial           = 1 << 5,
    HasWhitespace       = 1 << 6,
    HasForbiddenChars   = 1 << 7,
    RunTooLong          = 1 << 8,
}
