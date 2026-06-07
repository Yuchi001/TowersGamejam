using System.Collections;
using System.Collections.Generic;
using AudioPack;
using GameManagerPack;
using UIPack.Elements;
using UIPack.NavigationPack;
using UIPack.NavigationPack.Interface;
using UnityEngine;

namespace UIPack
{
    public class MenuUI : StaticUIBase, INavigationUI
    {
        [SerializeField] private List<TextUIButton> buttons;
        
        public NavigationManager NavigationManager { get; private set; }

        public override void OnOpen(string key)
        {
            var buttonSection = new NavigationSection(this, buttons, NavigationSection.ENavigationOrientation.VERTICAL, NavigationSection.ENavigationOrientation.VERTICAL);
            NavigationManager = new NavigationManager(buttonSection);
            base.OnOpen(key);

            StartCoroutine(DelegateMenuSound());
        }

        private IEnumerator DelegateMenuSound()
        {
            yield return new WaitForSeconds(0.5f);
            AudioManager.PlaySound(ESoundType.cleanOrDie);
        }

        public void OnExit() => Application.Quit();

        public void OnStart()
        {
            UIManager.CloseUI(Key);
            GameManager.StartGame();
        }
    }
}