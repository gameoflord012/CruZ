using CruZ.Framework;
using CruZ.Framework.GameSystem.ECS;
using CruZ.Framework.Resource;
using CruZ.Framework.Serialization;

using System;
using System.Collections.Generic;

namespace CruZ.Common.Scene
{
    public partial class GameScene : IResource, ICustomSerializable
    {
        public event Action<TransformEntity>? EntityAdded;
        public event Action<TransformEntity>? EntityRemoved;

        public string Name = ""; // temporary use name as runtime resource path

        [JsonIgnore]
        public TransformEntity[] Entities { get => _entities.ToArray(); }
        ResourceInfo? IResource.Info { get; set; }

        public GameScene()
        {
            GameApplication.Exiting += Game_Exiting;
        }

        public void AddEntity(TransformEntity e)
        {
            if (_entities.Contains(e)) return;
            _entities.Add(e);
            e.IsActive = _isActive;

            EntityAdded?.Invoke(e);
        }

        public void RemoveEntity(TransformEntity e)
        {
            if (!_entities.Contains(e))
                throw new ArgumentException($"Entity \"{e}\" not in scene {this}");

            _entities.Remove(e);
            e.IsActive = _isActive;

            EntityRemoved?.Invoke(e);
        }

        public void SetActive(bool isActive)
        {
            if (_isActive == isActive) return;
            _isActive = isActive;

            foreach (var e in _entities)
            {
                e.IsActive = _isActive;
            }
        }

        public TransformEntity CreateEntity(string? name = null, TransformEntity? parent = null)
        {
            var e = ECSManager.CreateTransformEntity();

            if(!string.IsNullOrEmpty(name)) e.Name = name;
            e.Parent = parent;

            AddEntity(e);

            return e;
        }

        private void Game_Exiting()
        {
            Dispose();
        }

        public object ReadJson(JsonReader reader, JsonSerializer serializer)
        {
            serializer.Populate(reader, this);
            return this;
        }

        public void WriteJson(JsonWriter writer, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            {
                writer.WritePropertyName(nameof(Name));
                serializer.Serialize(writer, Name);

                writer.WritePropertyName(nameof(_entities));
                serializer.Serialize(writer, _entities);
            }
            writer.WriteEnd();
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? ((IResource)this).Info.ResourceName : Name;
        }

        bool _isActive = false;

        [JsonProperty]
        List<TransformEntity> _entities = [];
    }
}
