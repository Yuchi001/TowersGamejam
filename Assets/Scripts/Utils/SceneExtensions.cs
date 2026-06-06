using GameManagerPack;
using PoolPack;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public static class SceneExtensions
    {
        public static bool IsSceneLoaded(string sceneName)
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name == sceneName) return true;
            }
            return false;
        }

        public static void ClearSceneObjectPools(GameManager.EScene sceneType)
        {
            var sceneObj = SceneManager.GetSceneByBuildIndex((int)sceneType);
            foreach (var root in sceneObj.GetRootGameObjects())
            {
                if (root.TryGetComponent(out PoolManager poolManager)) poolManager.ClearAll();
            }
        }

        public static void MoveObjectsToScene(GameManager.EScene scene, params GameObject[] objects)
        {
            var sceneObj = SceneManager.GetSceneByBuildIndex((int)scene);
            foreach (var obj in objects) SceneManager.MoveGameObjectToScene(obj, sceneObj);
        } 
        
        public static bool IsSceneLoaded(int id)
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.buildIndex == id) return true;
            }
            return false;
        }
    }
}