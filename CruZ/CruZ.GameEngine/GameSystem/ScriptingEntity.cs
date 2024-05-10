using System;

using CruZ.GameEngine.GameSystem.Scene;
using CruZ.GameEngine.GameSystem.Script;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem
{
    public class ScriptingEntity : IDisposable
    {
        public ScriptingEntity(GameScene gameScene)
        {
            Entity = gameScene.CreateEntity();

            _script = new ScriptComponent();
            {
                _script.Drawing += OnDrawing;
                _script.Updating += OnUpdating;
            }
            Entity.AddComponent(_script);
        }

        protected virtual void OnUpdating(ScriptUpdateArgs args)
        {

        }

        protected virtual void OnDrawing(GameTime time)
        {

        }

        public TransformEntity Entity
        {
            get;
            private set;
        }

        public TransformEntity? ParentEntity
        {
            get => Entity.Parent;
            set => Entity.Parent = value;
        }

        ScriptComponent _script;

        public virtual void Dispose()
        {
            _script.Drawing -= OnDrawing;
            _script.Updating -= OnUpdating;
            Entity.Dispose();
        }
    }
}
