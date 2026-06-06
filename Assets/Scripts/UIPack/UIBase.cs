using System;
using System.Collections;
using UnityEngine;

namespace UIPack
{
    public abstract class UIBase : MonoBehaviour
    {
        [SerializeField] protected float animTime = 0.3f;
        [SerializeField] protected bool useAnim = false;
        public bool Open => gameObject != null && gameObject.activeSelf;
        protected string Key { get; private set; }

        public virtual bool OnEscape()
        {
            UIManager.CloseUI(Key);
            return true;
        }
        
        public virtual void OnOpen(string key)
        {
            gameObject.SetActive(true);
            if (useAnim)
            {
                transform.localScale = Vector3.zero;
                transform.LeanScale(Vector3.one, animTime).setEaseInBack().setEaseOutBack().setIgnoreTimeScale(true);
            }
            Key = key;
        }

        public virtual void OnClose()
        {
            if (useAnim)
            {
                transform.LeanScale(Vector3.zero, animTime).setEaseInBack().setIgnoreTimeScale(true);
                StartCoroutine(Deactivate());
            } else OnDeactivate();
        }

        protected virtual void OnDeactivate()
        {
            gameObject.SetActive(false);
        }

        private IEnumerator Deactivate()
        {
            yield return new WaitForSecondsRealtime(animTime);
            
            OnDeactivate();
        }
    }
}