using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class tweenExample : MonoBehaviour
{
    public bool isRotating = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isRotating) return;
            RotateTheDude();
        }
    }
    void RotateTheDude()
    {
        isRotating = true;
        Vector3 rotation;
        rotation = transform.rotation.eulerAngles + new Vector3(0, 90, 0);
        transform.DORotate(rotation, 3, RotateMode.Fast).SetEase(Ease.InOutQuad).OnComplete(() => { isRotating = false; });
    }
    void SetFalse()
    {
        isRotating = false;
    }
}
