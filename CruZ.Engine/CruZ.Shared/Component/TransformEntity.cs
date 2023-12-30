using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace CruZ.Components
{
    //public class RectTransform
    //{
    //    public void SetWidth(float x)
    //    {
    //        Size = new Vector3(x, Size.Y);
    //    }

    //    public void SetHeight(float y)
    //    {
    //        Size = new Vector3(Size.X, y);
    //    }

    //    public Vector3 GetUpperLeft()
    //    {
    //        return new Vector3(-Size.X / 2f, -Size.Y / 2f);
    //    }

    //    public Matrix GetOffsetMatrix()
    //    {
    //        return Matrix.CreateTranslation(
    //                CenterOffset.X,
    //                CenterOffset.Y, 0);
    //    }

    //    public Vector3 CenterOffset = Vector3.Zero;
    //    public Vector3 Size = Vector3.One;
    //}

    //public struct RectCoord
    //{
    //    public float X;
    //    public float Y;

    //    public float Width;
    //    public float Height;

    //    public Vector3 TopLeft { get => new Vector3(X, Y); set { X = value.X; Y = value.Y; } }
    //    public Vector3 BottomRight { get => new Vector3(X + Width, Y - Height); }
    //    public Vector3 Center { get => new Vector3(X + Width / 2f, Y - Height / 2f); }
    //    public Vector3 Size { get => new Vector3(Width, Height); set { Width = value.X; Height = value.Y; } }
    //}

    public partial class TransformEntity : IEquatable<TransformEntity>
    {
        public TransformEntity(Entity e)
        {
            Entity = e;
            Transform = new();
            //RectTransform = new();
            _transformEntityDict[e.Id] = this;
        }

        #region Entities
        public TransformEntity AddComponent<T>(T component) where T : class
        {
            AddComponent(component, typeof(T));

            return this;
        }

        public TransformEntity AddComponent(object component, Type ty)
        {
            Entity.Attach(component, ty);

            _entityDict[component] = this;
            _addedComponents.Add(ty, component);

            if (component is IComponentAddedCallback)
            {
                var callback = component as IComponentAddedCallback;
                callback.OnComponentAdded(this);
            }

            return this;
        }

        public static TransformEntity GetEntity(object component)
        {
            if (!_entityDict.ContainsKey(component))
            {
                throw new Exception("Im tired of this shit");
            }
            else
            {
                return _entityDict[component];
            }
        }

        public static KeyValuePair<Type, object>[] GetAllComponents(TransformEntity e)
        {
            return e._addedComponents.ToArray();
        }

        public static TransformEntity GetTransformEntity(int eId)
        {
            Trace.Assert(_transformEntityDict.ContainsKey(eId));
            return _transformEntityDict[eId];
        }
        #endregion

        public void RemoveFromWorld()
        {
            IsActive = false;
            MGWrapper.Instance().World.DestroyEntity(Entity);
            Entity = null;
        }

        public bool Equals(TransformEntity other)
        {
            return other.Entity.Id == Entity.Id;
        }

        //public RectCoord GetRectCoord()
        //{
        //    RectCoord rectCoord;

        //    rectCoord.Width = RectTransform.Size.X * Transform.Scale.X;
        //    rectCoord.Height = RectTransform.Size.Y * Transform.Scale.Y;

        //    rectCoord.X = Transform.Position.X - rectCoord.Width / 2f - RectTransform.CenterOffset.X;
        //    rectCoord.Y = Transform.Position.Y + rectCoord.Height / 2f - RectTransform.CenterOffset.Y;

        //    return rectCoord;
        //}

        private static Dictionary<object, TransformEntity> _entityDict = new();
        private static Dictionary<int, TransformEntity> _transformEntityDict = new();

        public Transform Transform { get => _transform; set => _transform = value; }
        public Entity Entity { get => _entity; set => _entity = value; }
        public bool IsActive { get => _isActive; set => _isActive = value; }
        //public RectTransform RectTransform { get => _rectTransform; set => _rectTransform = value; }

        bool _isActive;
        Entity _entity;
        Transform _transform;
        //RectTransform _rectTransform;
        Dictionary<Type, object> _addedComponents = new();
    }
}
