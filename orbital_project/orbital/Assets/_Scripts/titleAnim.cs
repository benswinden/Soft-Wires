using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class titleAnim : MonoBehaviour {


    public float time;

    public Kino.AnalogGlitch glitch;

    bool glitchout;

    public FadeIn fader;


    void Start() {

        StartCoroutine("wait");
    }

    void Update() {

        if (glitchout)
            glitch.scanLineJitter += 0.01f;
    }

    IEnumerator wait() {

        yield return new WaitForSeconds(time);

        GetComponent<Animation>().Play();

        yield return new WaitForSeconds(10);

        glitchout = true;

        yield return new WaitForSeconds(2);

        fader.Fade();

        yield return new WaitForSeconds(6);

        Application.LoadLevel(1);
    }
}
