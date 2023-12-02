using System.Collections;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

namespace HudolHouse
{
    //TO DO 2: test out cancelation of asyncs - DONE
    //To Do 3: convert the TextFader coroutines to async events in this script - DONE.
    //TO Do 4: make cancelable. Everytime the function is attempted, add the tmp text to list.
    //also create a cencelationtoken and add it to a list too. fore runing the function, check if the tmp is already in the list, if so use the assosiated cencelation token to cancel the function

    public static class TextAnimators
    {
        //static CancellationTokenSource cts;

        private static List<TMP_Text> activeTexts = new List<TMP_Text>();
        private static List<CancellationTokenSource> activeTokens = new List<CancellationTokenSource>();


        public static async void DisplayText(string text, TMP_Text tmp, float letterFadeDuration = 2f, float characterDelay = 0.02f)
        {
            //if the tmp passed in already has a 
            if (activeTexts.Contains(tmp))
            {
                Debug.Log("Text is in use");
                int index = activeTexts.FindIndex(TMP_Text => TMP_Text == tmp);
                Debug.Log(index);
                activeTokens[index].Cancel();

                

                await FadeOutText(new CancellationTokenSource().Token, text, tmp);

                Debug.Log("Await returns completed");
                activeTokens[index].Dispose();
                activeTokens.Remove(activeTokens[index]);
                activeTexts.Remove(tmp);

            }
            else
            {
                //add the token and text mesh pro to our lists so we can avoid double ups
                CancellationTokenSource token = new CancellationTokenSource();
                activeTokens.Add(token);
                activeTexts.Add(tmp);

                //Run the task and await its finish
                await FadeInText(token.Token, text, tmp, letterFadeDuration, characterDelay);

                //once finished, remove the token and textmeshpro
                activeTexts.Remove(tmp);
                activeTokens.Remove(token);
                token.Dispose();
            }
            
        }

        public static async Task FadeInText(CancellationToken cancellation, string text, TMP_Text tmp, float letterFadeDuration = 2f, float characterDelay = 0.02f)
        {
            //set wait times
            //WaitForEndOfFrame frame = new WaitForEndOfFrame();
            float elapsedTime = 0f;

            //blank all the letters first
            TMP_TextInfo textInfo = tmp.textInfo;
            Color32[] newVertexColors;

            for (int i = 0; i < text.Length; i++)
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
                tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                //yield return new WaitForSeconds(characterDelay);
            }
            await Task.Delay(1);

            //determine how long it will take in total to fade all the letters
            float totalTime = letterFadeDuration + (characterDelay * text.Length);

            if (cancellation.IsCancellationRequested)
            {
                Debug.Log("cancel");
                //tmp.text = "";
                //tmp.ForceMeshUpdate();
                return;
            }

            //attempt to fade all characters
            while (elapsedTime < totalTime)
            {
                if (cancellation.IsCancellationRequested)
                {
                    //throw new OperationCanceledException(cancellation);
                    Debug.Log("cancel");
                    // tmp.text = "";
                    //tmp.ForceMeshUpdate();
                    //await FadeOutText(cancellation, text, tmp);
                    return;
                }
                elapsedTime += Time.deltaTime;

                for (int i = 0; i < text.Length; i++)
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
                tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                await Task.Delay(1);
            }
        }

        public static async Task FadeOutText(CancellationToken cancellation, string text, TMP_Text tmp, float letterFadeDuration = 0.5f, float characterDelay = 0.01f)
        {
            Debug.Log("Attempting to fade out letters");
            Debug.Log(text);
            //set wait times
            
            float elapsedTime = 0f;

            //blank all the letters first
            TMP_TextInfo textInfo = tmp.textInfo;
            Color32[] newVertexColors;


            //determine how long it will take in total to fade all the letters
            float totalTime = letterFadeDuration + (characterDelay * text.Length);

            if (cancellation.IsCancellationRequested)
            {
                Debug.Log("cancel");
                //tmp.text = "";
                //tmp.ForceMeshUpdate();
                return;
            }

            //save starting alpha levels to an array
            List<byte> startingAlpha = new List<byte>();
            for (int i = 0; i < text.Length; i++)
            {
                // Skip characters that are not visible
                //if (!textInfo.characterInfo[i].isVisible) continue;

                // Get the index of the material used by the current character.
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                // Get the vertex colors of the mesh used by this text element (character or sprite).
                newVertexColors = textInfo.meshInfo[materialIndex].colors32;
                // Get the index of the first vertex used by this text element.
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                // Set new alpha values.
                startingAlpha.Add(newVertexColors[vertexIndex].a);
            }

            //attempt to fade all characters
            while (elapsedTime < totalTime)
            {
                Debug.Log("Another loop");
                if (cancellation.IsCancellationRequested)
                {
                    //throw new OperationCanceledException(cancellation);
                    Debug.Log("cancel");
                    //tmp.text = "";
                    //tmp.ForceMeshUpdate();
                    return;
                }
                elapsedTime += Time.deltaTime;

                

                for (int i = 0; i < text.Length; i++)
                {
                    //add the text delay to the fade of each letter
                    float adjustedTime = elapsedTime - (characterDelay * (i + 1));

                    // Skip characters that are not visible
                    if (!textInfo.characterInfo[i].isVisible) continue;

                    // Get the index of the material used by the current character.
                    int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                    // Get the vertex colors of the mesh used by this text element (character or sprite).
                    newVertexColors = textInfo.meshInfo[materialIndex].colors32;
                    // Get the index of the first vertex used by this text element.
                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                    byte alpha = (byte)Mathf.Lerp(255, 0, adjustedTime / letterFadeDuration);

                    alpha = (byte)Mathf.Clamp(alpha, 0, startingAlpha[i]);
                    
                    // Get the current character's alpha value.
                    //byte alpha = (byte)Mathf.Clamp(newVertexColors[vertexIndex + 0].a - fadeSteps, 0, 255);
                    // Set new alpha values.
                    newVertexColors[vertexIndex + 0].a = alpha;
                    newVertexColors[vertexIndex + 1].a = alpha;
                    newVertexColors[vertexIndex + 2].a = alpha;
                    newVertexColors[vertexIndex + 3].a = alpha;

                }
                // Update the vertex color with the modified alpha value.
                tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                await Task.Delay(1);
            }
            Debug.Log("FadeOutCompleted");
            return;
        }

    }
   
}

