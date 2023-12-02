using System.Collections;
using TMPro;
using UnityEngine;

public class TextFader : MonoBehaviour
{
    //COMPLETE!
    //TO DO 1: convert coroutines into one coroutine. The coroutine should loop through each letter in the string adding a time buffer for each to delay their fade.
    //something like `while the last letter in the string isn't opaque yet` `for each letter loop through` clamp the time step so when its below zero the letter stays on 0 but when its above 255 the letter stays opaque;
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
            HudolHouse.TextAnimators.DisplayText(originalText, textMeshPro );
        }
    }

    public void DisplayText(string text)
    {
        originalText = text;
        StartCoroutine(AnimateText());
    }

    private IEnumerator AnimateText()
    {
       // textMeshPro.text = string.Empty;
        //textMeshPro.ForceMeshUpdate();

        //textMeshPro.text = originalText;
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
            // Get the current character's alpha value.
            //byte alpha = (byte)Mathf.Clamp(newVertexColors[vertexIndex + 0].a - fadeSteps, 0, 255);
            // Set new alpha values.
            newVertexColors[vertexIndex + 0].a = (byte)0;
            newVertexColors[vertexIndex + 1].a = (byte)0;
            newVertexColors[vertexIndex + 2].a = (byte)0;
            newVertexColors[vertexIndex + 3].a = (byte)0;

            //textMeshPro.textInfo.characterInfo[characterIndex].color = textColor;

            // Update the vertex color with the modified alpha value.
            textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            //yield return new WaitForSeconds(characterDelay);
        }

        for (int i = 0; i < originalText.Length; i++)
        {
            // Create the text with fading characters up to the current index.
            //textMeshPro.text += originalText[i];
            int characterIndex = i;

            // Start fading the current character.
            StartCoroutine(FadeCharacter(characterIndex));

            // Wait for the character delay.
            yield return new WaitForSeconds(characterDelay);
        }
    }

    private IEnumerator FadeCharacter(int characterIndex)
    {
        
        float elapsedTime = 0f;

        // combined code
        TMP_TextInfo textInfo = textMeshPro.textInfo;
        Color32[] newVertexColors;
        
        

        while (elapsedTime < letterFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            byte alpha = (byte)Mathf.Lerp(0, 255, elapsedTime / letterFadeDuration);
            // Skip characters that are not visible
            if (!textInfo.characterInfo[characterIndex].isVisible) continue;
            // Get the index of the material used by the current character.
            int materialIndex = textInfo.characterInfo[characterIndex].materialReferenceIndex;
            // Get the vertex colors of the mesh used by this text element (character or sprite).
            newVertexColors = textInfo.meshInfo[materialIndex].colors32;
            // Get the index of the first vertex used by this text element.
            int vertexIndex = textInfo.characterInfo[characterIndex].vertexIndex;
            // Get the current character's alpha value.
            //byte alpha = (byte)Mathf.Clamp(newVertexColors[vertexIndex + 0].a - fadeSteps, 0, 255);
            // Set new alpha values.
            newVertexColors[vertexIndex + 0].a = alpha;
            newVertexColors[vertexIndex + 1].a = alpha;
            newVertexColors[vertexIndex + 2].a = alpha;
            newVertexColors[vertexIndex + 3].a = alpha;

            //textMeshPro.textInfo.characterInfo[characterIndex].color = textColor;

            // Update the vertex color with the modified alpha value.
            textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            yield return null;
        }
    }
}
