using System;
using GameManagerPack;
using UnityEngine;
using WindowPack;

namespace BulletPack
{
    public class BulletEntity : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer bulletSprite;
        [SerializeField] private float bulletSpeed;

        private Vector3 _moveVector;
        
        public static void SpawnBullet(Vector3 position, Color bulletColor, int direction)
        {
            var bulletPrefab = GameManager.GetPrefab<BulletEntity>(PrefabNames.BulletEntity);
            var spawnedBullet = Instantiate(bulletPrefab, position, Quaternion.identity);
            spawnedBullet._moveVector = new Vector3(direction * spawnedBullet.bulletSpeed, 0);
        }

        private void Update()
        {
            transform.position += _moveVector * Time.deltaTime;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("HIT");
            if (other.TryGetComponent(out WindowEntity window))
            {
                window.DirtyUp();
                Destroy(gameObject);
            }
        }
    }
}