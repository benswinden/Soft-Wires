using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class GizmoSlot : MonoBehaviour {

    public Gizmo gizmo { get; set; }

    void Start() {

        foreach (Transform child in transform) {

            if (child.GetComponent<Gizmo>()) {

                Manager.gizmoUI.addGizmo(child.GetComponent<Gizmo>(), this);
            }
        }
    }
}