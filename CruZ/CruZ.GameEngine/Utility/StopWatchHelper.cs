using System.Diagnostics;

namespace CruZ.GameEngine.Utility
{
    public static class StopWatchHelper
    {
        public static float GetElapsed(this Stopwatch stopwatch)
        {
            return (float)stopwatch.Elapsed.TotalSeconds;
        }
    }
}
