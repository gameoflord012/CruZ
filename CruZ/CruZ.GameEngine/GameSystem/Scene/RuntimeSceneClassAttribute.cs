﻿namespace CruZ.GameEngine.GameSystem.Scene
{
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class SceneAssetClassAttribute
        : System.Attribute
    {
        public SceneAssetClassAttribute(string assetClassId = "")
        {
            AssetClassId = assetClassId;
        }

        public string AssetClassId { get; }
    }
}