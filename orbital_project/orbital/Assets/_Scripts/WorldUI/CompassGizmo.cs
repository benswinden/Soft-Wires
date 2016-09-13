using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class CompassGizmo : MonoBehaviour {

    public float turnSpeed = 1;

    public GameObject cone;

    bool randomRotation;


    void Update() {
        
        var targetVector = Manager.user.body.transform.forward;
        var rotationTarget = Quaternion.LookRotation(targetVector);

        cone.transform.rotation = Quaternion.Slerp(cone.transform.rotation, rotationTarget, Time.deltaTime * turnSpeed);

    }


    public void Toggle() {

        Manager.user.resetRotation();
    }

}