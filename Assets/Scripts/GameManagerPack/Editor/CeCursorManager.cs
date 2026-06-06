using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Utils.Editor;

namespace GameManagerPack.Editor
{
    [CustomEditor(typeof(CursorManager))]
    public class CeCursorManager : UnityEditor.Editor
    {
        private static readonly HashSet<string> _except = new()
        {
            "states",
        };

        private CursorManager _cursorManager;

        private void OnEnable()
        {
            _cursorManager = (CursorManager)target;
        }

        public override void OnInspectorGUI()
        {
            this.DrawExcept(_except);

            var states = new List<CursorManager.CursorState>();
            foreach (CursorManager.EState stateType in Enum.GetValues(typeof(CursorManager.EState)))
            {
                var currentStateObj = _cursorManager.GetStateObject(stateType);
                EditorGUILayout.LabelField(stateType.ToString().ToLower().FirstCharacterToUpper(), EditorStyles.boldLabel);
                EditorGUILayout.BeginHorizontal();
                var sprite = EditorGUILayout.ObjectField(currentStateObj?.Sprite, typeof(Sprite), false);
                var canInterupt = EditorGUILayout.Toggle("Can Interrupt", currentStateObj?.CanInterupt ?? false);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                
                states.Add(new CursorManager.CursorState(
                    stateType,
                    (Sprite)sprite,
                    canInterupt
                ));
            }
            
            _cursorManager.SetStatesList(states);
            

            if (GUI.changed)
            {
                EditorUtility.SetDirty(_cursorManager);
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}