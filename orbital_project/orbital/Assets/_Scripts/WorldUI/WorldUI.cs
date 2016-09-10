using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class WorldUI : MonoBehaviour {

    public GameObject selector;

    public List<GameObject> leftUISlots;
    public List<GameObject> rightUISlot;

    bool hovering;
    GameObject currentGizmoHover;


    void Awake() {

        Manager.worldUI = this;
    }

    public void selectionHover(GameObject gizmo) {
        
        selector.SetActive(true);
        selector.transform.position = gizmo.transform.position;
        hovering = true;
    }

    public void selectionHoverExit() {
        
        selector.SetActive(false);
        hovering = false;
    }
}