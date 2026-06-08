using System.Collections.Generic;
using System.Linq;
using AudioPack;
using CurtainsPack;
using UIPack;
using UnityEngine;
using UnityEngine.Rendering;
using MonoBehaviour = UnityEngine.MonoBehaviour;

namespace GameManagerPack
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Volume volume;
        [SerializeField] private GameController gameController;
        [SerializeField] private List<GameObject> togleList;
        [SerializeField] private ParticleSystem confetti;

        public static Volume Volume => Instance.volume;
        
        private Dictionary<string, GameObject> _prefabs = new();
        public static bool HasInstance() => Instance != null;

        private static GameManager Instance { get; set; }

        private bool _ready = false;
        

        private EScene _currentScene;
        public static EScene CurrentScene => Instance._currentScene;

        private readonly HashSet<object> _pauseInstances = new();

        private void Awake()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            if (Instance != this && Instance != null) Destroy(gameObject);
            else Instance = this;

            _prefabs.Clear();
            var prefabs = Resources.LoadAll<GameObject>("Prefabs");
            foreach (var prefab in prefabs) _prefabs.Add(prefab.name, prefab);
            
            var managers = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IMainManager>();
            foreach (var mainManager in managers) mainManager.Init();

            AudioManager.SetTheme(ESoundType.MenuMusic);
            UIManager.InstantiateUI<MenuUI>(PrefabNames.MenuUI);
        }

        public static void ToggleConfetti(bool enable)
        {
            if (enable) Instance.confetti.Play();
            else Instance.confetti.Stop();
        }

        public static void StartGame()
        {
            CurtainsManager.Out();
            AudioManager.SetTheme(ESoundType.MainTheme);
            Instance.gameController.StartRun();
        }
        
        public static void ToggleList()
        {
            foreach (var obj in Instance.togleList)
            {
                if (obj == null) continue;
                obj.SetActive(!obj.activeSelf);
            }
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