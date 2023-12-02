using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TextSequencer : MonoBehaviour
{
    public TMP_Text displayText;

    public void PrintSequence(TextBlock[] blocks, float pauseTime)
    {
        StopAllCoroutines();
        StartCoroutine(SequenceCoroutine(blocks, pauseTime));
    }

    /// <summary>
    /// Print a sequence of messages one word at a time, pausing for a defined amount between each word
    /// then pausing by a time defined by the blocks between each completed block.
    ///
    /// </summary>
    /// <param name="blocks"> The collection of TextBlocks. Each text block holds a text and an amount of time to pause before the next</param>
    /// <param name="pauseTime"> The amount of time to wait between words in a block</param>
    /// <returns></returns>
    public IEnumerator SequenceCoroutine(TextBlock[] blocks, float pauseTime)
    {
        displayText.text = "";

        WaitForSeconds pause = new WaitForSeconds(pauseTime);

        foreach(TextBlock block in blocks)
        {
            string[] words = block.text.Split(char.Parse(" "));
            for (int i = 0; i < words.Length; i++)
            {
                displayText.text += words[i] + " ";
                yield return pause; 
            }

            yield return new WaitForSeconds(block.pauseAfterPrinting);

            if (block.leaveOnScreen)
            {
                yield return null;
            }
            else
            {
                displayText.text = "";
            }
        }
        yield return null;
    }
    public IEnumerator SequenceFader(TextBlock[] blocks, float pauseTime)
    {
        displayText.text = "";

        WaitForSeconds pause = new WaitForSeconds(pauseTime);

        foreach (TextBlock block in blocks)
        {
            string[] words = block.text.Split(char.Parse(" "));
            for (int i = 0; i < words.Length; i++)
            {
                displayText.text += words[i] + " ";
                yield return pause;
            }

            yield return new WaitForSeconds(block.pauseAfterPrinting);

            if (block.leaveOnScreen)
            {
                yield return null;
            }
            else
            {
                displayText.text = "";
            }
        }
        yield return null;
    }
}
