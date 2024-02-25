using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeIn : MonoBehaviour
{
    [SerializeField] bool onActive, pingPong;
    public float startDelay, duration, minFade, maxFade, recurringDelay;

    CanvasGroup cv;
    public enum mode { FadeIn, FadeOut}
    public mode fadeMode;
    // Start is called before the first frame update
    void Start()
    {
        cv = GetComponent<CanvasGroup>();
        if (onActive) BeginFade();
    }

    public void BeginFade()
    {
        switch (fadeMode)
        {
            case mode.FadeIn:
                StartCoroutine(Fader(minFade, maxFade, startDelay));
                break;
            case mode.FadeOut:
                StartCoroutine(Fader(maxFade, minFade, startDelay));
                break;
        }
        
    }

    IEnumerator Fader(float start, float end, float delay)
    {
        
        yield return new WaitForSeconds(delay);
        cv.alpha = start;
        float t = 0;
        

        while(t < duration)
        {
            cv.alpha = Mathf.Lerp(start, end, t / duration);
            yield return null;
            t += Time.deltaTime;
        }
        cv.alpha = end;
        if (pingPong)
        {
            float newStart = end, newEnd = start;
            StartCoroutine(Fader(newStart, newEnd, recurringDelay));
        }
        yield return null;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
