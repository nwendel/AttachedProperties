using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AttachedProperties.Infrastructure;

internal static class GuardAgainst
{
    public static void Null<T>([NotNull] T? value, [CallerArgumentExpression("value")] string? argumentName = null)
        where T : class
    {
        if (value == null)
        {
            throw new ArgumentNullException(argumentName);
        }
    }

    public static void NullOrWhiteSpace([NotNull] string? value, [CallerArgumentExpression("value")] string? argumentName = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(argumentName);
        }
    }
}
