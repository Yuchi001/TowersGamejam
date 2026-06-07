using AudioPack;
using BulletPack;
using UnityEngine;

namespace RobotPack.DropPack.Drops
{
    public class Portal : DropObject
    {
        [SerializeField] private float offsetY;
        [SerializeField] private float offsetX;
        
        public override void OnBulletHit(BulletEntity bullet)
        {
            var color = bullet.GetComponentInChildren<SpriteRenderer>().color;
            var position = bullet.transform.position;
            var dir = bullet.SpawnPos.x < 0 ? 1 : -1;
            var pos1 = position + new Vector3(dir * offsetX, -offsetY);
            var pos2 = position + new Vector3(dir * offsetX, offsetY);
            var pos3 = position + new Vector3(dir * offsetX, 0);
            
            BulletEntity.SpawnBullet(pos1, color, dir);
            BulletEntity.SpawnBullet(pos2, color, dir);
            BulletEntity.SpawnBullet(pos3, color, dir);
            
            AudioManager.PlaySound(ESoundType.Portal);
            
            Destroy(bullet.gameObject);
        }
    }
}