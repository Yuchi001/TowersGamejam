using System.Collections.Generic;
using System.Linq;
using GameManagerPack;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace UIPack
{
    public class BackdropManager : MonoBehaviour, IMainManager
    {
        [SerializeField] private float backdropDepthOfField;
        
        private static BackdropManager Instance { get; set; }

        private Backdrop _backdropPrefab;

        private readonly Dictionary<string, Backdrop> _spawnedBackdrops = new();
        
        private DepthOfField _depthOfField;
        private float _defaultDepthOfField;
        
        public void Init()
        {
            Instance = this;
            _backdropPrefab = GameManager.GetPrefab<Backdrop>(PrefabNames.Backdrop);
            //GameManager.Volume.profile.TryGet(out _depthOfField);
            //_defaultDepthOfField = _depthOfField.focusDistance.value;
        }

        public static void In(string uiKey)
        {
            //Instance._depthOfField.focusDistance.Override(Instance.backdropDepthOfField);
            
            var spawnedBackdrop = Instantiate(Instance._backdropPrefab, UIManager.MainCanvas);
            spawnedBackdrop.transform.SetSiblingIndex(spawnedBackdrop.transform.GetSiblingIndex() - 1);
            spawnedBackdrop.gameObject.LeanValue(0, spawnedBackdrop.EnabledAlpha, 0.3f).setOnUpdate(e =>
            {
                var color = spawnedBackdrop.Image.color;
                color.a = e / 255;
                spawnedBackdrop.Image.color = color;
            }).setIgnoreTimeScale(true);
            Instance._spawnedBackdrops.Add(uiKey, spawnedBackdrop);
        }

        public static void Out(string uiKey)
        {
            if (!Instance._spawnedBackdrops.ContainsKey(uiKey)) return;
            
            var backdrop = Instance._spawnedBackdrops[uiKey];
            Instance._spawnedBackdrops.Remove(uiKey);
            backdrop.gameObject.LeanValue(backdrop.EnabledAlpha, 0, 0.3f).setOnUpdate(e =>
            {
                var color = backdrop.Image.color;
                color.a = e / 255;
                backdrop.Image.color = color;
            }).setIgnoreTimeScale(true).setOnComplete(() => Destroy(backdrop.gameObject));

            /*var depthOfFieldValue = Instance._spawnedBackdrops.Any()
                ? Instance.backdropDepthOfField
                : Instance._defaultDepthOfField;
            Instance._depthOfField.focusDistance.Override(depthOfFieldValue);*/
        }
    }
}