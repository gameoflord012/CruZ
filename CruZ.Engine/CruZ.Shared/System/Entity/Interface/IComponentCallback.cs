namespace CruZ.Components
{
    public interface IComponentCallback
    {
        public void OnAttached(TransformEntity entity);
        public void OnDettached(TransformEntity entity);
    }
}