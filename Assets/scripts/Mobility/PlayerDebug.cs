using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerDebug : MonoBehaviour
{
    public Transform player;
    public TMP_Text text;
    

    // Update is called once per frame
    void Update()
    {
        text.text = player.GetComponent<CharacterController>().velocity.magnitude.ToString();
    }
}
