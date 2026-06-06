using TMPro;
using UIPack.NavigationPack.Interface;
using UnityEngine;

namespace UIPack.Elements
{
    public class TextUIButton : UIButton, INavigationElement
    {
        private TextMeshProUGUI _textField;

        private string _text;

        protected override void Awake()
        {
            base.Awake();
            _textField = animationObject.GetComponentInChildren<TextMeshProUGUI>();
            _text = _textField.text.Replace(">", "");
        }

        protected override void PointerEnter()
        {
            SetText(_textField.text);
            _textField.text = _text.Insert(0, "> ");
        }

        protected override void PointerExit()
        {
            _textField.text = _text;
        }

        public override void OnClick()
        {
            _textField.text = _text;
            base.OnClick();
        }

        private void Update()
        {
            if (Active) return;
            animationObject.transform.localScale = Vector3.one; 
        }

        private void OnDisable()
        {
            animationObject.transform.localScale = Vector2.one;
        }

        public override void OnFocus()
        {
            
        }

        public void SetText(string text)
        {
            _textField.text = _text = text;
        }
        public string GetText() => _text;

        public virtual void OnSelect(INavigationUI parentUI) => OnPointerEnter(null);
        public virtual void OnDeselect(INavigationUI parentUI) => OnPointerExit(null);

        public virtual void OnSubmit(INavigationUI parentUI) => OnPointerClick(null);
    }
}