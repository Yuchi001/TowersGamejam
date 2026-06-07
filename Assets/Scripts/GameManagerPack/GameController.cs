using System;
using System.Collections;
using AudioPack;
using PlayerPack;
using TMPro;
using UnityEngine;
using Utils;
using WindowPack;

namespace GameManagerPack
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private float matchLength;
        [SerializeField] private PlayerMovement player1;
        [SerializeField] private PlayerMovement player2;
        [SerializeField] private float waitBeforePlayerDeath = 2;

        private float _currentTimer;
        private bool _ready = false;

        private bool _spawnedTimer = false;
        private bool _spawnedTimesUp = false;

        public void StartRun()
        {
            _currentTimer = 0;
            _ready = true;
            player1.enabled = true;
            player2.enabled = true;
        }
        
        
        private void Update()
        {
            if (!_ready) return;

            _currentTimer += Time.deltaTime;
            timerText.text = (matchLength - _currentTimer).ToShortTime();
            if (matchLength - _currentTimer <= 1f)
            {
                timerText.text = "Time's Up!";
                if (!_spawnedTimesUp)
                {
                    AudioManager.PlaySound(ESoundType.timesUp);
                    _spawnedTimesUp = true;
                }
            }

            if (!_spawnedTimer && matchLength - _currentTimer <= 9)
            {
                AudioManager.PlaySound(ESoundType.Clock8Sec);
                _spawnedTimer = true;
            }

            if (_currentTimer < matchLength) return;

            _ready = false;

            player1.enabled = false;
            player2.enabled = false;
            
            StartCoroutine(DelegatePlayerDeath());
        }

        private IEnumerator DelegatePlayerDeath()
        {
            yield return new WaitForSeconds(waitBeforePlayerDeath);
            
            var score1 = WindowManager.GetScore(0);
            var score2 = WindowManager.GetScore(1);
            
            if (score1 > score2) player2.Die();
            else player1.Die();
        }
    }
}