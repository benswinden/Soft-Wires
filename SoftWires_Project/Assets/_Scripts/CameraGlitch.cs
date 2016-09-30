using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class CameraGlitch : MonoBehaviour {

    public float changeAmount = 0.01f;

    bool glitching;

    Kino.AnalogGlitch glitchComponent;

    float startAmount;

    void Awake() {

        glitchComponent = GetComponent<Kino.AnalogGlitch>();
        startAmount = glitchComponent.scanLineJitter;
    }


    void Update() {

        if (glitching && glitchComponent.scanLineJitter < 1) {

            glitchComponent.scanLineJitter += changeAmount;
        }
        else if (!glitching && glitchComponent.scanLineJitter > startAmount) {

            glitchComponent.scanLineJitter -= changeAmount;
        }
        else if (glitchComponent.scanLineJitter < startAmount)
            glitchComponent.scanLineJitter = startAmount;
    }

    public void startGlitch(float time) {

        glitching = true;
        StartCoroutine(doGlitch(time));
    }

    IEnumerator doGlitch(float time) {

        yield return new WaitForSeconds(time);

        glitching = false;
    }

}