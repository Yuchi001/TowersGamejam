using UnityEngine;

namespace Utils
{
    public static class SpriteRendererExtensions
    {
        public static Vector2 GetRandomPoint(this SpriteRenderer spriteRenderer)
        {
            var bounds = spriteRenderer.bounds;
            return new Vector2
            {
                x = Random.Range(bounds.min.x, bounds.max.x),
                y = Random.Range(bounds.min.y, bounds.max.y),
            };
        }
        
        public static float GetAboveHeadY(this SpriteRenderer target)
        {
            var bounds = target.bounds;
            return bounds.max.y;
        }
        
        public static float GetBelowFeetY(this SpriteRenderer target)
        {
            var bounds = target.bounds;
            return bounds.min.y;
        }
        
        public static float GetRight(this SpriteRenderer target)
        {
            var bounds = target.bounds;
            return bounds.max.x;
        }
        
        public static float GetLeft(this SpriteRenderer target)
        {
            var bounds = target.bounds;
            return bounds.min.x;
        }
    }
}