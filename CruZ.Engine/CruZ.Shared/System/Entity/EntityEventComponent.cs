//using Microsoft.Xna.Framework;
//using System;

//namespace CruZ.Components
//{
//    public class EntityEventComponent : IComponent
//    {
//        public Type ComponentType => typeof(EntityEventComponent);

//        //public event Action?            OnStart;
//        public event Action<GameTime>?  OnUpdate;
//        public event Action<GameTime>?  OnDraw;

//        public void InvokeOnStart()                     { /*OnStart?.Invoke();*/ }
//        public void InvokeOnUpdate(GameTime gameTime)   { OnUpdate?.Invoke(gameTime); }
//        public void InvokeOnDraw(GameTime gameTime)     { OnDraw?.Invoke(gameTime); }
//    }
//}