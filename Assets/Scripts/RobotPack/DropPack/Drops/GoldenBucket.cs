using System.Linq;
using AudioPack;
using BulletPack;
using GameManagerPack;
using UnityEngine;
using WindowPack;

namespace RobotPack.DropPack.Drops
{
    public class GoldenBucket : DropObject
    {
        public override void OnBulletHit(BulletEntity bullet)
        {
            if (bullet.SpawnPos.x < 0) GameController.Player1.SetGoldenBucketTimer();
            else GameController.Player2.SetGoldenBucketTimer();
            
            AudioManager.PlaySound(ESoundType.goldenBucket);
            
            Destroy(gameObject);
        }
    }
}