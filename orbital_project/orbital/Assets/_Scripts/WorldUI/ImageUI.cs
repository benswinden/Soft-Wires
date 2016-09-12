using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class ImageUI : MonoBehaviour {

    public List<ImageButton> imageButtons;

    public GameObject selector;


    bool hovering;


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