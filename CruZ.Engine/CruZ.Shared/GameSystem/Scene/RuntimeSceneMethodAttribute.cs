namespace CruZ.Common.Scene
{
    [global::System.AttributeUsageAttribute(global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class SceneAssetMethodAttribute
        : global::System.Attribute
    {
        public SceneAssetMethodAttribute(string assetMethodId = "")
        {
            AssetMethodId = assetMethodId;
        }

        public string AssetMethodId { get; }
    }
}