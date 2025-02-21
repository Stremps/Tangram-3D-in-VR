using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public bool fadeOnStart = true;
    public float fadeDuration = 2f;
    public Color fadeColor;
    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();

        if(fadeOnStart){
            FadeIn();
        }
    }

    public void FadeIn(){
        Fade(1,0);
    }

    public void FadeOut(){
        Fade(0, 1);
    }

    public void Fade(float alphaIn, float alphaOut){
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    public IEnumerator FadeRoutine(float alphaIn, float alphaOut){
        float timer = 0f;

        while(timer <= fadeDuration){
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer/fadeDuration); // Here are going to be the interpolation
            rend.material.SetColor("_BaseColor", newColor);

            timer+= Time.deltaTime; // The timer is increased by the delta time
            yield return null;
        }

        Color newColor2 = fadeColor;
        newColor2.a = alphaOut; // Here are going to be the interpolation
        rend.material.SetColor("_BaseColor", newColor2);
    }
}
