namespace CruZ.GameEngine.GameSystem.Scene
{
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class GameSceneDecoratorAttribute : System.Attribute
    {
        public GameSceneDecoratorAttribute(string Id = "Scene Decorator")
        {
            this.Id = Id;
        }

        public string Id { get; }
    }
}
