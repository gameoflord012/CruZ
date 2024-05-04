using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruZ.GameEngine.GameSystem.StateMachine
{
    internal class StateMachineSystem : EntitySystem
    {
        protected override void OnUpdate(EntitySystemEventArgs args)
        {
            base.OnUpdate(args);

            foreach (var machine in 
                args.ActiveEntities.GetAllComponents<StateMachineComponent>())
            {
                machine.DoUpdate(args.GameTime);
            }
        }

        protected override void OnDraw(EntitySystemEventArgs args)
        {
            base.OnDraw(args);

            foreach (var machine in
                args.ActiveEntities.GetAllComponents<StateMachineComponent>())
            {
                machine.DoDraw(args.GameTime);
            }
        }
    }
}
