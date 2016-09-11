using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Compass : MonoBehaviour {

    public float turnSpeed = 1;

    public GameObject cone;

    bool randomRotation;
    GameObject closest;

    void Start() {

        StartCoroutine("checkForPoints");
    }

    void Update() {

        if (closest != null) {

            var targetVector = (closest.transform.position - Manager.rea.transform.position).normalized;
            targetVector = Manager.rea.transform.forward;
            var rotationTarget = Quaternion.LookRotation(targetVector);

            cone.transform.rotation = Quaternion.Slerp(cone.transform.rotation, rotationTarget, Time.deltaTime * turnSpeed);
        }
    }


    public void Toggle() {

        Manager.rea.resetRotation();
    }

    IEnumerator checkForPoints() {

        yield return new WaitForSeconds(2.0f);

        if (Manager.pointsOfInterest.Count > 0) {

            closest = Manager.pointsOfInterest[0];
            
            for (int i = 1; i < Manager.pointsOfInterest.Count; i++) {
                
                if (Vector3.Distance(transform.position, Manager.pointsOfInterest[i].transform.position) < Vector3.Distance(transform.position, closest.transform.position))
                    closest = Manager.pointsOfInterest[i];
            }
        }
        else {
            closest = null;
        }

        StartCoroutine("checkForPoints");
    }
}