using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Gizmo : MonoBehaviour {


    public int slot;
    public Transform parent;

    void Awake() {

        parent = transform.parent;
    }
}