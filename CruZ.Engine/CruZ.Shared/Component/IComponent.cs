using Newtonsoft.Json;
using System;

namespace CruZ.Components
{
    public interface IComponent
    {
        Type ComponentType { get; }
    }
}