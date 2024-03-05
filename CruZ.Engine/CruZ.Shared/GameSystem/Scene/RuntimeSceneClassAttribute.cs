namespace CruZ.Scene
{
    [global::System.AttributeUsageAttribute(global::System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class SceneAssetClassAttribute
        : global::System.Attribute
    {
        public SceneAssetClassAttribute(string assetClassId = "")
        {
            AssetClassId = assetClassId;
        }

        public string AssetClassId { get; }
    }
}