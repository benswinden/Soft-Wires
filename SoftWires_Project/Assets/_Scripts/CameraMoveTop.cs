using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class CameraMoveTop : MonoBehaviour {

    public float dampTime = 0.15F;      

    public Camera actualCamera;

    private Vector3 velocity = Vector3.zero;


    void LateUpdate() {

        var targetPos = Manager.user.currentBody.gameObject.transform.position + (Manager.user.currentBody.gameObject.transform.up * 1000);

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, dampTime);
    }

    public void resetRotation() {

        //transform.position = Manager.user.currentBody.gameObject.transform.position + (Manager.user.currentBody.gameObject.transform.up * 1000);
        transform.rotation = Manager.user.currentBody.gameObject.transform.rotation;
 
    }

    public void slowMovement() {


    }
}