using System.Collections;
using TMPro;
using UnityEngine;

public class TextFadersComined : MonoBehaviour
{
    public TMP_Text textMeshPro;
    public float letterFadeDuration = 0.1f;
    public float characterDelay = 0.1f;

    private string originalText;

    public string testText;

    private void Awake()
    {
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TMP_Text>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            DisplayText(testText);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            originalText = testText;
            HudolHouse.TextAnimators.DisplayText(originalText, textMeshPro, letterFadeDuration, characterDelay);
        }
    }
    public void DisplayText(string text)
    {
        originalText = text;
        StartCoroutine(AnimateText());
    }
    private IEnumerator AnimateText()
    {
        //set wait times
        WaitForEndOfFrame frame = new WaitForEndOfFrame();
        float elapsedTime = 0f;
        
        //blank all the letters first
        TMP_TextInfo textInfo = textMeshPro.textInfo;
        Color32[] newVertexColors;

        for (int i = 0; i < originalText.Length; i++)
        {
            // Skip characters that are not visible
            if (!textInfo.characterInfo[i].isVisible) continue;
            // Get the index of the material used by the current character.
            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
            // Get the vertex colors of the mesh used by this text element (character or sprite).
            newVertexColors = textInfo.meshInfo[materialIndex].colors32;
            // Get the index of the first vertex used by this text element.
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
            // Set new alpha values.
            newVertexColors[vertexIndex + 0].a = (byte)0;
            newVertexColors[vertexIndex + 1].a = (byte)0;
            newVertexColors[vertexIndex + 2].a = (byte)0;
            newVertexColors[vertexIndex + 3].a = (byte)0;

            // Update the vertex color with the modified alpha value.
            textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            //yield return new WaitForSeconds(characterDelay);
        }
        yield return frame;

        //determine how long it will take in total to fade all the letters
        float totalTime = letterFadeDuration + (characterDelay * originalText.Length);

        //attempt to fade all characters
        while(elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;

            for (int i = 0; i < originalText.Length; i++)
            {
                //add the text delay to the fade of each letter
                float adjustedTime = elapsedTime - (characterDelay * (i + 1));
                byte alpha = (byte)Mathf.Lerp(0, 255, adjustedTime / letterFadeDuration);
                // Skip characters that are not visible
                if (!textInfo.characterInfo[i].isVisible) continue;
                // Get the index of the material used by the current character.
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                // Get the vertex colors of the mesh used by this text element (character or sprite).
                newVertexColors = textInfo.meshInfo[materialIndex].colors32;
                // Get the index of the first vertex used by this text element.
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                // Get the current character's alpha value.
                //byte alpha = (byte)Mathf.Clamp(newVertexColors[vertexIndex + 0].a - fadeSteps, 0, 255);
                // Set new alpha values.
                newVertexColors[vertexIndex + 0].a = alpha;
                newVertexColors[vertexIndex + 1].a = alpha;
                newVertexColors[vertexIndex + 2].a = alpha;
                newVertexColors[vertexIndex + 3].a = alpha;

            }
            // Update the vertex color with the modified alpha value.
            textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            yield return frame;
        }
    }

}
