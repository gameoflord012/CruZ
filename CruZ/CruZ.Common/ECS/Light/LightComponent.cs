using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Common.ECS
{
    public class LightComponent : Component
    {
        public override Type ComponentType => typeof(LightComponent);

        public List<int> SortingLayers { get; } = [];

        public LightComponent()
        {
            _lightMap = GameContext.GameResource.Load<Texture2D>("imgs\\homelander.jpg");
            //_lightMap = GameContext.GameResource.Load<Texture2D>("internal\\lightmap.png");
        }

        internal void InternalDraw(SpriteBatch sp, Effect fx)
        {
            //Vector2 position = new(
            //    AttachedEntity.Transform.Position.X, 
            //    AttachedEntity.Transform.Position.Y);

            ////fx.Parameters["LightRadius"]?.SetValue(1f);

            Matrix projection = Matrix.CreateOrthographicOffCenter(0, 200, 200, 0, 0, 1);

            fx.Parameters["view_projection"].SetValue(Camera.Main.ViewMatrix());
            sp.Begin(effect: fx);
            sp.Draw(_lightMap, Vector2.Zero, Color.White);
            sp.End();
        }

        Texture2D _lightMap;
    }
}