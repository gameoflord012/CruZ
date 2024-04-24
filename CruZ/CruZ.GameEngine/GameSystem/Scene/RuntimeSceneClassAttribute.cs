namespace CruZ.GameEngine.GameSystem.Scene
{
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class SceneFactoryClassAttribute
        : System.Attribute
    {
        public SceneFactoryClassAttribute(string Id = "")
        {
            this.Id = Id;
        }

        public string Id { get; }
    }
}