using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Utils;
using Utils.Editor;

namespace AudioPack.Editor
{
    [CustomEditor(typeof(AudioManager))]
    public class CeAudioManager : UnityEditor.Editor
    {
        private HashSet<string> EXCEPT_FIELDS = new()
        {
            "sounds"
        };
        
        private AudioManager _audioManager;

        private string _searchInput = "";
        private EFilterType _filterType = EFilterType.NONE;
        
        private void OnEnable()
        {
            _audioManager = (AudioManager)target;
        }

        public override void OnInspectorGUI()
        {
            this.DrawExcept(EXCEPT_FIELDS);
            
             if (GUILayout.Button("ADD SOUND")) SFXWindow.OpenSFXWindow(_audioManager);
            
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            _searchInput = EditorGUILayout.TextField("Search", _searchInput);
            
            if (this.TrashButton())
            {
                _searchInput = "";
                GUI.FocusControl(null);
            }
            EditorGUILayout.EndHorizontal();
            _filterType = (EFilterType)EditorGUILayout.EnumPopup("Filter", _filterType);
            
            EditorGUILayout.Space();
            
            this.DrawUILine(5);
            
            var sounds = new List<SoundData>();
            var soundTypes = (ESoundType[])Enum.GetValues(typeof(ESoundType));
            Array.Sort(soundTypes, (a, b) => string.Compare(a.ToString(), b.ToString(), StringComparison.Ordinal));
            foreach (var soundType in soundTypes)
            {
                if (soundType == ESoundType.None) continue;
                
                var visible = _searchInput == "" || soundType.ToString().Contains(_searchInput, StringComparison.OrdinalIgnoreCase);
                
                var title = soundType.ToString().SplitCamelCase();
                var soundData = _audioManager.GetSound(soundType);
                var fetchedClip = soundData?.AudioClip;

                if (_filterType == EFilterType.SET) visible = visible && fetchedClip;
                else if (_filterType == EFilterType.NOT_SET) visible = visible && !fetchedClip;

                if (!visible)
                {
                    var current = _audioManager.GetSound(soundType) ?? new SoundData(soundType);
                    sounds.Add(current);
                    continue;
                }

                EditorGUILayout.BeginHorizontal();
                
                var normalColor = GUI.contentColor;
                GUI.contentColor = fetchedClip == null ? Color.red : normalColor;
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
                GUI.contentColor = normalColor;
                
                var newClip = (AudioClip)EditorGUILayout.ObjectField(fetchedClip, typeof(AudioClip), true);

                if (this.TrashButton()) RemoveEntryFromEnum(soundType.ToString());
                
                sounds.Add(new SoundData(soundType, newClip));
                
                EditorGUILayout.EndHorizontal();
                
                this.DrawUILine(5);
            }
            
            _audioManager.SetSoundDataList(sounds);
            
            if (GUI.changed)
            {
                Undo.RecordObject(_audioManager, "Audio Manager Changed");
                EditorUtility.SetDirty(_audioManager);
                serializedObject.ApplyModifiedProperties();
            }
        }

        private enum EFilterType
        {
            NONE,
            SET,
            NOT_SET,
        }
        
        private void RemoveEntryFromEnum(string entryName)
        {
            var enumFilePath = Path.Combine(Application.dataPath, "Scripts/AudioPack/ESoundType.cs");

            if (!File.Exists(enumFilePath))
            {
                Debug.LogError("Plik ESoundType.cs nie został znaleziony.");
                return;
            }

            var lines = File.ReadAllLines(enumFilePath).ToList();
            var enumStartIndex = -1;
            var enumEndIndex = -1;

            for (var i = 0; i < lines.Count; i++)
            {
                if (lines[i].Contains("public enum ESoundType"))
                    enumStartIndex = i;

                if (enumStartIndex != -1 && lines[i].Contains("}"))
                {
                    enumEndIndex = i;
                    break;
                }
            }

            if (enumStartIndex == -1 || enumEndIndex == -1)
            {
                Debug.LogError("Nie znaleziono definicji enumu ESoundType w pliku.");
                return;
            }

            for (var i = enumStartIndex + 1; i < enumEndIndex; i++)
            {
                var line = lines[i].Trim().TrimEnd(',');
                if (line.Equals(entryName))
                {
                    lines.RemoveAt(i);
                    break;
                }
            }

            File.WriteAllLines(enumFilePath, lines);
            Debug.Log($"Usunięto {entryName} z enumu ESoundType.");
            AssetDatabase.Refresh();
            UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
        }

        private class SFXWindow : EditorWindow
        {
            private AudioManager _audioManager;
            private AudioClip _currentClip;
            private string _currentClipName = "";

            public static void OpenSFXWindow(AudioManager audioManager)
            {
                var window = (SFXWindow)GetWindow(typeof(SFXWindow));
                window.minSize = window.maxSize = new Vector2(300, 100);
                window._audioManager = audioManager;
            }

            private void OnGUI()
            {
                if (_currentClip != null) _currentClipName = EditorGUILayout.TextField("Clip Name", _currentClipName);
                _currentClip = (AudioClip)EditorGUILayout.ObjectField("SFX", _currentClip, typeof(AudioClip), false);
                if (_currentClip == null) return;
                
                if (_currentClipName == "") _currentClipName = _currentClip.name;

                _currentClipName = Regex.Replace(_currentClipName, @"\s+", "");

                if (!GUILayout.Button("SAVE")) return;

                var success = AddEntry();
                if (!success) return;
                
                EditorPrefs.SetString("AudioManager_PendingClipName", _currentClipName);
                EditorPrefs.SetString("AudioManager_PendingClipPath", AssetDatabase.GetAssetPath(_currentClip));

                AssetDatabase.Refresh();
                UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
                
                Close();
            }

            private bool AddEntry()
            {
                if (!Regex.IsMatch(_currentClipName, @"^[A-Za-z_][A-Za-z0-9_]*$"))
                {
                    Debug.LogError("Nieprawidłowa nazwa dla enum (tylko litery, cyfry, podkreślenia, nie może zaczynać się od cyfry).");
                    return false;
                }
                
                var enumFilePath = Path.Combine(Application.dataPath, "Scripts/AudioPack/ESoundType.cs");

                if (!File.Exists(enumFilePath))
                {
                    Debug.LogError("Nie znaleziono pliku ESoundType.cs");
                    return false;
                }

                var lines = File.ReadAllLines(enumFilePath).ToArray();
                var enumStartIndex = -1;
                var enumEndIndex = -1;

                for (var i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("public enum ESoundType"))
                        enumStartIndex = i;

                    if (enumStartIndex != -1 && lines[i].Contains("}"))
                    {
                        enumEndIndex = i;
                        break;
                    }
                }

                var existingNames = new HashSet<string>();

                for (var i = enumStartIndex + 1; i < enumEndIndex; i++)
                {
                    var line = lines[i].Trim().TrimEnd(',');
                    if (!string.IsNullOrEmpty(line))
                        existingNames.Add(line);
                }

                if (existingNames.Contains(_currentClipName))
                {
                    Debug.LogWarning("Ta wartość już istnieje w ESoundType.");
                    return false;
                }
               
                var updatedLines = lines.ToList();
                updatedLines.Insert(enumEndIndex, $"    {_currentClipName} = {EnumExtensions.Count<ESoundType>() + 1},");
                File.WriteAllLines(enumFilePath, updatedLines);

                Debug.Log($"Dodano {_currentClipName} do ESoundType.");

                AssetDatabase.Refresh();
                UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
                return true;
            }

            [InitializeOnLoad]
            public static class PostLoad
            {
                static PostLoad()
                {
                    EditorApplication.update += OnEditorUpdate;
                }

                private static void OnEditorUpdate()
                {
                    if (!EditorPrefs.HasKey("AudioManager_PendingClipName") ||
                        !EditorPrefs.HasKey("AudioManager_PendingClipPath")) return;


                    var clipName = EditorPrefs.GetString("AudioManager_PendingClipName");
                    var clipPath = EditorPrefs.GetString("AudioManager_PendingClipPath");
                    var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(clipPath);

                    if (!Enum.GetNames(typeof(ESoundType)).Contains(clipName)) return;
                    
                    var audioManager = FindObjectOfType<AudioManager>();
                    if (Enum.TryParse(typeof(ESoundType), clipName, out var result))
                    {
                        var soundType = (ESoundType)result;
                        if (audioManager != null)
                        {
                            var sounds = new List<SoundData>(audioManager.GetSoundDataList());
                            sounds.Add(new SoundData(soundType, clip));
                            audioManager.SetSoundDataList(sounds);
                            EditorUtility.SetDirty(audioManager);
                            Debug.Log($"🎉 Zaktualizowano AudioManager po rekompilacji. Dodano {clipName}.");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Nie udało się sparsować nowego enumu po rekompilacji.");
                    }

                    EditorPrefs.DeleteKey("AudioManager_PendingClipName");
                    EditorPrefs.DeleteKey("AudioManager_PendingClipPath");

                    EditorApplication.update -= OnEditorUpdate;
                }
            }
        }
    }
}