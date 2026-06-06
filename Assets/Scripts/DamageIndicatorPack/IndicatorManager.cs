using System.Text;
using GameManagerPack;
using PoolPack;
using UnityEngine;
using UnityEngine.Pool;
using Utils;

namespace DamageIndicatorPack
{
    public class IndicatorManager : PoolManager, IMainManager
    {
        [SerializeField] private MinMax damageFontSize;
        
        #region Singleton

        private static IndicatorManager Instance { get; set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else Instance = this;
        }

        #endregion

        private ObjectPool<DamageIndicator> _pool;

        public void Init()
        {
            var prefab = GameManager.GetPrefab<DamageIndicator>(PrefabNames.DamageIndicatorHolder);
            _pool = PoolHelper.CreatePool(this, prefab, false);
        }

        public override void ReleasePoolObject(PoolObject poolObject)
        {
            _pool.Release((DamageIndicator)poolObject);
        }

        public override void ClearAll() => ClearAll(_pool);
        
        public static void SpawnIndicator(Vector2 position, int value, int maxValue, bool isCrit, bool isClean)
        {
            var sb = new StringBuilder();
            if (isCrit) sb.Append(isClean ? "<sprite name=\"critk\">" : "<sprite name=\"crit\">");
            foreach (var v in value.ToString())
            {
                sb.Append("<sprite name=\"");

                if (isCrit) sb.Append("c");
                if (isClean) sb.Append("k");

                sb.Append(v);
                
                sb.Append("\">");
            }
            
            var fontSize = Instance.damageFontSize.Lerp(value / (float)maxValue);
            Instance._pool.Get().Setup(position, sb.ToString(), Color.white, fontSize);
        }
        
        public static void SpawnIndicator(Vector2 position, EIndicatorText indicatorText)
        {
            var sb = new StringBuilder("<sprite name=\"");
            sb.Append(indicatorText.ToString().ToLower());
            sb.Append("\">");
            Instance._pool.Get().Setup(position, sb.ToString(), Color.white, DamageIndicator.EFontSize.SMALL);
        }

        public static void SpawnIndicator(Vector2 position, string text, Color color, DamageIndicator.EFontSize fontSize)
        {
            Instance._pool.Get().Setup(position, text, color, fontSize);
        }
    }
}