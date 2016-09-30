using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class PointOfInterest : MonoBehaviour {


    void Awake() {

        Manager.pointsOfInterest.Add(gameObject);
    }
}