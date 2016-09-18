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



    public void selectionHover(GameObject uiObject, GameObject selector) {        
                
        Manager.user.UIButtonHover(gameObject);

        StartCoroutine(wait(uiObject, selector));
    }

    IEnumerator wait(GameObject uiObject, GameObject selector) {

        yield return new WaitForSeconds(0.15f);

        selector.SetActive(true);
        selector.transform.position = uiObject.transform.position;
    }


    public void selectionHoverExit(GameObject selector) {

        Manager.user.UIButtonHoverExit(gameObject);

        selector.SetActive(false);        
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