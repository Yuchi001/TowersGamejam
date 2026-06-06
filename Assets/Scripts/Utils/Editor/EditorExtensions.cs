using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Utils.Editor
{
    public static class EditorExtensions
    {
        public static void DrawExcept(this UnityEditor.Editor editor, HashSet<string> exceptFields)
        {
            var serializedObject = editor.serializedObject;
            serializedObject.Update();

            var prop = serializedObject.GetIterator();
            var enterChildren = true;

            while (prop.NextVisible(enterChildren))
            {
                if (exceptFields.Contains(prop.name))
                {
                    enterChildren = false;
                    continue;
                }
                
                if (prop.name == "m_Script")
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.PropertyField(prop, true);
                    EditorGUI.EndDisabledGroup();
                    continue;
                }

                EditorGUILayout.PropertyField(prop, true);
                enterChildren = false;
            }

            serializedObject.ApplyModifiedProperties();
        }

        public static void DrawSubAsset<T>(this UnityEditor.Editor editor, T parent) where T: SoSubAssetParent
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField("Strategy Data", EditorStyles.boldLabel); 
            
            var newData = EditorGUILayout.EnumPopup(typeof(T).Name[2..], parent.AssetType);

            if (!newData.Equals(parent.AssetType)) parent.SetNewData(newData);

            var subAsset = parent.SubAsset;
            if (subAsset == null) return;
                
            var serializedStrategy = new SerializedObject(subAsset);
            var iterator = serializedStrategy.GetIterator();
                    
            iterator.NextVisible(true);
            while (iterator.NextVisible(false))
            {
                EditorGUILayout.PropertyField(iterator, true);
            }
            serializedStrategy.ApplyModifiedProperties();
            
            editor.serializedObject.ApplyModifiedProperties();
        }
        
        public static void DrawUILine(this UnityEditor.Editor editor, float padding = 10)
        {
            var r = EditorGUILayout.GetControlRect(GUILayout.Height(padding+1));
            r.height = 1;
            r.y+= padding / 2;
            r.x-=2;
            r.width +=6;
            EditorGUI.DrawRect(r, new Color32(26, 26, 26, 255));
        }

        public static bool TrashButton(this UnityEditor.Editor editor, float size = 20f)
        {
            var icon = EditorGUIUtility.IconContent("TreeEditor.Trash").image;

            // Przydziel prostokąt (tyle ile trzeba na przycisk)
            Rect rect = GUILayoutUtility.GetRect(size, size, GUILayout.Width(size), GUILayout.Height(size));

            // Rysuj przycisk z tłem i stylem
            if (GUI.Button(rect, GUIContent.none))
            {
                return true;
            }

            // Oblicz wewnętrzny środek i narysuj ikonę na środku
            float padding = 4f;
            float iconSize = size - padding * 2;
            Rect iconRect = new Rect(
                rect.x + padding,
                rect.y + padding,
                iconSize,
                iconSize
            );

            GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit, true);

            return false;
        }
    }
}