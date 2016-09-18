using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class GizmoUI : MonoBehaviour {


    public List<GizmoSlot> gizmoUISlots;


    void Awake() {
        Manager.gizmoUI = this;
    }
    
    // Use this to add a gizmo into the UI in a random slot
    public bool addGizmo(Gizmo gizmo) {

        List<GizmoSlot> emptySlots = new List<GizmoSlot>();

        foreach (GizmoSlot slot in gizmoUISlots) {

            if (slot.gizmo == null)
                emptySlots.Add(slot);
        }

        if (emptySlots.Count > 0) {
            GizmoSlot slot = emptySlots[Random.Range(0, emptySlots.Count)];
            gizmo.gameObject.SetActive(true);
            gizmo.transform.position = slot.transform.position;
            gizmo.transform.parent = slot.transform;
            slot.gizmo = gizmo;
        }
        else
            return false;

        return true;

    }

    // Use these two to add a gizmo into the UI in a specific slot
    public bool addGizmo(Gizmo gizmo, GizmoSlot slot) {

        gizmo.gameObject.SetActive(true);
        gizmo.transform.position = slot.transform.position;
        gizmo.transform.parent = slot.transform;
        slot.gizmo = gizmo;

        return true;

    }

    public bool addGizmo(Gizmo gizmo, int slotNumber) {

        gizmo.gameObject.SetActive(true);
        gizmo.transform.position = gizmoUISlots[slotNumber].transform.position;
        gizmo.transform.parent = gizmoUISlots[slotNumber].transform;
        gizmoUISlots[slotNumber].gizmo = gizmo;

        return true;

    }

    public void removeGizmo(Gizmo gizmo) {

        foreach (GizmoSlot slot in gizmoUISlots) {

            if (slot.gizmo == gizmo) {

                gizmo.transform.parent = gizmo.parent;
                gizmo.gameObject.SetActive(false);
                slot.gizmo = null;
            }
        }
    }

    public void removeGizmo(Gizmo gizmo, GizmoSlot slot) {

        gizmo.transform.parent = gizmo.parent;
        gizmo.gameObject.SetActive(false);
        slot.gizmo = null;
    }

    public void removeAllGizmos() {

        foreach (GizmoSlot slot in gizmoUISlots) {

            if (slot.gizmo != null) {

                slot.gizmo.transform.parent = slot.gizmo.parent;
                slot.gizmo.gameObject.SetActive(false);
                slot.gizmo = null;
            }
        }
    }
}