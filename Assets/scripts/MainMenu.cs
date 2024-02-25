using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class MainMenu : MonoBehaviour
{
    public CinemachineVirtualCamera splashScreenCamera, menuCamera;
    public GameObject ilight;
    private bool canInteract, isActivated;

    public UIFadeIn logoFade, menuFade;
    // Start is called before the first frame update
    void Start()
    {
        menuCamera.Priority = 9;
        splashScreenCamera.Priority = 10;
        Invoke("MakeInteractable", 5);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && canInteract && !isActivated)
        {
            StartCoroutine(TransitionToMenu());

        }
    }
    void MakeInteractable()
    {
        canInteract = true;
    }
    IEnumerator TransitionToMenu()
    {
        ilight.SetActive(false);
        isActivated = false;

        logoFade.fadeMode = UIFadeIn.mode.FadeOut;
        logoFade.duration = 1;
        logoFade.BeginFade();
        //yield return new WaitForSeconds(5);

        menuCamera.Priority = 10;
        splashScreenCamera.Priority = 9;

        menuFade.BeginFade();
        yield return null;
    }
}
