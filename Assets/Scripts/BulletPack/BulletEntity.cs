using System;
using AudioPack;
using GameManagerPack;
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
        
        public static void SpawnBullet(Vector3 position, Color bulletColor, int direction)
        {
            var bulletPrefab = GameManager.GetPrefab<BulletEntity>(PrefabNames.BulletEntity);
            var spawnedBullet = Instantiate(bulletPrefab, position, Quaternion.identity);
            spawnedBullet._moveVector = new Vector3(direction * spawnedBullet.bulletSpeed, 0);
            spawnedBullet.bulletSprite.color = bulletColor;
            var main = spawnedBullet.particleSystem.main;
            main.startColor = bulletColor * Color.gray;
        }

        private void Update()
        {
            transform.position += _moveVector * Time.deltaTime;
            _moveVector -= _moveVector / 2f * Time.deltaTime;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out WindowEntity window))
            {
                window.DirtyUp();
                AudioManager.PlaySound(ESoundType.cartoonsplash);
                Destroy(gameObject);
            }
        }
    }
}