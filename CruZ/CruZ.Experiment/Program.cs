
using CruZ.Experiment.Filter;
using CruZ.GameEngine;

using Microsoft.Xna.Framework;

namespace CruZ.Experiment;

internal class Program
{
    static void Main(string[] args)
    {
        GameWrapper game = new SoundExperiment();
        GameApplication gameApp = GameApplication.CreateContext(game);
        gameApp.Run();
    }
}
