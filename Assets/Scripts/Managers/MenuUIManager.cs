using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Managers
{
    public class MenuUIManager : MonoBehaviour
    {
        [Header("Fade")]
        [SerializeField]
        private Image _vinguette;
        [SerializeField, Min(0)]
        private float _fadeTime = 3f;

        private void Start()
        {
            if (_vinguette == null) throw new NullReferenceException("Vinguette not found.");
            _vinguette.gameObject.SetActive(true);
            _vinguette.color = Color.black;

            Fade(FadeMode.Out, _fadeTime);
        }

        public enum FadeMode : byte { In, Out }
        public void Fade(FadeMode fadeMode, float time)
        {
            StartCoroutine(FadeCoroutine(fadeMode, time));
        }
        private IEnumerator FadeCoroutine(FadeMode fadeMode, float time)
        {
            if (time == 0)
            {
                _vinguette.color = new Color(_vinguette.color.r, _vinguette.color.g, _vinguette.color.b, fadeMode == FadeMode.In ? 0 : 1);
                yield break;
            }
            if (time < 0) throw new ArgumentOutOfRangeException("Fade time can't be lower than zero.");

            Color startColor = _vinguette.color;
            Color endColor = startColor;
            endColor.a = fadeMode == FadeMode.In ? 1 : 0;

            float factor = 0;
            while (factor < 1)
            {
                _vinguette.color = Color.Lerp(startColor, endColor, factor += Time.deltaTime / time);
                yield return null;
            }
        }
    }
}