using CruZ.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace CruZ.Components
{
    using XNA = Microsoft.Xna.Framework;

    public class TileComponent : IComponent, IComponentCallback
    {
        [Browsable(false)]
        public Type ComponentType => typeof(TileComponent);

        public bool Debug       { get; set; } = false;
        public int  TileSize    { get; set; } = 16;

        public void OnAttached(TransformEntity entity)
        {
            _e = entity;
            _e.ComponentAdded += Entity_AddComponent;
        }


        public void OnDettached(TransformEntity entity)
        {
            throw new NotImplementedException();
        }

        private void Entity_AddComponent(object? sender, IComponent e)
        {
            if(_sp != null)
            {
                _sp.DrawLoopBegin -= Sprite_DrawLoopBegin;
                _sp.DrawLoopEnd -=  Sprite_DrawLoopEnd;
            }

            _e.TryGetComponent(ref _sp);

            if(_sp != null)
            {
                _sp.DrawLoopBegin += Sprite_DrawLoopBegin;
                _sp.DrawLoopEnd += Sprite_DrawLoopEnd;
            }
        }

        private void Sprite_DrawLoopBegin(object? sender, DrawLoopBeginEventArgs args)
        {
            if(Debug && (_idX + _idY) % 2 == 0)
            {
                args.Skip = true;
                return;
            }

            args.SourceRectangle = new(_idX * TileSize, _idY * TileSize, TileSize, TileSize);

            var delt = args.SourceRectangle.Center - args.Texture.Bounds.Center;

            args.Position += new Vector2(
                delt.X * _e.Transform.Scale.X, 
                delt.Y * _e.Transform.Scale.Y);

            if(_sp.YLayerDepth)
            {
                args.LayerDepth = (args.Position.Y / Variables.MAX_WORLD_DISTANCE + 1) / 2;
            }
        }

        private void Sprite_DrawLoopEnd(object? sender, DrawLoopEndEventArgs e)
        {
            var bounds = _sp.Texture.Bounds;

            if(_idX * TileSize < bounds.Width)
            {
                _idX++;
            }
            else if(_idY * TileSize < bounds.Height)
            {
                _idX = 0;
                _idY++;
            }
            else
            {
                _idX = 0;
                _idY = 0;
                e.KeepDrawing = false;
                
                return;
            }

            e.KeepDrawing = true;
        }

        int _idX, _idY;

        TransformEntity _e;
        SpriteComponent? _sp;
    }
}
