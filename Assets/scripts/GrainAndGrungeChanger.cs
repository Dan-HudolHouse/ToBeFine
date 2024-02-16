using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class GrainAndGrungeChanger : MonoBehaviour
{
    public VolumeProfile volume;
    public Renderer grungeRenderer;
    private Material mat;
    public Texture[] grungeTextures;
    private int grungeIndex;
    private Bloom bloom;
    public float intervals;
    bool isActive;
    // Start is called before the first frame update
    void Start()
    {
       volume.TryGet<Bloom>(out bloom);
        mat = grungeRenderer.material;
        StartCoroutine(ChangeTexture(intervals));
        
    }

   IEnumerator ChangeTexture(float interval)
    {
        WaitForSeconds pause = new WaitForSeconds(interval);
        
        while (true)
        {
            mat.SetTexture("_MainTex", grungeTextures[grungeIndex]);
            bloom.dirtTexture.Override(grungeTextures[grungeIndex]);
            yield return pause;
            grungeIndex = (grungeIndex + 1) % grungeTextures.Length;

        }

    }
}
