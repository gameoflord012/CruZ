using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace CruZ.Components
{
    public interface IComponent
    {
        [Browsable(false)]
        Type ComponentType { get; }
    }
}