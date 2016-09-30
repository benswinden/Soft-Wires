using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class WorldUICamera : MonoBehaviour {


    void Awake() {

        Manager.worldUICamera = GetComponent<Camera>();
    }

}