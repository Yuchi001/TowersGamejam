using System;
using System.Collections.Generic;
using System.Linq;
using GameManagerPack;
using UIPack.CloseStrategies;
using UIPack.NavigationPack;
using UIPack.NavigationPack.Interface;
using UIPack.OpenStrategies;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UIPack
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private RectTransform mainCanvas;
        [SerializeField] private RectTransform worldCanvas;
        [SerializeField] private Camera canvasCamera;
        [SerializeField] private Camera mainCamera;

        [Header("Input actions")] 
        [SerializeField] private InputActionReference submitInputAction;
        [SerializeField] private InputActionReference cancelInputAction;
        [SerializeField] private InputActionReference navLeftInputAction;
        [SerializeField] private InputActionReference navRightInputAction;
        [SerializeField] private InputActionReference navUpInputAction;
        [SerializeField] private InputActionReference navDownInputAction;

        private readonly List<UIRecord> UIBaseList = new();

        private UIRecord SelectedUI
        {
            get
            {
                if (_selectedUI?.Script != null && !_selectedUI.Script.Open) _selectedUI = null;
                return _selectedUI ??= UIBaseList.FirstOrDefault(e => e is { Script: INavigationUI } && e.Script.Open);
            }
        }
        private UIRecord _selectedUI;
        public static bool HasSelectedUI => Instance.SelectedUI != null;

        public static RectTransform WorldCanvas => Instance.worldCanvas;
        public static RectTransform MainCanvas => Instance.mainCanvas;
        public static Camera CanvasCamera => Instance.canvasCamera;
        public static Camera MainCamera => Instance.mainCamera;

        private static UIManager Instance { get; set; }

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else Instance = this;

            UIBaseList.Clear();
            
            submitInputAction.action.Enable();
            cancelInputAction.action.Enable();
            navLeftInputAction.action.Enable();
            navRightInputAction.action.Enable();
            navUpInputAction.action.Enable();
            navDownInputAction.action.Enable();

            submitInputAction.ToInputAction().performed += OnSubmitButtonClicked;
            cancelInputAction.ToInputAction().performed += OnCancelButtonClicked;
            navLeftInputAction.ToInputAction().performed += OnNavLeftButtonClicked;
            navRightInputAction.ToInputAction().performed += OnNavRightButtonClicked;
            navUpInputAction.ToInputAction().performed += OnNavUpButtonClicked;
            navDownInputAction.ToInputAction().performed += OnNavDownButtonClicked;
        }

        private void OnDisable()
        {
            submitInputAction.ToInputAction().performed -= OnSubmitButtonClicked;
            cancelInputAction.ToInputAction().performed -= OnCancelButtonClicked;
            navLeftInputAction.ToInputAction().performed -= OnNavLeftButtonClicked;
            navRightInputAction.ToInputAction().performed -= OnNavRightButtonClicked;
            navUpInputAction.ToInputAction().performed -= OnNavUpButtonClicked;
            navDownInputAction.ToInputAction().performed -= OnNavDownButtonClicked;
        }

        public static T InstantiateUI<T>(string uiPrefabName, IOpenStrategy openStrategy = null, ICloseStrategy closeStrategy = null, EUIPlacement uiPlacement = EUIPlacement.CENTER) where T : UIBase
        {
            var prefab = GameManager.GetPrefab<T>(uiPrefabName);
            openStrategy ??= new DefaultOpenStrategy(prefab);
            closeStrategy ??= new DestroyCloseStrategy(uiPrefabName);
            var uiBase = OpenUI<T>(uiPrefabName, openStrategy, closeStrategy, uiPlacement);
            AddUI(uiPrefabName, uiPlacement, closeStrategy, uiBase);
            return uiBase;
        }

        public static T OpenUI<T>(string key, IOpenStrategy openStrategy, ICloseStrategy closeStrategy, EUIPlacement placement) where T: UIBase
        {
            var opened = openStrategy.Open(out var uiBase, key);
            if (!opened) return null;
            
            if(placement == EUIPlacement.WORLD) uiBase.transform.SetParent(Instance.worldCanvas);
            
            uiBase.transform.SetAsLastSibling();

            if (placement == EUIPlacement.CENTER)
            {
                BackdropManager.In(key);
                Instance._selectedUI = new UIRecord(key, EUIPlacement.CENTER, uiBase, closeStrategy);
            }
            
            AddUI(key, placement, closeStrategy, uiBase);
            return uiBase as T;
        }

        /*private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;

            foreach (var uiRecord in UIBaseList.ToList())
            {
                if (uiRecord.Script == null) continue;

                if (uiRecord.Script.OnEscape()) return;
            }
        }*/

        private void OnCancelButtonClicked(InputAction.CallbackContext context)
        {
            if (SelectedUI != null && SelectedUI.Script.OnEscape()) return;

            
            foreach (var uiRecord in UIBaseList)
            {
                if (uiRecord == null || uiRecord.Script == null) continue;

                uiRecord.Script.OnEscape();
            }
        }

        private void OnSubmitButtonClicked(InputAction.CallbackContext context)
        {
            if (SelectedUI is not { Script: INavigationUI navigationUI }) return;

            navigationUI.NavigationManager.HandleSubmit();
        }

        private void OnNavRightButtonClicked(InputAction.CallbackContext context)
        {
            if (SelectedUI is not { Script: INavigationUI navigationUI }) return;
            
            navigationUI.NavigationManager.HandleNavigation(ENavigationDirection.RIGHT);
        }

        private void OnNavLeftButtonClicked(InputAction.CallbackContext context)
        {
            if (SelectedUI is not { Script: INavigationUI navigationUI }) return;

            navigationUI.NavigationManager.HandleNavigation(ENavigationDirection.LEFT);
        }

        private void OnNavUpButtonClicked(InputAction.CallbackContext context)
        {
            if (SelectedUI is not { Script: INavigationUI navigationUI }) return;

            navigationUI.NavigationManager.HandleNavigation(ENavigationDirection.UP);
        }

        private void OnNavDownButtonClicked(InputAction.CallbackContext context)
        {
            if (SelectedUI is not { Script: INavigationUI navigationUI }) return;

            navigationUI.NavigationManager.HandleNavigation(ENavigationDirection.DOWN);
        }

        public static void CloseUI(string key, bool removeFromList = false)
        {
            var record = Instance.UIBaseList.SingleOrDefault(r => r.Key == key);
            if (record == default) return;

            if (HasSelectedUI && Instance.SelectedUI.Key == key) Instance._selectedUI = null;
            
            if (record.Script == null)
            {
                RemoveUI(key);
                return;
            }
            
            if (!record.Script.Open) return;
            
            if (record.Placement == EUIPlacement.CENTER) BackdropManager.Out(key);
            
            record.CloseStrategy.Close(record.Script);
            if (removeFromList) RemoveUI(key);
        }

        public static bool IsOpen(string key)
        {
            return Instance.UIBaseList.Any(e => e.Key == key && e.Script.Open);
        }

        public static void CloseAllUIs()
        {
            foreach (var record in Instance.UIBaseList.Where(record => record.Script.Open))
            {
                record.CloseStrategy.Close(record.Script);
                RemoveUI(record.Key);
            }
        }

        public static UIBase SpawnUI(UIBase uiBase)
        {
            return Instantiate(uiBase, Instance.mainCanvas);
        }

        private static void AddUI(string key, EUIPlacement placement, ICloseStrategy closeStrategy, UIBase spawnedUIBase)
        {
            var alreadyInList = Instance.UIBaseList.FirstOrDefault(r => r.Key == key) != default;
            if (alreadyInList) return;
            Instance.UIBaseList.Insert(0, new UIRecord(key, placement, spawnedUIBase, closeStrategy)); // always add as first element
        }

        public static void RemoveUI(string key)
        {
            Instance.UIBaseList.RemoveAll(u => u.Key == key);
        }

        public static IEnumerable<UIRecord> GetCurrentUIBaseList()
        {
            return Instance.UIBaseList;
        }

        public enum EUIPlacement
        {
            CENTER,
            RIGHT_SIDE,
            LEFT_SIDE,
            BOTTOM_TOOLTIP,
            TOP_TOOLTIP,
            WORLD,
            NONE,
        }

        public record UIRecord
        {
            public readonly EUIPlacement Placement;
            public readonly string Key;
            public readonly UIBase Script;
            public readonly ICloseStrategy CloseStrategy;

            public UIRecord(string key, EUIPlacement placement, UIBase script, ICloseStrategy closeStrategy)
            {
                Key = key;
                CloseStrategy = closeStrategy;
                Script = script;
                Placement = placement;
            }
        }
    }
}