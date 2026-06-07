using System.Collections.Generic;
using GameManagerPack;
using UIPack.Elements;
using UIPack.NavigationPack;
using UIPack.NavigationPack.Interface;
using UnityEngine;

namespace UIPack
{
    public class MenuUI : UIBase, INavigationUI
    {
        [SerializeField] private List<TextUIButton> buttons;
        
        public NavigationManager NavigationManager { get; private set; }

        public override void OnOpen(string key)
        {
            var buttonSection = new NavigationSection(this, buttons, NavigationSection.ENavigationOrientation.VERTICAL, NavigationSection.ENavigationOrientation.VERTICAL);
            NavigationManager = new NavigationManager(buttonSection);
            base.OnOpen(key);
        }

        public void OnExit() => Application.Quit();

        public void OnStart()
        {
            UIManager.CloseUI(Key);
            GameManager.StartGame();
        }
    }
}