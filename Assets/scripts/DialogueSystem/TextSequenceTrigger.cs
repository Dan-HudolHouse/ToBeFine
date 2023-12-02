using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TextSequenceTrigger : MonoBehaviour
{
    public TextBlock[] textBlocks;
    public float pauseTime;
    public bool activated = false;

    public TextSequencer sequencer;

    public string collisionTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if(sequencer != null && textBlocks.Length > 0 && other.CompareTag(collisionTag) && activated == false)
        {
            sequencer.PrintSequence(textBlocks, pauseTime);
            activated = true;
        }
    }
}
