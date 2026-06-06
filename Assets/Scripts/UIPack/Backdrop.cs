using UnityEngine;
using UnityEngine.UI;

namespace UIPack
{
    public class Backdrop : MonoBehaviour
    {
        [SerializeField] private int enabledAlpha;
        [SerializeField] private Image image;

        public Image Image => image;
        public int EnabledAlpha => enabledAlpha;
    }
}