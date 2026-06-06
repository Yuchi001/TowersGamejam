using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace GameManagerPack
{
    public class EnsureCorrectLoad : MonoBehaviour
    {
        [SerializeField] private bool unloadCurrent;
        
        private void Awake()
        {
            if (SceneExtensions.IsSceneLoaded((int)GameManager.EScene.MAIN)) return;

            StartCoroutine(LoadMain());
        }

        private IEnumerator LoadMain()
        {
            if (unloadCurrent) yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            SceneManager.LoadScene((int)GameManager.EScene.MAIN);
        }
    }
}