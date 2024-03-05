using Microsoft.Xna.Framework;

using Newtonsoft.Json;

using System;
using System.ComponentModel;

namespace CruZ.Common.ECS
{
    using DataType;

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

    
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public partial class Transform : INotifyPropertyChanged
    {
        public event EventHandler<TransformEventArgs> OnPositionChanged;
        public event EventHandler<TransformEventArgs> OnScaleChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        public Transform()
        {
            _position = Vector3.Zero;
            _scale = Vector3.One;
        }

        [JsonIgnore, Browsable(false)]
        public Matrix TotalMatrix { get => ScaleMatrix * TranslateMatrix; }
        [JsonIgnore, Browsable(false)]
        public Matrix TranslateMatrix { get => Matrix.CreateTranslation(_position); }
        [JsonIgnore, Browsable(false)]
        public Matrix ScaleMatrix { get => Matrix.CreateScale(_scale); }
        
        public Vector3 Scale { 
            get => _scale; 
            set { 
                _scale = value; 
                OnScaleChanged?.Invoke(this, TransformEventArgs.Create(this)); 
                //InvokeChanged(nameof(Scale));
            } 
        }
        
        public Vector3 Position 
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

        Vector3 _position;
        Vector3 _scale;
    }
}
