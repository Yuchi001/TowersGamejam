using System.Collections.Generic;
using System.Linq;
using AudioPack;
using GameManagerPack;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIPack.Elements
{
    public class UIButton : NavigationElement, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private UnityEvent onClick;
        [SerializeField] private Color enabledColor = Color.white;
        [SerializeField] private Color disabledColor = Color.grey;
        [SerializeField] protected Graphic animationObject;
        
        public bool Active { get; private set; } = true;

        private List<Graphic> _graphicElements;

        protected virtual void Awake()
        {
            _graphicElements = animationObject.GetComponentsInChildren<Graphic>().ToList();
        }

        public virtual void EnableButton(bool active)
        {
            Active = active;
            var color = active ? enabledColor : disabledColor;
            _graphicElements.ForEach(i => i.color = color);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!Active) return;
            CursorManager.SetState(CursorManager.EState.CLICK);
            AudioManager.PlaySound(ESoundType.ButtonHover);
            
            PointerEnter();
        }

        protected virtual void PointerEnter()
        {
            LeanTween.cancel(gameObject);
            animationObject.transform.LeanScale(Vector2.one * 1.2f, 0.1f).setEaseInBack().setEaseInOutBack().setIgnoreTimeScale(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!Active) return;
            CursorManager.SetState(CursorManager.EState.DEFAULT);
            
            PointerExit();
        }

        protected virtual void PointerExit()
        {
            LeanTween.cancel(gameObject);
            animationObject.transform.LeanScale(Vector2.one, 0.1f).setEaseInBack().setEaseInOutBack().setIgnoreTimeScale(true);
        }

        private void Update()
        {
            if (Active) return;
            animationObject.transform.localScale = Vector3.one; 
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick();
        }

        private void OnDisable()
        {
            animationObject.transform.localScale = Vector2.one;
        }

        public override void OnFocus()
        {
            
        }

        public override void OnClick()
        {
            if (!Active) return;
            
            onClick?.Invoke();
            AudioManager.PlaySound(ESoundType.ButtonClick);
        }
    }
}