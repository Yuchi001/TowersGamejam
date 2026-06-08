using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace CurtainsPack
{
    public class CurtainsManager : MonoBehaviour
    {
        [SerializeField] private Image leftCurtain;
        [SerializeField] private Image rightCurtain;
        [SerializeField] private MinMax borderPositions;
        [SerializeField] private float animTime;
        
        private static CurtainsManager Instance { get; set; }

        private Vector3 _startPosLeft;
        private Vector3 _startPosRight;

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else Instance = this;

            _startPosLeft = leftCurtain.transform.position;
            _startPosRight = rightCurtain.transform.position;
        }

        public static void In()
        {
            LeanTween.value(Instance.borderPositions.Min, Instance._startPosLeft.x, Instance.animTime)
                .setOnUpdate((float e) =>
                {
                    Instance.leftCurtain.transform.position = new Vector3(e, Instance.leftCurtain.transform.position.y);
                });
            
            LeanTween.value(Instance.borderPositions.Max, Instance._startPosRight.x, Instance.animTime)
                .setOnUpdate((float e) =>
                {
                    Instance.rightCurtain.transform.position = new Vector3(e, Instance.rightCurtain.transform.position.y);
                });
        }

        public static void Out()
        {
            LeanTween.value(Instance._startPosLeft.x, Instance.borderPositions.Min, Instance.animTime)
                .setOnUpdate((float e) =>
                {
                    Instance.leftCurtain.transform.position = new Vector3(e, Instance.leftCurtain.transform.position.y);
                });
            
            LeanTween.value(Instance._startPosRight.x, Instance.borderPositions.Max, Instance.animTime)
                .setOnUpdate((float e) =>
                {
                    Instance.rightCurtain.transform.position = new Vector3(e, Instance.rightCurtain.transform.position.y);
                });
        }
    }
}