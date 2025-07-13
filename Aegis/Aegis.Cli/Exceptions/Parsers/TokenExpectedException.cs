namespace Aegis.Cli.Exceptions.Parsers;

internal sealed class TokenExpectedException() : IntentionalException("Incomplete command. Token expected");