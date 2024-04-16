using Microsoft.Xna.Framework;

using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CruZ.Framework.GameSystem
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public partial class Transform
    {
        public Transform()
        {
            _position = Vector2.Zero;
            _scale = Vector2.One;
        }

        [JsonIgnore, Browsable(false)]
        public Matrix TotalMatrix { get => ScaleMatrix * TranslateMatrix; }
        [JsonIgnore, Browsable(false)]
        public Matrix TranslateMatrix { get => Matrix.CreateTranslation(_position.X, _position.Y, 0); }
        [JsonIgnore, Browsable(false)]
        public Matrix ScaleMatrix { get => Matrix.CreateScale(_scale.X, _scale.Y, 1); }

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

        Vector2 _position;
        Vector2 _scale;
    }
}
