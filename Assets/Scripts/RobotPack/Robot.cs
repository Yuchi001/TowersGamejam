using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameManagerPack;
using RobotPack.DropPack;
using UnityEngine;
using Utils;

namespace RobotPack
{
    public class Robot : MonoBehaviour
    {
        [SerializeField] private List<DropData> drops;
        [SerializeField] private Transform itemSpawnPos;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;
        [SerializeField] private float walkTime;
        [SerializeField] private float walkSpeed;
        [SerializeField] private float itemSpawnTime;

        private float _timer = 0;
        private int _currentDirection;
        private int _backDirection;
        
        private bool _spawnedItem = false;

        private Vector3 _spawnPos;

        public static void SpawnRobot(Vector3 spawnPos)
        {
            var robotPrefab = GameManager.GetPrefab<Robot>(PrefabNames.Robot);
            var spawnedRobot = Instantiate(robotPrefab, spawnPos, Quaternion.identity);

            spawnedRobot._currentDirection = spawnPos.x > 0 ? -1 : 1;
            spawnedRobot._backDirection = spawnPos.x > 0 ? 1 : -1;
            spawnedRobot.spriteRenderer.flipX = spawnPos.x > 0;
            spawnedRobot._spawnPos = spawnPos;
        }
        
        private void Update()
        {
            if (Vector2.Distance(transform.position, _spawnPos) > 20)
            {
                Destroy(gameObject);
                return;
            }
            
            transform.position += Vector3.right * (_currentDirection * Time.deltaTime * walkSpeed);
            
            _timer += Time.deltaTime;
            if (_timer < walkTime) return;

            if (_spawnedItem) return;

            animator.SetTrigger("drop");
            _currentDirection = 0;
            _spawnedItem = true;
            StartCoroutine(DelegateItemSpawn());
        }

        private IEnumerator DelegateItemSpawn()
        {
            yield return new WaitForSeconds(itemSpawnTime);
            
            SpawnItem();
            _currentDirection = _backDirection;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        public void SpawnItem()
        {
            var randomDrop = WeightedRandom.Pick(drops.Select(e => new WeightedRandom.WeightedObject<DropObject>(e.item, e.weight)));
            var spawnPos = itemSpawnPos.position;
            if (transform.position.x > 0) spawnPos.x = transform.position.x - (spawnPos.x - transform.position.x);
            Instantiate(randomDrop, spawnPos, Quaternion.identity);
        }     
    }
}