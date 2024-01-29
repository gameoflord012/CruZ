using CruZ.Components;
using CruZ.Systems;

namespace CruZ.UI
{
    public class EntityControl : UIControl
    {
        public EntityControl(TransformEntity e)
        {
            _e = e;
        }

        public override void Update(UIArgs args)
        {
            base.Update(args);

            Location = Camera.Main.CoordinateToPoint(_e.Transform.Position);
            Width = 100;
            Height = 100;
        }

        TransformEntity _e;
    }
}