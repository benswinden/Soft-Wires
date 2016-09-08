using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Rea : MonoBehaviour {


    public float turnSpeed = 1;
    public float moveSpeed = 1;

    public float moveDrag = 4;

    public GameObject mesh;
    public Crosshair crosshair;

    public Camera topDownCamera;
    public Camera FPCamera;

    public bool FPMode { get; set; }

    Quaternion rotationTowardsTarget;    
    new Rigidbody rigidbody;


    void Awake() {

        Manager.rea = this;
        rigidbody = GetComponent<Rigidbody>();

    }

    void Update() {

        mesh.transform.Rotate(new Vector3(0, 0.1f, 0));

        if (Input.GetMouseButtonUp(1))
            toggleMode();        

    }

    void FixedUpdate() {

        if (!FPMode) {
            if (crosshair.mouseDown) {

                rigidbody.drag = moveDrag;

                rotationTowardsTarget = Quaternion.AngleAxis(Mathf.Atan2(crosshair.transform.position.y - transform.position.y, crosshair.transform.position.x - transform.position.x) * 180 / Mathf.PI - 90, transform.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotationTowardsTarget, Time.deltaTime * turnSpeed);

                rigidbody.AddForce(transform.up * moveSpeed);

            }
            else {

                rigidbody.drag = 0;
            }
        }
        else {


        }
    }

    void toggleMode() {

        if (FPMode) {

            FPMode = false;
            topDownCamera.enabled = true;
            FPCamera.enabled = false;
        }
        else {

            FPMode = true;
            topDownCamera.enabled = false;
            FPCamera.enabled = true;
        }
    }
}