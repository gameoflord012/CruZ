﻿using CruZ.Utility;
using Microsoft.Xna.Framework.Graphics;
using System.Numerics;

namespace CruZ.Systems
{
    public partial class Camera
    {
        public Camera(Viewport viewport)
        {
            _viewPortWidth = viewport.Width;
            _viewPortHeight = viewport.Height;
        }

        public Camera(int vpWidth, int vpHeight) : this(new(0, 0, vpWidth, vpHeight))
        {
            
        }

        public Vector3 PointToCoordinate(Vector3 p)
        {
            var normalize_x = (p.X / _viewPortWidth - 0.5f);
            var normalize_y = (p.Y / _viewPortHeight - 0.5f);

            var coord = new Vector3(normalize_x * VirtualWidth, normalize_y * VirtualHeight, 0);

            return coord;
        }

        public Vector2 CoordinateToPoint(Vector3 coord)
        {
            coord += Position;

            var normalize_x = 0.5f + coord.X / VirtualWidth;
            var normalize_y = 0.5f + coord.Y / VirtualHeight;

            return new(
                FunMath.RoundInt(normalize_x * _viewPortWidth),
                FunMath.RoundInt(normalize_y * _viewPortHeight));
        }

        public Matrix4x4 ViewMatrix()
        {
            return
                Matrix4x4.CreateTranslation(Position) *

                Matrix4x4.CreateTranslation(
                    VirtualWidth / 2f,
                    VirtualHeight / 2f,
                    0f) *

                Matrix4x4.CreateScale(
                    _viewPortWidth / VirtualWidth,
                    _viewPortHeight / VirtualHeight, 1);
        }

        public Vector2 ScreenToSpaceScale()
        {
            return new(
                VirtualWidth / _viewPortWidth,
                VirtualHeight / _viewPortHeight);
        }

        public float VirtualWidth { get => _virtualWidth * Zoom.X; set => _virtualWidth = value; }
        public float VirtualHeight { 
            get => (PreserveRatio ? VirtualWidth / Ratio : _virtualHeight) * Zoom.Y;
            set => _virtualHeight = value; }

        public float ViewPortWidth    { get => _viewPortWidth; set => _viewPortWidth = value; }
        public float ViewPortHeight   { get => _viewPortHeight; set => _viewPortHeight = value; }

        public Vector3 Position = Vector3.Zero;

        public bool PreserveRatio = true;
        public float Ratio => ViewPortWidth / ViewPortHeight;

        public Vector3 Zoom = new(1, 1);

        private float _viewPortWidth;
        private float _viewPortHeight;
        private float _virtualWidth = 1980;
        private float _virtualHeight = 1080;
    }
}