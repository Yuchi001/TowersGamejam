using System;
using System.Collections;
using System.Collections.Generic;
using GameManagerPack;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace RobotPack
{
    public class RobotManager : MonoBehaviour, IMainManager
    {
        [SerializeField] private int robotSpawnTryCount;
        [SerializeField] private MinMax robotSpawnPercentage;
        [SerializeField] private Transform robotSpawnPos1;
        [SerializeField] private Transform robotSpawnPos2;

        private int _failCount = 0;
        private int _tryCount = 0;

        private float _currentThreshold = 9999;
        private float _timer = 0;

        private List<Vector3> _spawnPositions;
        
        private void Update()
        {
            if (!GameController.Active) return;
            
            _timer += Time.deltaTime;
            if (_timer < _currentThreshold) return;

            _timer = 0;
            _tryCount++;
            _currentThreshold = Mathf.Lerp(0, GameController.MatchLength, (_tryCount + 1f) / robotSpawnTryCount);

            var random = Random.Range(0f, 1f);
            if (random > robotSpawnPercentage.Lerp(_failCount / (float)robotSpawnTryCount))
            {
                _failCount++;
                return;
            }

            _failCount = 0;
            Robot.SpawnRobot(_spawnPositions.RandomElement());
        }

        public void Init()
        {
            StartCoroutine(DelegateInit());
        }

        private IEnumerator DelegateInit()
        {
            yield return new WaitUntil(() => GameController.HasInstance);
            
            _currentThreshold = Mathf.Lerp(0, GameController.MatchLength, (_tryCount + 1f) / robotSpawnTryCount);
            _spawnPositions = new List<Vector3>
            {
                robotSpawnPos1.position,
                robotSpawnPos2.position,
            };
        }
    }
}