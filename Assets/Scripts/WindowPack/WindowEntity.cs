using System;
using AudioPack;
using DamageIndicatorPack;
using UnityEngine;
using Utils;

namespace WindowPack
{
    public class WindowEntity : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer dirtySpriteRenderer;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private int maxDirtValue;
        [SerializeField] private MinMax dirtRange;
        [SerializeField] private MinMax cleanRange;
        [SerializeField] private Sprite windowPlayer1;
        [SerializeField] private Sprite windowPlayer2;
        [SerializeField] private Color dirtyColor1;
        [SerializeField] private Color dirtyColor2;

        private int _dirtValue = 0;

        public void Setup(int playerID)
        {
            spriteRenderer.sprite = playerID == 0 ? windowPlayer1 : windowPlayer2;
            dirtySpriteRenderer.color = playerID == 0 ? dirtyColor1 : dirtyColor2;
            dirtySpriteRenderer.flipX = playerID == 1;
        }

        private void Update()
        {
            var color = dirtySpriteRenderer.color;
            color.a = _dirtValue / (float)maxDirtValue;
            dirtySpriteRenderer.color = color;
        }

        public int Points => maxDirtValue - _dirtValue;

        public void Clean()
        {
            var clean = cleanRange.RandomInt();
            _dirtValue -= clean;
            _dirtValue = Mathf.Max(0, _dirtValue);
            
            AudioManager.PlaySound(ESoundType.Clean);
            
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