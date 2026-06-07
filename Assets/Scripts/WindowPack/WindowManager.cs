using System;
using System.Collections.Generic;
using System.Linq;
using GameManagerPack;
using TMPro;
using UnityEngine;
using Utils;

namespace WindowPack
{
    public class WindowManager : MonoBehaviour, IMainManager
    {
        [SerializeField] private int windowCount;
        [SerializeField] private float windowSize;
        [SerializeField] private Transform playerOneWindowsSpawnPoint;
        [SerializeField] private Transform playerTwoWindowsSpawnPoint;
        [SerializeField] private TextMeshProUGUI scoreField1;
        [SerializeField] private TextMeshProUGUI scoreField2;
        
        private static WindowManager Instance { get; set; }
        public static bool HasInstance => Instance != null;

        private readonly Dictionary<int, int> _playerPos = new();
        private readonly Dictionary<int, List<WindowEntity>> _windows = new();

        public void Init()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else Instance = this;
            
            _playerPos.Add(0, 0); // Add player one in 0 pos
            _playerPos.Add(1, 0); // Add player two in 0 pos

            var windowPrefab = GameManager.GetPrefab<WindowEntity>(PrefabNames.WindowEntity);
            
            _windows.Add(0, new List<WindowEntity>()); // Add player one entry
            _windows.Add(1, new List<WindowEntity>()); // Add player two entry
            
            windowCount.Repeat(index =>
            {
                var spawnPointBase1 = playerOneWindowsSpawnPoint.position;
                var pos1 = new Vector3(spawnPointBase1.x, spawnPointBase1.y - windowSize * index);
                var window1 = Instantiate(windowPrefab, pos1, Quaternion.identity);
                window1.Setup(0);
                _windows[0].Add(window1);
                
                var spawnPointBase2 = playerTwoWindowsSpawnPoint.position;
                var pos2 = new Vector3(spawnPointBase2.x, spawnPointBase2.y - windowSize * index);
                var window2 = Instantiate(windowPrefab, pos2, Quaternion.identity);
                window2.Setup(1);
                _windows[1].Add(window2);
            });
        }

        public static int GetScore(int playerID) => Instance._windows[playerID].Sum(e => e.Points);
        public static List<WindowEntity> GetWindows(int playerID) => Instance._windows[playerID];

        private void Update()
        {
            scoreField1.text = "SCORE\n" + GetScore(0).ToShortInt();
            scoreField2.text = "SCORE\n" + GetScore(1).ToShortInt();
        }

        public static void CleanWindow(int playerId, bool full)
        {
            var pos = Instance._playerPos[playerId];
            var window = Instance._windows[playerId][pos];
            window.Clean(full);
        }

        public static Vector3 MoveUp(int playerId)
        {
            var playerPos = Instance._playerPos[playerId];
            if (playerPos > 0) Instance._playerPos[playerId]--;
            playerPos = Instance._playerPos[playerId];
            return Instance._windows[playerId][playerPos].transform.position;
        }

        public static Vector3 MoveDown(int playerId)
        {
            var playerPos = Instance._playerPos[playerId];
            if (playerPos + 1 < Instance._windows[playerId].Count) Instance._playerPos[playerId]++;
            playerPos = Instance._playerPos[playerId];
            return Instance._windows[playerId][playerPos].transform.position;
        }
    }
}