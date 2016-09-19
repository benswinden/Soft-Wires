using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class UIButton : MonoBehaviour {

    public bool startHidden;

    public GameObject selector;

    public List<MeshRenderer> thingsToChangeColor;    
    public List<GameObject> thingsToDisable;

    public Material matBlack;
    public Material matGreen;

    [Space]

    public bool debug;


    bool hidden;

    bool hoverActive;
    Vector3 startMousePosition;

    public bool activated { get; set; }


    void Start() {

        if (startHidden)
            hide();
        else
            seek();
    }

    void Update() {

        var mousePos = Input.mousePosition;
        mousePos.z = Vector3.Distance(transform.position, Manager.worldUICamera.transform.position);
        mousePos = Manager.worldUICamera.ScreenToWorldPoint(mousePos);

        if (debug) Debug.Log("Activated: " + activated + "    Hidden: " + hidden + "    Hover Active: " + hoverActive + "    Start Mouse Pos: " + startMousePosition + "    Current Mouse Pos: " + mousePos + "    Diff: " + Vector3.Distance(startMousePosition, mousePos));

        if (!hidden && hoverActive) {            

            if (Vector3.Distance(startMousePosition, mousePos) > Manager.worldUI.distanceToExitHover) {

                hoverExit();
            }

            if (Input.GetMouseButtonDown(0)) {

                gameObject.SendMessage("Toggle", SendMessageOptions.DontRequireReceiver);
            }
        }
    }


    void OnMouseEnter() {

        if (!hidden) {

            hoverEnter();
        }
    }

    void hoverEnter() {        

        //  Store the mouse start position so we can check against that to see when we should stop hovering
        var mousePos = Input.mousePosition;
        mousePos.z = Vector3.Distance(transform.position, Manager.worldUICamera.transform.position);
        mousePos = Manager.worldUICamera.ScreenToWorldPoint(mousePos);

        startMousePosition = mousePos;
        hoverActive = true;
            
        Manager.user.UIButtonHover(this);

        StartCoroutine("waitToActivateSelector");        
    }

    IEnumerator waitToActivateSelector() {

        yield return new WaitForSeconds(0.15f);

        selector.SetActive(true);
        selector.transform.position = transform.position;
    }

    public void hoverExit() {

        StopCoroutine("waitToActivateSelector");

        hoverActive = false;

        Manager.user.UIButtonHoverExit(this);

        selector.SetActive(false);    
    }

    // We are hovering, and want to stop hovering without using hoverExit
    public void deactivate() {

        hoverActive = false;
    }

    // Default things that all buttons should do when toggled, more specific tasks are in their respective components
    public void Toggle() {

        if (activated) {

            foreach (MeshRenderer obj in thingsToChangeColor) {

                obj.material = matBlack;
            }

            activated = false;

        }
        else {

            foreach (MeshRenderer obj in thingsToChangeColor) {

                obj.material = matGreen;
            }

            activated = true;
        }        
    }

    public void seek() {

        hidden = false;

        foreach (GameObject obj in thingsToDisable) {

            obj.SetActive(true);
        }

        foreach (MeshRenderer obj in thingsToChangeColor) {

            obj.material = matBlack;
        }
    }

    public void hide() {

        if (activated) gameObject.SendMessage("Toggle", SendMessageOptions.DontRequireReceiver);

        hidden = true;

        foreach (GameObject obj in thingsToDisable) {

            obj.SetActive(false);
        }
    }
}