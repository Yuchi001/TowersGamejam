using System.Collections;
using BulletPack;
using GameManagerPack;
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
        
        [Header("Input Actions")]
        [SerializeField] private InputActionReference moveUpAction;
        [SerializeField] private InputActionReference moveDownAction;
        [SerializeField] private InputActionReference attackAction;
        [SerializeField] private InputActionReference cleanAction;
        

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
        }

        private void OnDown(InputAction.CallbackContext context)
        {
            var newPos = WindowManager.MoveDown(playerID);
            newPos.x += windowOffset;
            transform.position = newPos;
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            BulletEntity.SpawnBullet(bulletSpawnPos.position, playerColor, playerID == 0 ? 1 : -1);
        }

        private void OnClean(InputAction.CallbackContext context)
        {
            WindowManager.CleanWindow(playerID);
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