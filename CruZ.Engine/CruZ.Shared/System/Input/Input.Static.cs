using System;
using System.Collections.Generic;
using System.Text;

namespace CruZ.Systems
{
    public partial class Input
    {
        static Input? _instance;
        public static Input Instance { get => _instance ??= new Input(); }
    }
}
