using System;
using System.Collections.Generic;
using System.Linq;
using PoolPack;
using TMPro;
using UIPack;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DamageIndicatorPack
{
    public class DamageIndicator : SimplePoolObject
    {
        [SerializeField] private TextMeshProUGUI damageText;
        [SerializeField] private float lifeTime = 0.3f;
        [SerializeField] private Animator animator;
        [SerializeField] private List<FontSizeData> fontSizeDataList;

        private float _timer = 0;

        private IndicatorManager _poolManager;

        private Dictionary<EFontSize, float> _fontSizeDataDict;

        private void Awake()
        {
            _fontSizeDataDict = fontSizeDataList.ToDictionary(f => f.SizeType, f => f.FontSize);
        }

        public override void OnCreate(PoolManager poolManager)
        {
            base.OnCreate(poolManager);

            _poolManager = (IndicatorManager)poolManager;
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            
            transform.SetParent(UIManager.WorldCanvas);
        }

        public void Setup(Vector2 position, string message, Color color, EFontSize fontSize = EFontSize.MEDIUM) =>
            Setup(position, message, color, _fontSizeDataDict[fontSize]);
        
        public void Setup(Vector2 position, string message, Color color, float fontSize)
        {
            var randomX = Random.Range(-0.3f, 0.3f);
            var randomY = Random.Range(-0.3f, 0.3f);
            position.x += randomX;
            position.y += randomY;

            transform.position = position;

            damageText.text = message;
            damageText.color = color;
            damageText.fontSize = fontSize;
            
            OnGet(null);

            _timer = 0;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer < lifeTime) return;
            
            _poolManager.ReleasePoolObject(this);
        }

        public enum EFontSize
        {
            SMALL,
            MEDIUM,
            BIG
        }

        [System.Serializable]
        public class FontSizeData
        {
            [SerializeField] private float fontSize;
            [SerializeField] private EFontSize sizeType;

            public float FontSize => fontSize;
            public EFontSize SizeType => sizeType;
        }
    }
}