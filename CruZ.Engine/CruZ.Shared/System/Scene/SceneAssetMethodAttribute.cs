namespace CruZ.Scene
{
    [System.AttributeUsage(System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class SceneAssetMethodAttribute : System.Attribute
    {
        public SceneAssetMethodAttribute(string assetMethodId = "_")
        {
            AssetMethodId = assetMethodId;
        }

        public string AssetMethodId { get; }
    }
}