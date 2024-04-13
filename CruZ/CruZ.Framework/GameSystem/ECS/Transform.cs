using Microsoft.Xna.Framework;

using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CruZ.Framework.GameSystem.ECS
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public partial class Transform : INotifyPropertyChanged
    {
        public event EventHandler<TransformEventArgs> OnPositionChanged;
        public event EventHandler<TransformEventArgs> OnScaleChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

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
                OnScaleChanged?.Invoke(this, TransformEventArgs.Create(this));
                //InvokeChanged(nameof(Scale));
            }
        }

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPositionChanged?.Invoke(this, TransformEventArgs.Create(this));
                //InvokeChanged(nameof(Position));
            }
        }

        private void InvokeChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        Vector2 _position;
        Vector2 _scale;
    }
}
