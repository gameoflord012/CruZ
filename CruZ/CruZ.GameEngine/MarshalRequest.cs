using System;
using System.Threading;

namespace CruZ.GameEngine
{
    public partial class GameApplication
    {
        private record MarshalRequest(Action Action, ManualResetEvent ResetEvent);
    }
}
