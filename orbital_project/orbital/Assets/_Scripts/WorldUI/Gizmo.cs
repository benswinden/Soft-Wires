using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Gizmo : MonoBehaviour {
    
    public GameObject follower;

    public GameObject slot { get; set; }

    bool hoverActive;


    Vector3 startMousePosition;

    void Update() {

        if (hoverActive) {


            var mousePos = Input.mousePosition;
            mousePos.z = Vector3.Distance(transform.position, Manager.worldUICamera.transform.position);
            mousePos = Manager.worldUICamera.ScreenToWorldPoint(mousePos);

            if (Vector3.Distance( startMousePosition, mousePos) > 30) {

                hoverExit();
            }

            if (Input.GetMouseButtonDown(0)) {
                
                gameObject.SendMessage("Toggle",SendMessageOptions.DontRequireReceiver);
            }
        }
    }


    void OnMouseEnter() {


        var mousePos = Input.mousePosition;
        mousePos.z = Vector3.Distance(transform.position, Manager.worldUICamera.transform.position);
        mousePos = Manager.worldUICamera.ScreenToWorldPoint(mousePos);

        startMousePosition = mousePos;

        Manager.worldUI.selectionHover(gameObject);

        hoverActive = true;
        Manager.rea.gizmoHover(gameObject);
    }

    public void hoverExit() {

        Manager.worldUI.selectionHoverExit();

        hoverActive = false;
        Manager.rea.gizmoHoverExit();
    }

}