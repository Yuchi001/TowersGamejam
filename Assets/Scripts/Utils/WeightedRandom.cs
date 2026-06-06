using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class WeightedRandom
    {
        public static T Pick<T>(IEnumerable<WeightedObject<T>> items)
        {
            var list = items.Where(x => x.Weight > 0).ToList();

            if (list.Count == 0) throw new InvalidOperationException("Cannot pick random from empty list");

            var totalWeight = list.Sum(x => x.Weight);
            var roll = UnityEngine.Random.Range(0f, totalWeight);

            foreach (var pair in list)
            {
                roll -= pair.Weight;
                if (roll <= 0)
                    return pair.Item;
            }

            return list[^1].Item;
        }

        public class WeightedObject<T>
        {
            public readonly T Item;
            public readonly float Weight;

            public WeightedObject(T item, float weight)
            {
                Item = item;
                Weight = weight;
            }
        }
    }
}