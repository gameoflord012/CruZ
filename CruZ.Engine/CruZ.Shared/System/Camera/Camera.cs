using CruZ.Utility;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Drawing;
using System.Numerics;

namespace CruZ.Systems
{
    public partial class Camera
    {
        //public event Action OnCameraValueChanged;

        public Camera(Viewport viewport)
        {
            ViewPortWidth = viewport.Width;
            ViewPortHeight = viewport.Height;
        }

        public Camera(int vpWidth, int vpHeight) : this(new(0, 0, vpWidth, vpHeight))
        {
            
        }

        public Vector3 PointToCoordinate(Vector3 p)
        {
            var normalize_x = (p.X / ViewPortWidth - 0.5f);
            var normalize_y = (p.Y / ViewPortHeight - 0.5f);

            var coord = new Vector3(normalize_x * VirtualWidth, normalize_y * VirtualHeight, 0);
            coord += Position;

            return coord;
        }

        public Vector3 PointToCoordinate(Point p)
        {
            return PointToCoordinate(new Vector3(p.X, p.Y));
        }

        public Point CoordinateToPoint(Vector3 coord)
        {
            coord -= Position;

            var normalize_x = 0.5f + coord.X / VirtualWidth;
            var normalize_y = 0.5f + coord.Y / VirtualHeight;

            return new(
                FunMath.RoundInt(normalize_x * ViewPortWidth),
                FunMath.RoundInt(normalize_y * ViewPortHeight));
        }

        public Matrix4x4 ViewMatrix()
        {
            return
                Matrix4x4.CreateTranslation(-Position.X, -Position.Y, -Position.Z) *

                Matrix4x4.CreateTranslation(
                    VirtualWidth / 2f,
                    VirtualHeight / 2f,
                    0f) *

                Matrix4x4.CreateScale(
                    ViewPortWidth / VirtualWidth,
                    ViewPortHeight / VirtualHeight, 1);
        }

        public Vector2 ScreenToWorldScale()
        {
            return new(
                VirtualWidth / ViewPortWidth,
                VirtualHeight / ViewPortHeight);
        }

        public Vector2 WorldToScreenScale()
        {
            return new(
                ViewPortWidth / VirtualWidth,
                ViewPortHeight / VirtualHeight);
        }

        public float VirtualWidth { 
            get => _virtualWidth * Zoom.X; 
            set { _virtualWidth = value; } }

        public float VirtualHeight { 
            get => (PreserveRatio ? VirtualWidth / Ratio : _virtualHeight) * Zoom.Y;
            set { _virtualHeight = value; } }

        public float ViewPortWidth { 
            get => _viewPortWidth; 
            set { _viewPortWidth = value; } }

        public float ViewPortHeight { 
            get => _viewPortHeight; 
            set { _viewPortHeight = value; } }

        public Vector3 Position { 
            get => _position; 
            set { _position = value; } }

        private Vector3 _position = Vector3.Zero;

        public bool PreserveRatio = true;
        public float Ratio => ViewPortWidth / ViewPortHeight;

        public Vector3 Zoom { 
            get => _zoom; 
            set { _zoom = value; } }

        private Vector3 _zoom = new(1, 1);
        private float _viewPortWidth;
        private float _viewPortHeight;
        private float _virtualWidth = 19;
        private float _virtualHeight = 10;
    }
}