using System;
using AudioPack;
using GameManagerPack;
using RobotPack.DropPack;
using RobotPack.DropPack.Drops;
using UnityEngine;
using WindowPack;

namespace BulletPack
{
    public class BulletEntity : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer bulletSprite;
        [SerializeField] private ParticleSystem particleSystem;
        [SerializeField] private float bulletSpeed;

        private Vector3 _moveVector;
        public Vector3 SpawnPos { get; private set; }
        
        public static void SpawnBullet(Vector3 position, Color bulletColor, int direction)
        {
            var bulletPrefab = GameManager.GetPrefab<BulletEntity>(PrefabNames.BulletEntity);
            var spawnedBullet = Instantiate(bulletPrefab, position, Quaternion.identity);
            spawnedBullet._moveVector = new Vector3(direction * spawnedBullet.bulletSpeed, 0);
            spawnedBullet.bulletSprite.color = bulletColor;
            var main = spawnedBullet.particleSystem.main;
            main.startColor = bulletColor * Color.gray;
            spawnedBullet.SpawnPos = position;
        }

        private void Update()
        {
            transform.position += _moveVector * Time.deltaTime;
            _moveVector -= _moveVector / 2f * Time.deltaTime;

            if (Vector3.Distance(transform.position, SpawnPos) < 20) return;
            
            Destroy(gameObject);
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out WindowEntity window))
            {
                window.DirtyUp();
                DestroyBullet();
                return;
            }

            if (other.TryGetComponent(out DropObject dropObject)) dropObject.OnBulletHit(this);
        }

        public void DestroyBullet()
        {
            AudioManager.PlaySound(ESoundType.cartoonsplash);
            Destroy(gameObject);
        }
    }
}