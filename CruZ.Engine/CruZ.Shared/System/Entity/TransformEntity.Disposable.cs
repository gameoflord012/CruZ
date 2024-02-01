using CruZ.Components;
using MonoGame.Extended.Entities;
using System;

namespace CruZ.Components
{
    public partial class TransformEntity : IDisposable
    {
        public void Dispose()
        {
            this.RemoveFromWorld();
        }
    }
}