using System.Linq;
using AudioPack;
using BulletPack;
using GameManagerPack;
using UnityEngine;
using WindowPack;

namespace RobotPack.DropPack.Drops
{
    public class Bucket : DropObject
    {
        [SerializeField] private int bulletCount;
        
        public override void OnBulletHit(BulletEntity bullet)
        {
             if (bullet.SpawnPos.x < 0) GameController.Player1.AddBullets(bulletCount);
             else GameController.Player2.AddBullets(bulletCount);
            
            AudioManager.PlaySound(ESoundType.bucket);
            
            Destroy(gameObject);
        }
    }
}