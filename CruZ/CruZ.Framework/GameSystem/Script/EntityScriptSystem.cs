using CruZ.Framework.GameSystem.ECS;

using Microsoft.Xna.Framework;

namespace CruZ.Common.ECS
{
    internal class EntityScriptSystem : EntitySystem
    {
        public void Draw(GameTime gameTime)
        {
            //foreach (var script in this.GetAllComponents(_scriptMapper))
            //{
            //    script.InternalDraw(gameTime);
            //}
        }

        public void Update(GameTime gameTime)
        {
            //foreach (var script in this.GetAllComponents(_scriptMapper))
            //{
            //    script.InternalUpdate(gameTime);
            //}
        }

        //ComponentMapper<EntityScript> _scriptMapper;
    }
}