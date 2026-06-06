using UnityEngine;

namespace AudioPack
{
    [System.Serializable]
    public class SoundData
    {
        [SerializeField] private AudioClip audioClip;
        [SerializeField] private ESoundType soundType;

        public AudioClip AudioClip => audioClip;
        public ESoundType SoundType => soundType;

        public SoundData(ESoundType soundType, AudioClip audioClip = null)
        {
            this.audioClip = audioClip;
            this.soundType = soundType;
        }
    }
}