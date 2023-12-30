using CruZ.Components;
using System.Collections.Generic;

namespace CurZ
{
    public class GameScene
    {
        List<TransformEntity> _entities = new();
        public List<TransformEntity> Entities { get => _entities; set => _entities = value; }
        public string Name { get => _name; set => _name = value; }

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

        string _name;
    }
}
