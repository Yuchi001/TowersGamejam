using System;
using GameManagerPack;
using UIPack;
using UnityEngine;

namespace ExplosionPack
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField] private float waitBeforeEnd;

        private float _timer = 0f;

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer < waitBeforeEnd) return;

            UIManager.InstantiateUI<EndMenuUI>(PrefabNames.EndMenuUI);
            Destroy(gameObject);
        }
    }
}