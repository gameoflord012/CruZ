using System.Collections.Generic;

using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Render;

using Microsoft.Xna.Framework;

using MonoGame.Extended.BitmapFonts;

namespace NinjaAdventure
{
    internal class HealthComponent : Component
    {
        public HealthComponent(int maxHealth, SpriteRendererComponent healthRenderer)
        {
            ShouldDisplay = true;

            _font = GameApplication.InternalResource.Load<BitmapFont>("Fonts\\Fixedsys.fnt", true);

            MaxHealth = maxHealth;
            _health = MaxHealth;
            _healthRenderer = healthRenderer;

            _healthRenderer.DrawRequestsFetching += SpriteRenderer_DrawRequestsFetching;
        }

        private void SpriteRenderer_DrawRequestsFetching(List<DrawRequestBase> drawRequests)
        {
            if(!ShouldDisplay) return;

            _font.LetterSpacing = -11;

            string text = new string('/', Current) + new string('-', MaxHealth - Current);
            var textRect = _font.GetStringRectangle(text);
            var scale = new Vector2(2f / textRect.Width, 2f / textRect.Width);

            var stringDrawRequest = new StringDrawRequest(_font, text, AttachedEntity.Position + Vector2.UnitY * 0.7f, scale);
            drawRequests.Add(stringDrawRequest);
        }

        public int MaxHealth
        {
            get;
            set;
        }

        public int Current
        { get => _health; set => _health = value < 0 ? 0 : value; }

        public bool ShouldDisplay
        {
            get; set;
        }

        private SpriteRendererComponent _healthRenderer;
        private BitmapFont _font;
        private int _health;

        public override void Dispose()
        {
            base.Dispose();
            _healthRenderer.DrawRequestsFetching -= SpriteRenderer_DrawRequestsFetching;
        }
    }
}
