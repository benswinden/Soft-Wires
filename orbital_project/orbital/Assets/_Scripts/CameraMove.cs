using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class CameraMove : MonoBehaviour {

    public float dampTime = 0.15F;  
    public GameObject User;


    private Vector3 velocity = Vector3.zero;

    void Awake() {

        transform.position = new Vector3(User.transform.position.x, transform.position.y, User.transform.position.z);
    }


    void LateUpdate() {

        var targetPos = Manager.user.body.transform.position + (Manager.user.body.transform.up * 1000);

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, dampTime);
    }
}