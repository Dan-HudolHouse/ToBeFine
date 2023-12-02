using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "NewText", menuName = "ScriptableObjects/TextBlock")]
public class TextBlock : ScriptableObject
{
    public string text;
    public int pauseAfterPrinting;
    public AudioClip narration;
    public bool leaveOnScreen = false;
}
