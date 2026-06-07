using System.Collections.Generic;
using TMPro;
using UIPack.Elements;
using UIPack.NavigationPack;
using UIPack.NavigationPack.Interface;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using WindowPack;

namespace UIPack
{
    public class EndMenuUI : UIBase, INavigationUI
    {
        [SerializeField] private TextMeshProUGUI winField;
        [SerializeField] private TextMeshProUGUI scoreField;
        [SerializeField] private List<TextUIButton> buttons;
        
        public NavigationManager NavigationManager { get; private set; }
        
        public override void OnOpen(string key)
        {
            var buttonSection = new NavigationSection(this, buttons, NavigationSection.ENavigationOrientation.VERTICAL, NavigationSection.ENavigationOrientation.VERTICAL);
            NavigationManager = new NavigationManager(buttonSection);

            var score1 = WindowManager.GetScore(0);
            var score2 = WindowManager.GetScore(1);
            var maxScore = Mathf.Max(score1, score2).ToShortInt();
            winField.text = $"Player {(score1 > score2 ? "one" : "two")} wins!";
            scoreField.text = $"SCORE:\n{maxScore}";
            
            base.OnOpen(key);
        }

        public void OnRestart() => SceneManager.LoadScene(0);
    }
}