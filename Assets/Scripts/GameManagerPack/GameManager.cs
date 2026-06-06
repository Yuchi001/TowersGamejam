using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using MonoBehaviour = UnityEngine.MonoBehaviour;

namespace GameManagerPack
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Volume volume;

        public static Volume Volume => Instance.volume;
        
        private readonly Dictionary<string, GameObject> _prefabs = new();
        public static bool HasInstance() => Instance != null;

        private static GameManager Instance { get; set; }

        private bool _ready = false;
        

        private EScene _currentScene;
        public static EScene CurrentScene => Instance._currentScene;

        private readonly HashSet<object> _pauseInstances = new();

        private void Awake()
        {
            if (Instance != this && Instance != null) Destroy(gameObject);
            else Instance = this;
        }
       
        public static void PauseGame(object obj)
        {
            Instance._pauseInstances.Add(obj);
            Time.timeScale = 0;
        }

        public static void ResumeGame(object obj)
        {
            Instance._pauseInstances.Remove(obj);
            Time.timeScale = Instance._pauseInstances.Any() ? 0 : 1;
        }

        public static T GetPrefab<T>(string prefName) where T : class
        {
            var hasValue = Instance._prefabs.TryGetValue(prefName, out var prefab);
            return hasValue ? prefab.GetComponent<T>() : null;
        }
        
        public static GameObject GetPrefab(string prefName)
        {
            Instance._prefabs.TryGetValue(prefName, out var prefab);
            return prefab;
        }
        
        public static List<T> GetPrefabs<T>(string prefName) where T : class
        {
            var prefabs = new List<T>();
            foreach (var key in Instance._prefabs.Keys)
            {
                if (!key.ToLower().StartsWith(prefName.ToLower()) || 
                    !Instance._prefabs[key].TryGetComponent(out T prefab)) continue;
                prefabs.Add(prefab);
            }

            return prefabs;
        }
        
        public enum EScene
        {
            MENU = 0,
            TAVERN = 1,
            MAIN = 2,
            MAP = 3,
            GAME = 4,
            TUTORIAL = 5
        }
    }
}