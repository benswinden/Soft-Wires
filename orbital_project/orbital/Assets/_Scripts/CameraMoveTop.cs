using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class CameraMoveTop : MonoBehaviour {

    public float dampTime = 0.15F;  
    public GameObject userBody;

    public Camera actualCamera;

    private Vector3 velocity = Vector3.zero;

    void Start() {

        userBody = Manager.user.currentBody.gameObject;

        transform.position = userBody.transform.position + (userBody.transform.up * 1000);
        transform.rotation = userBody.transform.rotation;
    }


    void LateUpdate() {

        var targetPos = userBody.transform.position + (userBody.transform.up * 1000);

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, dampTime);
    }

    public void resetRotation() {

        //transform.position = userBody.transform.position + (userBody.transform.up * 1000);
        transform.rotation = userBody.transform.rotation;
 
    }

    public void slowMovement() {


    }
}