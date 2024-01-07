using System.Collections;
using System.Collections.Generic;
using HudolHouse;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            SpawnManager.instance.respawnPosition = SpawnManager.instance.player.transform.position;
        }
    }
}
