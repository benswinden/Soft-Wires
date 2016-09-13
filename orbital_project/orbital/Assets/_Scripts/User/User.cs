using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class User : MonoBehaviour {

    [Header("Variables")]
    public float TDturnSpeed = 1;
    public float FPturnSpeed = 1;
    public float moveSpeed = 1;

    public float moveDrag = 4;
    public float driftDrag = 1;    

    public float grappleShotForce = 1000;


    [Header("Containers")]
    public GameObject body;
    public GameObject grapple;    
    public Camera topDownCamera;
    public Camera FPCamera;    
    public GameObject TDCrosshair;
    public GameObject FPCrosshair;    

    [Space]
    public bool debug;

    bool hovering;
    GameObject hoveringFollower;
    GameObject hoveringGizmo;

    bool grappleShot;    

    List<GameObject> followerList = new List<GameObject>();

    Quaternion _lookRotation;
    Vector3 _direction;

    Quaternion rotationTowardsTarget;    
    Rigidbody bodyRigidbody;
    
    // Properties
    public bool FPMode { get; set; }
    public CameraGizmo fpGizmo { get; set; }

    public List<Crosshair> activeCrosshairs { get; set; }

    
    void Awake() {

        Manager.user = this;
        Manager.currentCamera = topDownCamera;

        bodyRigidbody = body.GetComponent<Rigidbody>();

        activeCrosshairs = new List<Crosshair>();
    }

    void Update() {

        if (debug) {
            Debug.DrawRay(body.transform.position, transform.forward * 1000, Color.blue);
            Debug.DrawRay(body.transform.position, transform.up * 1000, Color.green);
            Debug.DrawRay(body.transform.position, transform.right * 1000, Color.red);
        }

    }

    void FixedUpdate() {


        if (Input.GetMouseButton(0) && !hovering) {

            bodyRigidbody.drag = moveDrag;

            // Check if the mouse is currently over a gizmo display
            bool overDisplay = false;
            var mousePos = Input.mousePosition;
            mousePos.z = 96;
            mousePos = Manager.worldUICamera.ScreenToWorldPoint(mousePos);
            Ray ray = new Ray(mousePos, Vector3.forward);

            Debug.DrawRay(mousePos, Vector3.forward * 10, Color.magenta, 1);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f)) {

                overDisplay = true;
            }
            
            // Mouse is hovering over a display, so rotate us based on that ( For now just FP, would need to be adapted to support different cameras
            if (overDisplay) {

                var targetVector = (FPCrosshair.transform.position - body.transform.position).normalized;
                var rotationTarget = Quaternion.LookRotation(targetVector);

                body.transform.rotation = Quaternion.Slerp(body.transform.rotation, rotationTarget, Time.deltaTime * FPturnSpeed);
            }
            // Normal Top down rotation
            else {
                
                rotationTowardsTarget = Quaternion.AngleAxis(Mathf.Atan2(TDCrosshair.transform.position.z - body.transform.position.z, TDCrosshair.transform.position.x - body.transform.position.x) * 180 / Mathf.PI - 90, -body.transform.up);
                body.transform.rotation = Quaternion.Slerp(body.transform.rotation, rotationTowardsTarget, Time.deltaTime * TDturnSpeed);                
            }            

            bodyRigidbody.AddForce(body.transform.forward * moveSpeed);

        }
        else if (Input.GetMouseButtonDown(0) && hovering) {

            // FOLLOWER HOVER
            if (hoveringFollower != null) {
                
                if (followerList.Contains(hoveringFollower)) {

                    hoveringFollower.GetComponent<Follower>().deactivate();
                    followerList.Remove(hoveringFollower);

                    //Manager.currentCrosshair.selectorInactive();
                }
                else
                    shootGrapple();
            }
            // GIZMO HOVER
            else {


            }
        }
        else {

            bodyRigidbody.drag = driftDrag;

        }                
    }

    public void toggleMode() {


        if (hoveringFollower != null) {

            foreach (Crosshair crosshair in activeCrosshairs)
                crosshair.selectorInactive();

            hovering = false;
            hoveringFollower.GetComponent<Follower>().hoverExit();
        }

        if (FPMode) {

            if (fpGizmo.activated)
                fpGizmo.Toggle();

            Manager.currentCamera = topDownCamera;

            topDownCamera.transform.position = body.transform.position + (transform.up * 1000);
            topDownCamera.transform.rotation = body.transform.rotation;
            topDownCamera.transform.Rotate(new Vector3(90, 0, 0));

            FPMode = false;
            topDownCamera.enabled = true;
            FPCamera.enabled = false;            

            //TDCrosshair.GetComponent<Crosshair>().activate();
            TDCrosshair.transform.rotation = body.transform.rotation;
            FPCrosshair.GetComponent<Crosshair>().deactivate();
        }
        else {

            Manager.currentCamera = FPCamera;

            FPMode = true;            
            FPCamera.enabled = true;            

            //TDCrosshair.GetComponent<Crosshair>().deactivate();
            FPCrosshair.GetComponent<Crosshair>().activate();

        }
    }

    public void resetRotation() {

        body.transform.rotation = Quaternion.LookRotation(Vector3.forward);
        topDownCamera.transform.position = body.transform.position + (transform.up * 1000);
        topDownCamera.transform.rotation = body.transform.rotation;
        topDownCamera.transform.Rotate(new Vector3(90, 0, 0));

        if (!FPMode)
            TDCrosshair.transform.rotation = body.transform.rotation;
    }

    public void gizmoHover(GameObject gizmo) {

        hoveringGizmo = gizmo;        
        hovering = true;

        foreach (Crosshair crosshair in activeCrosshairs)
            crosshair.deactivateCursor();
    }

    public void gizmoHoverExit() {

        hoveringGizmo = null;        
        hovering = false;

        foreach (Crosshair crosshair in activeCrosshairs)
            crosshair.reactivateCursor();
    }


    public void followerHover(Follower follower) {

        hoveringFollower = follower.gameObject;        
        hovering = true;

        foreach (Crosshair crosshair in activeCrosshairs)
            crosshair.selectorActive(follower.gameObject);
    }

    public void followerHoverExit() {

        hoveringFollower = null;        
        hovering = false;

        foreach (Crosshair crosshair in activeCrosshairs)
            crosshair.selectorInactive();
    }

    void shootGrapple() {

        if (!grappleShot) {

            GameObject grap = Instantiate(grapple, body.transform.position, Quaternion.identity) as GameObject;
            
            grap.transform.LookAt(hoveringFollower.transform);
            grap.GetComponent<Rigidbody>().AddForce(grap.transform.forward * grappleShotForce, ForceMode.Impulse);
                        
            grappleShot = true;
        }
    }

    public void grappleDeath() {

        grappleShot = false;
    }

    public void followerHit(GameObject follower) {
        
        followerList.Add(follower);
        //Manager.currentCrosshair.selectorInactive();
        follower.GetComponent<Follower>().activate();        
        grappleDeath();        
    }
}