namespace CruZ.GameEngine.GameSystem.Scene
{
    [System.AttributeUsage(System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class SceneFactoryMethodAttribute
        : System.Attribute
    {
        public SceneFactoryMethodAttribute(string Id = "")
        {
            this.Id = Id;
        }

        public string Id { get; }
    }
}