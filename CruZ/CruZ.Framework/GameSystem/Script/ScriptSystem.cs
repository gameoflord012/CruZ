using CruZ.Framework.GameSystem.ECS;

using Microsoft.Xna.Framework;

namespace CruZ.Framework.GameSystem.Script
{
    internal class ScriptSystem : EntitySystem
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

        //ComponentMapper<ScriptComponent> _scriptMapper;
    }
}