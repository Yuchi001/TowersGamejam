using System.Linq;
using AudioPack;
using BulletPack;
using GameManagerPack;
using UnityEngine;
using WindowPack;

namespace RobotPack.DropPack.Drops
{
    public class GoldenSponge : DropObject
    {
        public override void OnBulletHit(BulletEntity bullet)
        {
            if (bullet.SpawnPos.x < 0) GameController.Player1.SetGoldenSpongeTimer();
            else GameController.Player2.SetGoldenSpongeTimer();
            
            AudioManager.PlaySound(ESoundType.goldensponge);
            
            Destroy(gameObject);
        }
    }
}