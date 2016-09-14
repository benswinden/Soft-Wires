using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class CameraMoveFront : MonoBehaviour {

    public float moveSpeed = 0.15F;
    public float rotationSpeed = 1;
    public GameObject userBody;

    public Camera actualCamera;

    private Vector3 velocity = Vector3.zero;

    void Start() {

        userBody = Manager.user.currentBody.gameObject;

        transform.position = userBody.transform.position + (userBody.transform.forward * 16);
        transform.rotation = Quaternion.LookRotation(userBody.transform.forward);
    }


    void LateUpdate() {

        var pos = userBody.transform.position + (userBody.transform.forward * 16);
        var rot = Quaternion.LookRotation(userBody.transform.forward);

        transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, Time.deltaTime * moveSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
    }

    public void resetRotation() {


    }
}