using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utils.Editor;

namespace UIPack.Colors.Editor
{
    [CustomEditor(typeof(SoTierColor))]
    public class CeSoTierColor : UnityEditor.Editor
    {
        private SoTierColor _tierColor;
        private static readonly HashSet<string> _exceptFields = new()
        {
            "tierColors"
        };

        private void OnEnable()
        {
            _tierColor = (SoTierColor)target;
        }

        public override void OnInspectorGUI()
        {
            this.DrawExcept(_exceptFields);

            var colorList = _tierColor.GetColors();
            var currentTier = 1;
            var newColorList = new List<SoTierColor.ListWrapper>();
            var changed = false;
            foreach (var wrapper in colorList)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField($"Tier {(currentTier < colorList.Count ? currentTier.ToString() : $">= {currentTier}")}", EditorStyles.boldLabel);
                var newWrapper = new SoTierColor.ListWrapper();
                foreach (var current in wrapper.colors)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(current.Type.ToString());
                    var color = EditorGUILayout.ColorField("", current.Color);
                    EditorGUILayout.EndHorizontal();
                    if (color != current.Color) changed = true;
                    newWrapper.colors.Add(new SoTierColor.UIColorPair(current.Type, color));
                }
                
                newColorList.Add(newWrapper);
                currentTier++;
            }

            if (!changed) return;
            
            _tierColor.SetTierColors(newColorList);

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(_tierColor);
        }
    }
}