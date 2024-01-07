using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace HudolHouse
{
    public class ScreenFader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Image canvasImage;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasImage = GetComponent<Image>();
        }
       
        public async Task FadeIn(float duration, Color color)
        {
            canvasGroup.alpha = 0;
            canvasImage.color = color;

            float pos = 0;

            while (pos < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(0, 1, pos / duration);
                pos += Time.deltaTime;
                await Task.Delay(1);
            }
            return;
        }
        public async Task FadeOut(float duration, Color color)
        {
            canvasGroup.alpha = 1;
            canvasImage.color = color;

            float pos = 0;

            while (pos < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(1, 0, pos / duration);
                pos += Time.deltaTime;
                await Task.Delay(1);
            }
            return;
        }

    }
}

