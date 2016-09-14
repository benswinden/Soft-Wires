using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Crosshair : MonoBehaviour {

    public bool activated;

    public float moveSpeed;
    public float _MINMOVEDISTANCE;

    public float distanceFromCamera = 1000;

    public Material matBlack;
    public Material matGreen;

    public bool topDown;

    public Camera attachedCamera;               // The camera associated with this crosshair

    public GameObject normalMesh;
    public GameObject selectorMesh;


    float minMoveDistance;
    new Rigidbody rigidbody;
    Vector3 lastMousePosition;

    bool selectorActivated;



    void Awake() {

        //CreateLineMaterial();

        rigidbody = GetComponent<Rigidbody>();
        minMoveDistance = _MINMOVEDISTANCE;
    }

    void Start() {

        if (activated) {
            activated = false;
            activate();
        }
        else {
            activated = true;
            deactivate();
        }
    }

    void Update() {

        if (activated) {

            // Change color when the mouse is held
            if (Input.GetMouseButtonDown(0)) {

                foreach (MeshRenderer ren in normalMesh.GetComponentsInChildren<MeshRenderer>()) {
                    ren.material = matGreen;
                }                
            }

            if (Input.GetMouseButtonUp(0)) {

                foreach (MeshRenderer ren in normalMesh.GetComponentsInChildren<MeshRenderer>()) {
                    ren.material = matBlack;
                }
            }
        }
    }

    void FixedUpdate() {

        if (activated && !selectorActivated) {

            var mousePos = Input.mousePosition;                        
            mousePos.z = distanceFromCamera;                     
            mousePos = attachedCamera.ScreenToWorldPoint(mousePos);            

            if (Vector3.Distance(mousePos, lastMousePosition) > 12) {
                minMoveDistance = _MINMOVEDISTANCE;
            }
            else {
                if (minMoveDistance >= 10)
                    minMoveDistance -= 1;
            }

            print("distance from cam: " + Vector3.Distance(transform.position, attachedCamera.transform.position) + "    player dist: " + Vector3.Distance(Manager.user.body.transform.position, attachedCamera.transform.position));

            lastMousePosition = mousePos;

            if (Vector3.Distance(mousePos, transform.position) > minMoveDistance) {

                rigidbody.AddForce((mousePos - transform.position).normalized * moveSpeed);
            }

            if (!topDown) {

                transform.rotation = Manager.user.body.transform.rotation;
            }
        }
        else if (activated && selectorActivated) {

            transform.position = hoveringFollower.transform.position;
        }

    }

    public void activate() {

        if (!activated) {

            Manager.user.activeCrosshairs.Add(this);

            activated = true;

            foreach (MeshRenderer ren in GetComponentsInChildren<MeshRenderer>()) {
                ren.enabled = true;
            }

            var mousePos = Input.mousePosition;
            mousePos.z = distanceFromCamera;
            mousePos = attachedCamera.ScreenToWorldPoint(mousePos);    

            transform.position = mousePos;
        }
    }

    public void deactivate() {

        if (activated) {

            Manager.user.activeCrosshairs.Remove(this);

            activated = false;            
        }
    }

    GameObject hoveringFollower;

    public void deactivateCursor() { 

        activated = false;        
        normalMesh.SetActive(false);
    }

    public void reactivateCursor() {

        activated = true;
        normalMesh.SetActive(true);
    }

    public void selectorActive(GameObject follower) {

        hoveringFollower = follower;
        rigidbody.velocity = Vector3.zero;

        transform.position = follower.transform.position;
        selectorActivated = true;
        normalMesh.SetActive(false);
        selectorMesh.SetActive(true);
    }

    public void selectorInactive() {

        
        selectorActivated = false;
        normalMesh.SetActive(true);
        selectorMesh.SetActive(false);
    }
}