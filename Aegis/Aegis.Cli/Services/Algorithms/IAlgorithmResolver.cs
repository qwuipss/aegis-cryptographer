using Aegis.Core.Algorithms;

namespace Aegis.Cli.Services.Algorithms;

internal interface IAlgorithmResolver
{
    IAlgorithm Resolve(string algorithmName);
}