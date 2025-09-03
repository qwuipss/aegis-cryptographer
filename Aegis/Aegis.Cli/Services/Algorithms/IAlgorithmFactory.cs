using Aegis.Core.Algorithms;

namespace Aegis.Cli.Services.Algorithms;

internal interface IAlgorithmFactory
{
    IAlgorithm Create(AlgorithmType algorithmType);
}