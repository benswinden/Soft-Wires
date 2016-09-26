using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class FadeIn : MonoBehaviour {

    public float fadeSpeed = 0.1f;


    bool fading;
    float alpha = 0;

    void Update() {

        if (fading) {
            alpha += fadeSpeed;
            GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, alpha);
        }
    }

    public void Fade() {

        fading = true;
    }
}