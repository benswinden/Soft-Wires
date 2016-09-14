using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Gizmo : MonoBehaviour {


    public int slot;
    public Transform parent;

    bool hoverActive;


    Vector3 startMousePosition;

    public LineRenderer lineRen;

    void Awake() {

        parent = transform.parent;
    }

    void Update() {

        if (hoverActive) {

            //if (lineRen != null && follower != null) {                
            //    lineRen.SetPosition(0,  Manager.currentCamera.ScreenToWorldPoint( Manager.worldUICamera.WorldToScreenPoint(lineRen.transform.position)));
            //    lineRen.SetPosition(1, follower.transform.position);
            //}


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
        Manager.user.gizmoHover(gameObject);
    }

    public void hoverExit() {

        Manager.worldUI.selectionHoverExit();

        hoverActive = false;
        Manager.user.gizmoHoverExit();
    }

}