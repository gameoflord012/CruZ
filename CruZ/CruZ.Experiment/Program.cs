
using CruZ.Experiment.Filter;

using Microsoft.Xna.Framework;

namespace CruZ.Experiment;

internal class Program
{
    static void Main(string[] args)
    {
        Game game = new PhysicExperiment();
        game.Run();
    }
}
