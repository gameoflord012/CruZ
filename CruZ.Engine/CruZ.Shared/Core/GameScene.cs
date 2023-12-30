using CruZ.Components;
using System;
using System.Collections.Generic;

namespace CruZ
{
    public class GameScene
    {
        public event Action<TransformEntity>? OnEntityAdded;
        public event Action<TransformEntity>? OnEntityRemoved;

        public string Name                      { get => _name; set => _name = value; }
        public List<TransformEntity> Entities   { get => _entities; set => _entities = value; }

        public void AddEntity(TransformEntity e)
        {
            if (_entities.Contains(e)) return;
            _entities.Add(e);
        }

        public void RemoveEntity(TransformEntity e)
        {
            if (_entities.Contains(e)) return;
            _entities.Remove(e);
        }

        string _name = "";
        List<TransformEntity> _entities = new();
    }
}
