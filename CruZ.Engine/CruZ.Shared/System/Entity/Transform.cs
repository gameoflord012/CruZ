using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace CruZ.Components
{
    public class TransformEventArgs : EventArgs
    {
        public Vector3 Position;
        public Vector3 Scale;

        public static TransformEventArgs Create(Transform t)
        {
            var args = new TransformEventArgs();
            args.Position = t.Position;
            args.Scale = t.Scale;
            return args;
        }
    }

    public partial class Transform
    {
        public event EventHandler<TransformEventArgs> OnPositionChanged;
        public event EventHandler<TransformEventArgs> OnScaleChanged;
        public Transform()
        {
            _position = Vector3.Zero;
            _scale = Vector3.One;
        }

        [JsonIgnore]
        public Matrix TotalMatrix { get => ScaleMatrix * TranslateMatrix; }
        [JsonIgnore]
        public Matrix TranslateMatrix { get => Matrix.CreateTranslation(_position); }
        [JsonIgnore]
        public Matrix ScaleMatrix { get => Matrix.CreateScale(_scale); }
        
        public Vector3 Scale { 
            get => _scale; 
            set { _scale = value; OnScaleChanged?.Invoke(this, TransformEventArgs.Create(this)); } 
        }
        
        public Vector3 Position 
        { 
            get => _position;
            set { _position = value; OnPositionChanged?.Invoke(this, TransformEventArgs.Create(this)); }
        }

        Vector3 _position;
        Vector3 _scale;
    }
}
