namespace CruZ.Scene
{
    [System.AttributeUsage(System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class SceneAssetMethodAttribute
        : System.Attribute
    {
        public SceneAssetMethodAttribute(string assetMethodId = "")
        {
            AssetMethodId = assetMethodId;
        }

        public string AssetMethodId { get; }
    }
}