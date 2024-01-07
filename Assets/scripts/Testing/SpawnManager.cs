using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HudolHouse;
using UnityEngine;
using UnityEngine.Events;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    
    public  Vector3 respawnPosition;
    public  GameObject player;

    bool respawning = false;

    public ScreenFader screenFader;

    public  UnityEvent OnBeginRespawn, OnFinishRespawn;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        respawnPosition = player.transform.position;
        respawning = false;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
    }
    public async void Respawn()
    {
        if (respawning) return;
        respawning = true;
        //allow other subscribed scripts to play respawning effects.
        OnBeginRespawn.Invoke();
        //Freeze player controls and default all the values of the player
        

        //fade the screen in, move the player, then fade the screen out
        await screenFader.FadeIn(1, Color.white);
        TogglePlayerComponents(false);
        player.transform.position = respawnPosition;
        await Task.Delay(1);
        TogglePlayerComponents(true);
        //ALSO RESET THE SAVED INFORMATION AND CUE ANY DIALOGUE NEEDED
        await screenFader.FadeOut(0.5f, Color.white);
        OnFinishRespawn.Invoke();

        respawning = false;
        return;

    }
    
    void TogglePlayerComponents(bool IsEnabled)
    {
        player.GetComponent<CharacterController>().enabled = IsEnabled;
        PlayerMovement pv = player.GetComponent<PlayerMovement>();

       
            pv.jumpSettings.jumping = false;
            pv.jumpSettings.jumpSpeed = 0f;
            //pv.playerVelocity.y = 0;
            pv.currentSpeed = 0;
        pv.enabled = IsEnabled;
        player.GetComponentInChildren<TrailRenderer>().enabled = IsEnabled;
        

    }
}
