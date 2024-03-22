using CruZ.Common.Utility;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;
using CruZ.Framework.GameSystem.ECS;


namespace CruZ.Common.ECS
{
    public partial class TransformEntity : Entity
    {
        public event EventHandler<bool>? OnActiveStateChanged;
        public event EventHandler? RemoveFromWorldEvent;
        public event Action<ComponentCollection>? ComponentChanged;

        internal TransformEntity(World world) : base(world) { }

        private void SetIsActive(bool value)
        {
            _isActive = value;
            OnActiveStateChanged?.Invoke(this, value);
        }

        [ReadOnly(true)]
        public string           Name        { get => _name;         set => _name = value; }
        public TransformEntity? Parent      { get => _parent;       set => _parent = value; }
        public bool             IsActive    { get => _isActive;     set => SetIsActive(value); }
        public Transform        Transform   { get => _transform;    set => _transform = value; }
        [Browsable(false)]
        public Vector2          Position    { get => Transform.Position; set => Transform.Position = value; }
        [Browsable(false)]
        public Vector2          Scale       { get => Transform.Scale; set => Transform.Scale = value; }

        TransformEntity?                _parent;
        bool                            _isActive = false;
        Transform                       _transform = new();
        Dictionary<Type, Component>     _tyToComp = new();
        string                          _name = "";
    }
}
