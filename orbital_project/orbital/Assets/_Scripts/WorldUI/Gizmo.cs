using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Gizmo : MonoBehaviour {
    
    public GameObject follower;

    public GameObject slot { get; set; }

    bool hoverActive;


    Vector3 startMousePosition;

    public LineRenderer lineRen;

    void Update() {

        if (hoverActive) {

            if (lineRen != null && follower != null) {                
                lineRen.SetPosition(0,  Manager.currentCamera.ScreenToWorldPoint( Manager.worldUICamera.WorldToScreenPoint(lineRen.transform.position)));
                lineRen.SetPosition(1, follower.transform.position);
            }


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

        if (lineRen != null && follower != null) {
            lineRen.SetVertexCount(2);
            lineRen.SetPosition(0, Manager.currentCamera.ScreenToWorldPoint(Manager.worldUICamera.WorldToScreenPoint(lineRen.transform.position)));
            lineRen.SetPosition(1, follower.transform.position);
        }


        var mousePos = Input.mousePosition;
        mousePos.z = Vector3.Distance(transform.position, Manager.worldUICamera.transform.position);
        mousePos = Manager.worldUICamera.ScreenToWorldPoint(mousePos);

        startMousePosition = mousePos;

        Manager.worldUI.selectionHover(gameObject);

        hoverActive = true;
        Manager.user.gizmoHover(gameObject);
    }

    public void hoverExit() {

        if (follower != null && lineRen != null)
            lineRen.SetVertexCount(0);

        Manager.worldUI.selectionHoverExit();

        hoverActive = false;
        Manager.user.gizmoHoverExit();
    }

}