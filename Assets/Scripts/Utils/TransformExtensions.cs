using UIPack;
using UnityEngine;
namespace Utils
{
    public static class TransformExtensions
    {
        public static void AdjustForPivot(this Transform transform, SpriteRenderer renderer)
        {
            var position = transform.position;
            var centerOffset = renderer.bounds.center - position;
            position -= centerOffset;
            transform.position = position;
        }

        public static void LookAt(this Transform objTransform, Vector2 lookAtPos)
        {
            var angleDeg = objTransform.GetAngleToObject(lookAtPos);
            objTransform.rotation = Quaternion.Euler(0,0,angleDeg);
        }
        
        public static float GetAngleToObject(this Transform objTransform, Vector2 lookAtPos, bool isRad = false)
        {
            var playerPos = objTransform.position;
            var x = lookAtPos.x - playerPos.x;
            var y = lookAtPos.y - playerPos.y;
            var angleRad = Mathf.Atan2(y, x);
            return isRad ? angleRad - Mathf.Deg2Rad * 90 : (180 / Mathf.PI) * angleRad - 90;
        }
        
        public static void SetUIElementPositionToMousePosition(this Transform transform, RectTransform parent)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, Input.mousePosition, UIManager.CanvasCamera, out var pos);
            transform.localPosition = pos;
        }

        public static bool InRange(this Transform current, Vector2 searched, float range)
        {
            return current.SqrMagnitude(searched) < range * range;
        }
        
        public static float SqrMagnitude(this Transform current, Vector2 searched)
        {
            return (searched - (Vector2)current.position).sqrMagnitude;
        }
        
        public static Vector2 RandomPointInRange(this Transform transform, float range)
        {
            Vector2 center = transform.position;
            var offset = Random.insideUnitCircle * range;
            return center + offset;
        }

        public static void MoveTowards(this Transform current, Vector2 target, float speed)
        {
            current.position = Vector2.MoveTowards(current.position, target, speed);
        }

        public static void MoveInDirection(this Transform current, Vector2 direction, float speed)
        {
            current.position += (Vector3)(direction * speed);
        }
    }
}