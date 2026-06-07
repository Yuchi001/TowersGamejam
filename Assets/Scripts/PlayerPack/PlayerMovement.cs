using System;
using System.Collections;
using BulletPack;
using GameManagerPack;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using WindowPack;

namespace PlayerPack
{
    public class PlayerMovement : MonoBehaviour, IMainManager
    {
        [Header("General")] 
        [SerializeField] private float windowOffset;
        [SerializeField] private int playerID;
        [SerializeField] private Color playerColor;
        [SerializeField] private Transform bulletSpawnPos;
        [SerializeField] private int attackStackCount;
        [SerializeField] private float stackRefreshRate;
        [SerializeField] private TextMeshProUGUI attackStackField;
        [SerializeField] private LineRenderer lineRenderer;
        
        [Header("Input Actions")]
        [SerializeField] private InputActionReference moveUpAction;
        [SerializeField] private InputActionReference moveDownAction;
        [SerializeField] private InputActionReference attackAction;
        [SerializeField] private InputActionReference cleanAction;


        private int _currentAttackCount;
        private float _refreshTimer = 0;

        private void Awake()
        {
            moveUpAction.action.Enable();
            moveDownAction.action.Enable();
            attackAction.action.Enable();
            cleanAction.action.Enable();

            moveUpAction.action.started += OnUp;
            moveDownAction.action.started += OnDown;
            attackAction.action.started += OnAttack;
            cleanAction.action.started += OnClean;

            _currentAttackCount = attackStackCount;
            
            attackStackField.text = $"Paint: {_currentAttackCount}/{attackStackCount}";
        }

        private void OnDestroy()
        {
            moveUpAction.action.performed -= OnUp;
            moveDownAction.action.performed -= OnDown;
            attackAction.action.performed -= OnAttack;
            cleanAction.action.performed -= OnClean;
        }

        private void OnUp(InputAction.CallbackContext context)
        {
            var newPos = WindowManager.MoveUp(playerID);
            newPos.x += windowOffset;
            transform.position = newPos;
            
            lineRenderer.SetPosition(1, newPos);
        }

        private void OnDown(InputAction.CallbackContext context)
        {
            var newPos = WindowManager.MoveDown(playerID);
            newPos.x += windowOffset;
            transform.position = newPos;
            
            lineRenderer.SetPosition(1, newPos);
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            if (_currentAttackCount == 0) return;

            _currentAttackCount--;
            attackStackField.text = $"Paint: {_currentAttackCount}/{attackStackCount}";
            BulletEntity.SpawnBullet(bulletSpawnPos.position, playerColor, playerID == 0 ? 1 : -1);
        }

        private void OnClean(InputAction.CallbackContext context)
        {
            WindowManager.CleanWindow(playerID);
        }

        private void Update()
        {
            if (_currentAttackCount == attackStackCount)
            {
                _refreshTimer = 0;
                return;
            }

            _refreshTimer += Time.deltaTime;
            if (_refreshTimer < 1f / stackRefreshRate) return;

            _refreshTimer = 0;
            _currentAttackCount++;

            attackStackField.text = $"Paint: {_currentAttackCount}/{attackStackCount}";
        }

        public void Init()
        {
            StartCoroutine(DelegateInit());
        }

        private IEnumerator DelegateInit()
        {
            yield return new WaitUntil(() => WindowManager.HasInstance);
            OnUp(new InputAction.CallbackContext());
        }
    }
}