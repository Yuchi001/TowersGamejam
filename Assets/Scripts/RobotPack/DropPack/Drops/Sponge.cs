using System.Linq;
using AudioPack;
using BulletPack;
using UnityEngine;
using WindowPack;

namespace RobotPack.DropPack.Drops
{
    public class Sponge : DropObject
    {
        [SerializeField] private int uses;
        
        public override void OnBulletHit(BulletEntity bullet)
        {
            var playerID = bullet.SpawnPos.x < 0 ? 0 : 1;
            var windows = WindowManager.GetWindows(playerID).ToList();
            windows.Sort((a, b) => a.Points - b.Points);
            foreach (var window in windows.Take(uses)) window.Clean();
            
            AudioManager.PlaySound(ESoundType.sponge);
            
            Destroy(gameObject);
        }
    }
}