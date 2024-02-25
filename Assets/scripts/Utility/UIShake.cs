using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class UIShake : MonoBehaviour
{
    public RectTransform rect;
    

    public float shakeDuration, strength, randomness;
    public int vibrato;
    public bool snapping, fadeOut;

    // Start is called before the first frame update
    void Start()
    {
        FloatingText();
    }

    void FloatingText()
    {
        rect.DOShakeAnchorPos(shakeDuration, strength, vibrato, randomness, snapping, fadeOut).OnComplete(FloatingText);
    }
    private void OnDisable()
    {
        rect.DOKill();
    }
}
