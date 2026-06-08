using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace BackgroundEffectsPack
{
    public class BackgroundManager : MonoBehaviour
    {
        [SerializeField] private List<Data> backgroundObjects;
        [SerializeField] private float spawnRate;

        private List<WeightedRandom.WeightedObject<Data>> _weightedData;
        private float _timer = 0;

        private void Awake()
        {
            //_weightedData = backgroundObjects.Select(e => new WeightedRandom.WeightedObject<Data>(e.obj, e.weight));
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer < 1 / spawnRate) return;

            _timer = 0;
            //Instantiate()
        }

        [System.Serializable]
        public class Data
        {
            public float weight;
            public BackgroundObj obj;
        }
    }
}