using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public static class GeneratorExtensions
    {
        public static List<Vector2Int> GenerateRandomMap(int count)
        {
            var tiles = new List<Vector2Int>();
            var start = new Vector2Int(0, 0);
            tiles.Add(start);

            var directions = new List<Vector2Int>()
            {
                new(1,0),
                new(-1,0),
                new(0,1),
                new(0,-1)
            };
            
            while (tiles.Count < count)
            {
                var current = tiles.RandomElement();
                var neighbor = current + directions.RandomElement();
                
                var minX = tiles.Min(t => t.x);
                var maxX = tiles.Max(t => t.x);
                var minY = tiles.Min(t => t.y);
                var maxY = tiles.Max(t => t.y);

                /*if (neighbor.x < minX - mapBorders.x / 2 || neighbor.x > maxX + mapBorders.x / 2)
                    continue;
                if (neighbor.y < minY - mapBorders.y / 2 || neighbor.y > maxY + mapBorders.y / 2)
                    continue;*/

                if (!tiles.Contains(neighbor))
                {
                    tiles.Add(neighbor);
                }
            }

            return tiles;
        }
    }
}