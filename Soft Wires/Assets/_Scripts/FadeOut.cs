using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class FadeOut : MonoBehaviour {

    public bool doscale = true;

    public float fadeSpeed = 0.01f;
    public float scaleSpeed = 0.01f;

    public float startScale = 4;



    bool fadeOut;
    public float alpha;
    float scale;


    void Start() {

        Destroy(gameObject, 6);

        fadeOut = true;        
                
        if (doscale)
        scale = startScale;        
    }

    void Update() {

        if (fadeOut) {

            scale += scaleSpeed;

            if (doscale)
            transform.localScale = new Vector3(scale, scale, scale);

            alpha -= fadeSpeed;
            GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, alpha);
        }
    }

    IEnumerator wait() {

        yield return new WaitForSeconds(1.0f);

        fadeOut = true;
    }

}