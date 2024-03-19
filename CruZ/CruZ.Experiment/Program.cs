using System;

using CruZ.Common.Input;

using Microsoft.Xna.Framework;

namespace CruZ.Experiment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new SpriteBatchLayerDepthExperiment();
            game.Run();
        }
    }
}
