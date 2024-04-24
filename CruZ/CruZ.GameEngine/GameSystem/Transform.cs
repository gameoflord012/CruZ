using Microsoft.Xna.Framework;

using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CruZ.GameEngine.GameSystem
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Transform
    {
        public Transform()
        {
            _position = Vector2.Zero;
            _scale = Vector2.One;
        }

        public Vector2 Scale
        {
            get => _scale;
            set
            {
                _scale = value;
            }
        }

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
            }
        }

        public float Rotation
        {
            get; 
            set;
        }

        Vector2 _position;
        Vector2 _scale;
    }
}
