using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class WorldUI : MonoBehaviour {


    public float distanceToExitHover = 25;

    public DataDisplayFrame worldImageHolder;


    GameObject currentGizmoHover;


    public bool activated { get; set; }


    void Awake() {

        activated = true;
        Manager.worldUI = this;
    }

    public void activate() {

        if (!activated) {

            Manager.worldUICamera.enabled = true;

            activated = true;
        }
    }

    public void deactivate() {

        if (activated) {

            Manager.worldUICamera.enabled = false;

            activated = false;
        }
    }
}