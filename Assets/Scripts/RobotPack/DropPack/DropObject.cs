using System;
using BulletPack;
using UnityEngine;

namespace RobotPack.DropPack
{
    public abstract class DropObject : MonoBehaviour
    {
        [SerializeField] private float movementSpeed;
        
        public abstract void OnBulletHit(BulletEntity bullet);

        private Vector3 _spawnPos;

        private void Awake()
        {
            _spawnPos = transform.position;
        }

        protected virtual void Update()
        {
            transform.position += Vector3.down * (movementSpeed * Time.deltaTime);

            if (_spawnPos.y > -10) return;
            
            Destroy(gameObject);
        }
    }
}