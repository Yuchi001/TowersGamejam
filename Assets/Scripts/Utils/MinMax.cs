using UnityEngine;
using UnityEngine.Serialization;

namespace Utils
{
    [System.Serializable]
    public class MinMax
    {
        [SerializeField] private float min;
        [SerializeField] private float max;

        public MinMax(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public int RandomInt()
        {
            return Random.Range((int)min, (int)max + 1);
        }

        public float RandomFloat()
        {
            return Random.Range(min, max);
        }

        public float Lerp(float time)
        {
            return Mathf.Lerp(min, max, time);
        }

        public int LerpInt(float time)
        {
            return (int)Mathf.Lerp(min, max, time);
        }

        public float Min => min;
        public float Max => max;
        
        public int MinInt => (int)min;
        public int MaxInt => (int)max;
    }
}