using CruZ.Components;
using CruZ.Serialization;
using MonoGame.Extended.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CruZ
{
    public class GameScene : ISerializable
    {
        public event Action<TransformEntity>? OnEntityAdded;
        public event Action<TransformEntity>? OnEntityRemoved;

        public string               Name        { get => _name; set => _name = value; }
        public TransformEntity[]    Entities    { get => _entities.ToArray(); }
        public List<EntityTemplate> Templates   { get => _templates; set => _templates = value; }

        public void AddEntity(TransformEntity e)
        {
            if (_entities.Contains(e)) return;
            _entities.Add(e);
            OnEntityAdded?.Invoke(e);
        }

        public void RemoveEntity(TransformEntity e)
        {
            if (_entities.Contains(e)) return;
            _entities.Remove(e);
            OnEntityRemoved?.Invoke(e);
        }


        ISerializable? ISerializable.CreateDefault()
        {
            return new GameScene();
        }

        public void ReadJson(JsonReader reader, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            foreach (var template in _templates)
            {
                if (jObject.ContainsKey(template.NameId))
                {
                    var e = jObject[template.NameId].ToObject<TransformEntity>(serializer);
                    template.Initialize(e);
                }
                else
                {
                    ECS.BuildTemplate(template);
                }

                AddEntity(template.Entity);
            }
        }

        public void WriteJson(JsonWriter writer, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            foreach(var template in _templates) 
            { 
                var e = template.Entity;

                writer.WritePropertyName(template.NameId);
                serializer.Serialize(writer, e);
            }
            writer.WriteEnd();
        }

        string _name = "";

        List<EntityTemplate> _templates = [];
        List<TransformEntity> _entities = [];
    }
}
