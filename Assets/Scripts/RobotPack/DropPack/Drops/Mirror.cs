using AudioPack;
using BulletPack;
using UnityEngine;

namespace RobotPack.DropPack.Drops
{
    public class Mirror : DropObject
    {
        [SerializeField] private float rotationSpeed;
        
        public override void OnBulletHit(BulletEntity bullet)
        {
            var spawnPosition = bullet.SpawnPos;
            var currentPosition = bullet.transform.position;
            var newX = spawnPosition.x < 0 ? currentPosition.x - 0.25f : currentPosition.x + 0.25f;
            currentPosition.x = newX;
            var newDir = spawnPosition.x < 0 ? -1 : 1;
            var color = bullet.GetComponentInChildren<SpriteRenderer>().color;
            bullet.DestroyBullet();
            BulletEntity.SpawnBullet(currentPosition, color, newDir);
            
            AudioManager.PlaySound(ESoundType.Mirror);
        }

        protected override void Update()
        {
            transform.GetChild(0).Rotate(0, 0, rotationSpeed * Time.deltaTime);
            base.Update();
        }
    }
}