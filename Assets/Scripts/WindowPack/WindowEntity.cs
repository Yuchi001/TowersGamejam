using System;
using DamageIndicatorPack;
using UnityEngine;
using Utils;

namespace WindowPack
{
    public class WindowEntity : MonoBehaviour
    {
        [SerializeField] private GameObject dirtySpriteObject;
        [SerializeField] private int maxDirtValue;
        [SerializeField] private MinMax dirtRange;
        [SerializeField] private MinMax cleanRange;

        private int _dirtValue = 0;

        private void Update()
        {
            dirtySpriteObject.SetActive(_dirtValue > 0);
        }

        public void Clean()
        {
            var clean = cleanRange.RandomInt();
            _dirtValue -= clean;
            _dirtValue = Mathf.Min(0, _dirtValue);
            
            if (_dirtValue == 0) IndicatorManager.SpawnIndicator(transform.position, EIndicatorText.CLEAN);
            else IndicatorManager.SpawnIndicator(transform.position, clean, cleanRange.MaxInt, clean == cleanRange.MaxInt, false);
        }

        public void DirtyUp()
        {
            var dirt = dirtRange.RandomInt();
            _dirtValue += dirt;
            _dirtValue = Mathf.Max(maxDirtValue, _dirtValue);
            
            IndicatorManager.SpawnIndicator(transform.position, dirt, dirtRange.MaxInt, dirt == dirtRange.MaxInt, false);
        }
    }
}