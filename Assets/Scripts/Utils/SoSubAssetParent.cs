using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utils
{
    public abstract class SoSubAssetParent : ScriptableObject
    {
        public abstract ScriptableObject SubAsset { get; }
        public abstract Enum AssetType { get; }
        protected abstract void SetCurrentData(Enum data);
        protected abstract bool IsNone(Enum data);

#if UNITY_EDITOR
        public void SetNewData(Enum data)
        {
            if (SubAsset != null)
            {
                var path = AssetDatabase.GetAssetPath(SubAsset);
                var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                DestroyImmediate(asset, true);
            }

            if (!IsNone(data))
            {
                SetCurrentData(data);
                AssetDatabase.AddObjectToAsset(SubAsset, this); 
            }
            
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif
    }
}