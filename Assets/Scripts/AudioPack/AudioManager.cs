using System;
using System.Collections.Generic;
using System.Linq;
using GameManagerPack;
using PoolPack;
using UnityEngine;
using UnityEngine.Pool;

namespace AudioPack
{
    public class AudioManager : PoolManager, IMainManager
    {
        [SerializeField] private float minSFXInterval = 0.5f;
        [SerializeField] private List<SoundData> sounds = new();

        private AudioSource mainAudio = null;

        private ObjectPool<SFXPoolObject> _sfxPool;

        public IEnumerable<SoundData> AllSounds => sounds;
        
        #region Singleton

        private static AudioManager Instance { get; set; }

        public void Init()
        {
            if (Instance != this && Instance != null) Destroy(gameObject);
            else Instance = this;
            
            var sfxPrefab = GameManager.GetPrefab<SFXPoolObject>(PrefabNames.SFX);
            _sfxPool = PoolHelper.CreatePool(this, sfxPrefab, false);

            foreach (ESoundType soundType in Enum.GetValues(typeof(ESoundType)))
                _soundEffectTimes.Add(soundType, -999999);
        }

        #endregion

        private Dictionary<ESoundType, float> _soundEffectTimes = new();
        
        public override void ClearAll() => ClearAll(_sfxPool);

        public static void PlaySound(ESoundType soundType)
        {
            if (Time.unscaledTime - Instance._soundEffectTimes[soundType] < Instance.minSFXInterval) return;

            Instance._soundEffectTimes[soundType] = Time.unscaledTime;
            Instance._sfxPool.Get().Play(soundType);
        }

        public static void SetTheme(ESoundType soundType)
        {
            var audioSource = Instance.mainAudio;
            if (audioSource == null)
            {
                var audioSourceObj = new GameObject($"Audio: {soundType}", typeof(AudioSource));
                audioSource = audioSourceObj.GetComponent<AudioSource>();
                Instance.mainAudio = audioSource;
            }
            
            var clip = Instance.sounds.FirstOrDefault(s => s.SoundType == soundType)?.AudioClip;
            if (clip == null) return;

            audioSource.clip = clip;
            audioSource.volume = 0.3f;
            audioSource.loop = true;
            audioSource.Play();
        }

        public static void StopTheme()
        {
            var audioSource = Instance.mainAudio;
            if (audioSource == null) return;
            
            audioSource.Stop();
        }

        public override void ReleasePoolObject(PoolObject poolObject)
        {
            _sfxPool.Release(poolObject as SFXPoolObject);
        }

#if UNITY_EDITOR
        public List<SoundData> GetSoundDataList() => sounds;

        public SoundData GetSound(ESoundType soundType) => sounds.FirstOrDefault(e => e.SoundType == soundType);

        public void SetSoundDataList(IEnumerable<SoundData> soundDataList) => sounds = new List<SoundData>(soundDataList);
#endif
    }
}