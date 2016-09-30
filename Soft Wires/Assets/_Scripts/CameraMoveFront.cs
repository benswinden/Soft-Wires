using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class CameraMoveFront : MonoBehaviour {

    public float moveSpeed = 0.15F;
    public float rotationSpeed = 1;    

    public Camera actualCamera;

    private Vector3 velocity = Vector3.zero;    
    

    void LateUpdate() {

        var pos = Manager.user.currentBody.gameObject.transform.position + (Manager.user.currentBody.gameObject.transform.forward * 16);
        var rot = Quaternion.LookRotation(Manager.user.currentBody.gameObject.transform.forward);

        transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, Time.deltaTime * moveSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
    }

    public void resetRotation() {


    }
}