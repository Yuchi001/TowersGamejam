using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Utils.Editor
{
    public class SpriteToAnimationClip : MonoBehaviour
    {
        [MenuItem("Assets/Create Animation Clip From Sprites")]
        private static void CreateOrOverwriteAnimationClip()
        {
            // Lista do zbierania sprite'ów
            var spritesList = new System.Collections.Generic.List<Sprite>();

            foreach (var obj in Selection.objects)
            {
                if (obj is Sprite sprite)
                {
                    spritesList.Add(sprite);
                }
                else if (obj is Texture2D tex)
                {
                    // Pobieramy wszystkie sprite'y z tekstury
                    string path = AssetDatabase.GetAssetPath(tex);
                    var assets = AssetDatabase.LoadAllAssetsAtPath(path);
                    var texSprites = assets.OfType<Sprite>();
                    spritesList.AddRange(texSprites);
                }
            }

            var sprites = spritesList.ToArray();
            if (sprites.Length == 0)
            {
                Debug.LogWarning("No sprites detected in selection!");
                return;
            }

            // Sortowanie po numerze w nazwie (np. AirA_0, AirA_1, ...)
            sprites = sprites.OrderBy(s => ExtractNumber(s.name)).ToArray();

            // Nazwa clipu na podstawie pierwszego sprite'a
            string firstName = sprites[0].name;
            string clipName = firstName.Contains("_") ? firstName.Split('_')[0] : firstName;

            // Folder docelowy
            string folder = "Assets/Animations";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            // Szukamy rekurencyjnie w folderze, czy clip już istnieje
            string[] guids = AssetDatabase.FindAssets(clipName + " t:AnimationClip", new[] { folder });
            string pathClip;
            AnimationClip clip = null;

            if (guids.Length > 0)
            {
                pathClip = AssetDatabase.GUIDToAssetPath(guids[0]);
                clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(pathClip);
                Debug.Log("Overwriting existing AnimationClip: " + clipName);
            }
            else
            {
                pathClip = Path.Combine(folder, clipName + ".anim");
                pathClip = AssetDatabase.GenerateUniqueAssetPath(pathClip);
                clip = new AnimationClip();
                clip.frameRate = 12;
                AssetDatabase.CreateAsset(clip, pathClip);
                Debug.Log("Created new AnimationClip: " + clipName);
            }

            // Tworzymy keyframe'y dla SpriteRenderer
            EditorCurveBinding spriteBinding = new EditorCurveBinding();
            spriteBinding.type = typeof(SpriteRenderer);
            spriteBinding.path = "";
            spriteBinding.propertyName = "m_Sprite";

            ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[sprites.Length];
            for (int i = 0; i < sprites.Length; i++)
            {
                keyframes[i] = new ObjectReferenceKeyframe
                {
                    time = i / clip.frameRate,
                    value = sprites[i]
                };
            }

            clip.wrapMode = WrapMode.Loop;
            AnimationUtility.SetObjectReferenceCurve(clip, spriteBinding, keyframes);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("AnimationClip ready: " + clipName);
        }

        // Funkcja pomocnicza do wyciągania numeru z nazwy (np. AirA_0 → 0)
        static int ExtractNumber(string name)
        {
            var match = Regex.Match(name, @"_(\d+)$");
            if (match.Success && int.TryParse(match.Groups[1].Value, out int num))
                return num;
            return 0;
        }
    }
}
