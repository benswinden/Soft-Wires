using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class UISlot : MonoBehaviour {

    public Gizmo gizmo { get; set; }

    void Start() {

        foreach (Transform child in transform) {

            if (child.GetComponent<Gizmo>()) {

                Manager.worldUI.addGizmo(child.GetComponent<Gizmo>(), this);
            }
        }
    }
}