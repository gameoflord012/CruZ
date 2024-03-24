using CruZ.Experiment.BloomFilter;

using Microsoft.Xna.Framework;

namespace CruZ.Experiment;

internal class Program
{
    static void Main(string[] args)
    {
        Game game = new BloomFilterExperiment();
        game.Run();
    }
}
