using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Utils.Editor
{
    public class SpriteSplitter : MonoBehaviour
    {
         [MenuItem("Assets/Split Sprites")]
        static void SplitAndReplace()
        {
            foreach (var obj in Selection.objects)
            {
                string atlasPath = AssetDatabase.GetAssetPath(obj);
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(atlasPath);
                TextureImporter ti = AssetImporter.GetAtPath(atlasPath) as TextureImporter;

                if (ti != null && ti.spriteImportMode == SpriteImportMode.Multiple)
                {
                    string folderPath = Path.GetDirectoryName(atlasPath);

                    // Wczytanie wszystkich slice'ów
                    Sprite[] slices = AssetDatabase.LoadAllAssetsAtPath(atlasPath)
                                       .OfType<Sprite>()
                                       .ToArray();

                    foreach (Sprite s in slices)
                    {
                        Rect r = s.rect;
                        Texture2D newTex = new Texture2D((int)r.width, (int)r.height, texture.format, false);
                        Color[] pixels = s.texture.GetPixels(
                            (int)r.x,
                            (int)r.y,
                            (int)r.width,
                            (int)r.height);
                        newTex.SetPixels(pixels);
                        newTex.Apply();

                        string newPath = Path.Combine(folderPath, s.name + ".png");
                        File.WriteAllBytes(newPath, newTex.EncodeToPNG());
                        AssetDatabase.ImportAsset(newPath);

                        // Skopiowanie ustawień z oryginalnego atlasu
                        TextureImporter newTI = AssetImporter.GetAtPath(newPath) as TextureImporter;
                        if (newTI != null)
                        {
                            newTI.textureType = ti.textureType;
                            newTI.spritePixelsPerUnit = ti.spritePixelsPerUnit;
                            newTI.mipmapEnabled = ti.mipmapEnabled;
                            newTI.filterMode = ti.filterMode;
                            newTI.wrapMode = ti.wrapMode;
                            newTI.textureCompression = ti.textureCompression;
                            newTI.spriteImportMode = SpriteImportMode.Single; // Single dla nowych sprite'ów
                            newTI.SaveAndReimport();
                        }
                    }

                    // Usunięcie oryginalnego atlasu
                    AssetDatabase.DeleteAsset(atlasPath);
                    Debug.Log("Atlas replaced with individual sprites with copied settings: " + atlasPath);
                }
            }

            AssetDatabase.Refresh();
        }
    }
}