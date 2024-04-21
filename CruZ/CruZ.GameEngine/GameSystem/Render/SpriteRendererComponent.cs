using Microsoft.Xna.Framework.Graphics;
using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using System.Drawing.Design;
using System.Text.Json.Serialization;
using CruZ.GameEngine.Serialization;
using CruZ.GameEngine.Resource;
using CruZ.GameEngine.GameSystem.UI;
using CruZ.GameEngine.GameSystem.Render;
using CruZ.GameEngine;
using CruZ.GameEngine.Utility;

namespace CruZ.GameEngine.GameSystem.ECS
{
    /// <summary>
    /// Game component loaded from specify resource
    /// </summary>
    public partial class SpriteRendererComponent : RendererComponent, IHasBoundBox
    {
        public event EventHandler<DrawArgs>? DrawLoopBegin;
        public event EventHandler<DrawLoopEndEventArgs>? DrawLoopEnd;
        public event Action? DrawBegin;
        public event Action? DrawEnd;
        public event Action<UIBoundingBox>? BoundingBoxChanged;

        #region Properties
        public bool SortByY { get; set; } = false;
        public bool FlipHorizontally { get; set; }

        [JsonIgnore, Browsable(false)]
        public Texture2D? Texture { get => _texture; set => _texture = value; }
        public Vector2 NormalizedOrigin { get; set; } = new(0.5f, 0.5f);

        [Editor(typeof(FileUITypeEditor), typeof(UITypeEditor))]
        public string TexturePath
        {
            get => _spriteResInfo != null ? _spriteResInfo.ResourceName : "";
            set => LoadTexture(value);
        }
        #endregion

        public SpriteRendererComponent()
        {
            _resource = GameContext.GameResource;
            InitBoundingBoxEventHandlers();
        }

        private void InitBoundingBoxEventHandlers()
        {
            DrawBegin += () =>
            {
                _boundingBox.WorldOrigins.Clear();
                _hasBoundingBox = false;
            };

            DrawLoopEnd += (sender, args) =>
            {
                _boundingBox.WorldOrigins.Add(args.DrawArgs.GetWorldOrigin());

                if (!_hasBoundingBox)
                {
                    _boundingBox.WorldBounds = args.DrawArgs.GetWorldBounds();
                    _hasBoundingBox = true;
                }
                else
                {
                    var bounds = args.DrawArgs.GetWorldBounds();

                    _boundingBox.WorldBounds.X = MathF.Min(_boundingBox.WorldBounds.X, bounds.X);
                    _boundingBox.WorldBounds.Y = MathF.Min(_boundingBox.WorldBounds.Y, bounds.Y);
                    _boundingBox.WorldBounds.Width = _boundingBox.WorldBounds.Right < bounds.Right ? bounds.Right - _boundingBox.WorldBounds.X : _boundingBox.WorldBounds.Width;
                    _boundingBox.WorldBounds.Height = _boundingBox.WorldBounds.Bottom < bounds.Bottom ? bounds.Bottom - _boundingBox.WorldBounds.Y : _boundingBox.WorldBounds.Height;
                }
            };

            DrawEnd += () => BoundingBoxChanged?.Invoke(_hasBoundingBox ? _boundingBox : UIBoundingBox.Default);
        }

        public void LoadTexture(string texturePath)
        {
            if (!string.IsNullOrEmpty(texturePath))
            {
                _spriteResInfo = _resource.RetriveResourceInfo(texturePath);

                try
                {
                    Texture = _resource.Load<Texture2D>(_spriteResInfo);
                }
                catch (Exception e)
                {
                    throw new ArgumentException($"Failed to load texture with path \"{texturePath}\"", e);
                }
            }
        }

        public int CompareLayer(SpriteRendererComponent other)
        {
            return SortingLayer == other.SortingLayer ?
                CalculateLayerDepth().CompareTo(other.CalculateLayerDepth()) :
                SortingLayer.CompareTo(other.SortingLayer);
        }

        public override void Render(RendererEventArgs e)
        {
            var fx = EffectManager.NormalSpriteRenderer;
            fx.Parameters["view_projection"].SetValue(e.ViewProjectionMatrix);
            fx.Parameters["hdrColor"].SetValue(new Vector4(1, 1, 1, 1));

            DrawBegin?.Invoke();
            e.SpriteBatch.Begin(
                effect: fx,
                sortMode: SpriteSortMode.FrontToBack,
                samplerState: SamplerState.PointClamp);

            while (true)
            {
                #region Before Drawloop
                DrawArgs drawArgs = new();
                if(Texture != null) drawArgs.Apply(Texture);
                drawArgs.Apply(AttachedEntity);
                drawArgs.LayerDepth = CalculateLayerDepth();
                drawArgs.NormalizedOrigin = NormalizedOrigin;
                drawArgs.Color = Color.White;
                drawArgs.SpriteEffect = FlipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                DrawLoopBegin?.Invoke(this, drawArgs); 
                #endregion

                e.SpriteBatch.Draw(drawArgs);

                #region After Drawloop
                var drawEndArgs = new DrawLoopEndEventArgs(drawArgs);
                DrawLoopEnd?.Invoke(this, drawEndArgs);
                if (!drawEndArgs.KeepDrawing) break; 
                #endregion
            }

            e.SpriteBatch.End();
            DrawEnd?.Invoke();
        }

        private float CalculateLayerDepth()
        {
            return SortByY ? AttachedEntity.Transform.Position.Y / 2 : LayerDepth;
        }

        public override string ToString()
        {
            return _spriteResInfo != null ? _spriteResInfo.ResourceName : "<None>";
        }

        Texture2D? _texture;
        [JsonInclude]
        ResourceInfo? _spriteResInfo;
        ResourceManager _resource;
        UIBoundingBox _boundingBox = new();
        bool _hasBoundingBox;
    }
}
