namespace Aegis.Cli.Exceptions.Parsers;

internal sealed class UnexpectedTokenException(string token) : IntentionalException($"Unexpected token '{token}'");