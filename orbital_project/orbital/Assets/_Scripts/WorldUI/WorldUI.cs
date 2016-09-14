using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class WorldUI : MonoBehaviour {

    public GameObject selector;

    public List<UISlot> UISlots;


    public WorldImageHolder worldImageHolder;

    GameObject currentGizmoHover;


    void Awake() {

        Manager.worldUI = this;
    }

    // Use this to add a gizmo into the UI
    public bool addGizmo(Gizmo gizmo) {

        List<UISlot> emptySlots = new List<UISlot>();

        foreach (UISlot slot in UISlots) {

            if (slot.gizmo == null)
                emptySlots.Add(slot);
        }
        
        if (emptySlots.Count > 0) {
            UISlot slot = emptySlots[Random.Range(0, emptySlots.Count)];
            gizmo.gameObject.SetActive(true);
            gizmo.transform.position = slot.transform.position;
            slot.gizmo = gizmo;
            gizmo.transform.parent = slot.transform;
        }
        else
            return false;

        return true;

    }

    public bool addGizmo(Gizmo gizmo, UISlot slot) {
        
        gizmo.gameObject.SetActive(true);
        gizmo.transform.position = slot.transform.position;
        slot.gizmo = gizmo;
        gizmo.transform.parent = slot.transform;        

        return true;

    }

    public void removeGizmo(Gizmo gizmo) {
        
        foreach (UISlot slot in UISlots) {

            if (slot.gizmo == gizmo) {

                slot.gizmo = null;
                gizmo.gameObject.SetActive(false);                
            }
        }
    }

    public void removeGizmo(Gizmo gizmo, UISlot slot) {

        slot.gizmo = null;
        gizmo.gameObject.SetActive(false);            
    }

    public void selectionHover(GameObject gizmo) {
        
        selector.SetActive(true);
        selector.transform.position = gizmo.transform.position;        
    }

    public void selectionHoverExit() {
        
        selector.SetActive(false);        
    }
}