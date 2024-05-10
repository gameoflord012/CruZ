namespace CruZ.GameEngine.GameSystem
{
    public interface IPoolObject
    {
        Pool Pool { get; set; }

        public void OnDisabled();
    }
}
