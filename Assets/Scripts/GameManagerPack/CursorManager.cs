using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameManagerPack
{
    public class CursorManager : MonoBehaviour
    {
        [SerializeField] private List<CursorState> states = new();
        [SerializeField] private Image cursorImage;
        
        private Dictionary<EState, CursorState> _stateDict;
        private CursorState _currentState;
        private static CursorManager Instance { get; set; }
        
        private void Awake()
        {
            Instance = this;
            _stateDict = states.ToDictionary(e => e.State, e => e);

            Cursor.visible = false;
            
            SetState(EState.DEFAULT);
        }

        private void Update()
        {
            cursorImage.transform.position = Input.mousePosition;
            
            if (!_currentState.CanInterupt) return;
            
            if (Input.GetMouseButtonDown(0)) SetState(EState.GRAB);
            if (Input.GetMouseButtonUp(0)) SetState(EState.DEFAULT);
        }

        public static void SetState(EState state)
        {
            return;
            
            Instance._currentState = Instance._stateDict[state];
            Instance.cursorImage.sprite = Instance._currentState.Sprite;
        }

        public enum EState
        {
            DEFAULT = 0,
            GRAB = 1,
            LOAD = 2,
            CLICK = 3,
        }

        [System.Serializable]
        public class CursorState
        {
            [SerializeField] private EState state;
            [SerializeField] private Sprite sprite;
            [SerializeField] private bool canInterupt;

            public EState State => state;
            public Sprite Sprite => sprite;
            public bool CanInterupt => canInterupt;

            public CursorState(EState state, Sprite sprite, bool canInterupt)
            {
                this.state = state;
                this.sprite = sprite;
                this.canInterupt = canInterupt;
            }
        }
        
        #if UNITY_EDITOR

        public CursorState GetStateObject(EState state) => states.FirstOrDefault(e => e.State == state);

        public void SetStatesList(List<CursorState> newList)
        {
            states = new List<CursorState>(newList);
        }
        
        #endif
    }
}