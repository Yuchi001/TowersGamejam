using System;
using DamageIndicatorPack;
using UnityEngine;
using Utils;

namespace WindowPack
{
    public class WindowEntity : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer dirtySpriteRenderer;
        [SerializeField] private int maxDirtValue;
        [SerializeField] private MinMax dirtRange;
        [SerializeField] private MinMax cleanRange;

        private int _dirtValue = 0;

        private void Update()
        {
            var color = dirtySpriteRenderer.color;
            color.a = _dirtValue / (float)maxDirtValue;
            dirtySpriteRenderer.color = color;
        }

        public void Clean()
        {
            var clean = cleanRange.RandomInt();
            _dirtValue -= clean;
            _dirtValue = Mathf.Max(0, _dirtValue);
            
            if (_dirtValue == 0) IndicatorManager.SpawnIndicator(transform.position, EIndicatorText.CLEAN);
            else IndicatorManager.SpawnIndicator(transform.position, clean, cleanRange.MaxInt, clean == cleanRange.MaxInt, true);
        }

        public void DirtyUp()
        {
            var dirt = dirtRange.RandomInt();
            _dirtValue += dirt;
            _dirtValue = Mathf.Min(maxDirtValue, _dirtValue);
            
            IndicatorManager.SpawnIndicator(transform.position, dirt, dirtRange.MaxInt, dirt == dirtRange.MaxInt, false);
        }
    }
}