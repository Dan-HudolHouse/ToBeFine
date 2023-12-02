using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostArea : MonoBehaviour
{
    public PlayerMovement.SpeedMods mods = new PlayerMovement.SpeedMods();

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (!PlayerMovement.modifiers.Contains(mods))
            {
                PlayerMovement.modifiers.Add(mods);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (PlayerMovement.modifiers.Contains(mods))
            {
                PlayerMovement.modifiers.Remove(mods);
            }
        }
    }


}
