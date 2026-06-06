using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UIPack.Colors
{
    [CreateAssetMenu(fileName = "new TierColors", menuName = "Custom/TierColors")]
    public class SoTierColor : ScriptableObject
    {
        [SerializeField] private List<ListWrapper> tierColors = new();
        [SerializeField] private int maxValue;

        public Color GetColor(int tier, UIColorPair.EElementType elementType) => tierColors[Mathf.Clamp(tier - 1, 0, tierColors.Count - 1)].colors.Single(e => e.Type == elementType).Color;
        
#if UNITY_EDITOR
        public List<ListWrapper> GetColors()
        {
            while (tierColors.Count <= maxValue)
                tierColors.Add(new ListWrapper());

            while (tierColors.Count > maxValue)
                tierColors.RemoveAt(tierColors.Count - 1);
            
            foreach (var wrapper in tierColors)
            {
                var distinctColors = new List<UIColorPair>();
                foreach (var type in (UIColorPair.EElementType[])Enum.GetValues(typeof(UIColorPair.EElementType)))
                {
                    var pair = wrapper.colors.FirstOrDefault(c => c.Type == type);
                    if (pair != null) distinctColors.Add(pair);
                    else distinctColors.Add(new UIColorPair(type));
                }

                wrapper.colors = distinctColors;
            }

            return tierColors;
        }

        public void SetTierColors(List<ListWrapper> colors)
        {
            tierColors = new List<ListWrapper>(colors);
        }
#endif
        
        [System.Serializable]
        public class UIColorPair
        {
            [SerializeField] private Color color;
            [SerializeField] private EElementType type;

            public Color Color => color;
            public EElementType Type => type;

            public UIColorPair(EElementType elementType)
            {
                type = elementType;
                color = Color.white;
            }
            
            public UIColorPair(EElementType elementType, Color elementColor)
            {
                type = elementType;
                color = elementColor;
            }
            
            public enum EElementType
            {
                BACKGROUND,
                TEXT,
                ITEM_BACKGROUND
            }
        }

        [System.Serializable]
        public class ListWrapper
        {
            public List<UIColorPair> colors;

            public ListWrapper() => colors = new List<UIColorPair>();
        }
    }
}