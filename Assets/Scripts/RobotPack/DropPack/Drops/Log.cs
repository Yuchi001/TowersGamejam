using AudioPack;
using BulletPack;

namespace RobotPack.DropPack.Drops
{
    public class Log : DropObject
    {
        public override void OnBulletHit(BulletEntity bullet)
        {
            Destroy(bullet.gameObject);
            AudioManager.PlaySound(ESoundType.Wood);
        }
    }
}