using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class User : MonoBehaviour {

    public bool startActivated;

    [Header("Variables")]
    public float TDturnSpeed = 1;
    public float FPturnSpeed = 1;
    public float moveSpeed = 1;

    public float moveDrag = 4;
    public float driftDrag = 1;    

    public float grappleShotForce = 1000;


    [Header("Containers")]    
    public GameObject grapple;    
    public CameraMoveTop topCamera;
    public CameraMoveFront frontCamera;    
    public GameObject topCrosshair;
    public GameObject frontCrosshair;    

    [Space]
    public bool debug;


    // Private

    bool hovering;
    GameObject hoveringBody;

    bool grappleShot;        

    Quaternion _lookRotation;
    Vector3 _direction;

    Quaternion rotationTowardsTarget;    
    
    // Properties
    public Body currentBody { get; set; }

    public bool FPMode { get; set; }
    public CameraGizmo fpGizmo { get; set; }

    public List<Crosshair> activeCrosshairs { get; set; }


    public bool activated { get; set; }


    void Awake() {

        if (Manager.user)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        Manager.user = this;
        Manager.currentCamera = topCamera.actualCamera;

        activeCrosshairs = new List<Crosshair>();        
    }

    void Start() {

        // Check for our body
        foreach (Transform child in transform) {

            if (child.GetComponent<Body>())
                switchBody(child.GetComponent<Body>());
        }

        if (startActivated) {
            activated = false;
            activate();
        }
        else {
            activated = true;
            deactivate();
        }
    }

    void Update() {

        if (debug) {
            Debug.DrawRay(currentBody.transform.position, currentBody.transform.forward * 1000, Color.blue);
            Debug.DrawRay(currentBody.transform.position, currentBody.transform.up * 1000, Color.green);
            Debug.DrawRay(currentBody.transform.position, currentBody.transform.right * 1000, Color.red);
        }

    }

    void FixedUpdate() {


        // Check if the mouse is currently over a gizmo display
        //  If it is, set that as the currentCamera, otherwise we user the Top camera as default
        bool overDisplay = false;
        var mousePos = Input.mousePosition;
        mousePos.z = 98;
        mousePos = Manager.worldUICamera.ScreenToWorldPoint(mousePos);
        Ray ray = new Ray(mousePos, Vector3.forward);

        Debug.DrawRay(mousePos, Vector3.forward * 10, Color.magenta, 1);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100.0f)) {
            
            if (hit.collider.tag.Equals("CameraDisplay")) {

                overDisplay = true;
                Manager.currentCamera = frontCamera.actualCamera;                
            }
        }
        else {
            
            Manager.currentCamera = topCamera.actualCamera;            
        }

        if (Input.GetMouseButton(0) && !hovering) {

            currentBody.GetComponent<Rigidbody>().drag = moveDrag;

            
            // Mouse is hovering over a display, so rotate us based on that ( For now just FP, would need to be adapted to support different cameras
            if (overDisplay) {

                var targetVector = (frontCrosshair.transform.position - currentBody.transform.position).normalized;
                var rotationTarget = Quaternion.LookRotation(targetVector);

                currentBody.transform.rotation = Quaternion.Slerp(currentBody.transform.rotation, rotationTarget, Time.deltaTime * FPturnSpeed);
            }
            // Normal Top down rotation
            else {

                //rotationTowardsTarget = Quaternion.AngleAxis(Mathf.Atan2(topCrosshair.transform.position.z - body.transform.position.z, topCrosshair.transform.position.x - body.transform.position.x) * 180 / Mathf.PI - 90, -body.transform.up);
                //body.transform.rotation = Quaternion.Slerp(body.transform.rotation, rotationTowardsTarget, Time.deltaTime * TDturnSpeed);

                var newRotation = Quaternion.LookRotation(topCrosshair.transform.position - currentBody.transform.position, currentBody.transform.up);
                //newRotation.x = 0.0f;
                //newRotation.z = 0.0f;
                currentBody.transform.rotation = Quaternion.Slerp(currentBody.transform.rotation, newRotation, Time.deltaTime * TDturnSpeed);
            }

            currentBody.GetComponent<Rigidbody>().AddForce(currentBody.transform.forward * moveSpeed);

        }
        else if (Input.GetMouseButtonDown(0) && hovering) {

            // FOLLOWER HOVER
            if (hoveringBody != null) {
                
                shootGrapple();
            }
            // GIZMO HOVER
            else {


            }
        }
        else {

            currentBody.GetComponent<Rigidbody>().drag = driftDrag;

        }                
    }




    public void switchBody(Body newBody) {
        

        if (currentBody != null) {

            Manager.worldUICamera.GetComponent<CameraGlitch>().startGlitch(0.2f);

            currentBody.transform.parent = null;
            currentBody.GetComponent<Body>().deactivate();
        }

        newBody.activate();
        newBody.transform.parent = transform;
        currentBody = newBody;

        // Remove all gizmos
        Manager.worldUI.removeAllGizmos();


        // Add gizmos
        List<Gizmo> gizmoList = new List<Gizmo>();
        foreach (Transform child in currentBody.transform) {            

            if (child.GetComponent<Gizmo>()) {

                // Have to add these to a list to use outside of the loop since their transforms get moved which fucks up the loop
                gizmoList.Add(child.GetComponent<Gizmo>());                
            }
        }

        foreach (Gizmo giz in gizmoList) {

            Manager.worldUI.addGizmo(giz, giz.slot);
        }

        resetRotation();
    }



    public void activate() {
        
        if (!activated) {

            // Body
            if (currentBody) currentBody.gameObject.SetActive(true);

            // UI
            if (Manager.worldUI) Manager.worldUI.activate();

            activated = true;
        }
    }

    public void deactivate() {

        if (activated) {

            // Body
            if (currentBody) currentBody.gameObject.SetActive(false);

            // UI
            if (Manager.worldUI) Manager.worldUI.deactivate();

            activated = false;
        }
    }


    public void toggleMode() {


        if (hoveringBody != null) {

            foreach (Crosshair crosshair in activeCrosshairs)
                crosshair.selectorInactive();

            hovering = false;
            hoveringBody.GetComponent<Body>().hoverExit();
        }

        if (FPMode) {

            if (fpGizmo.activated)
                fpGizmo.Toggle();

            topCamera.resetRotation();

            FPMode = false;            
            frontCamera.actualCamera.enabled = false;            

            //TDCrosshair.GetComponent<Crosshair>().activate();
            topCrosshair.transform.rotation = currentBody.transform.rotation;
            frontCrosshair.GetComponent<Crosshair>().deactivate();
        }
        else {

            Manager.currentCamera = frontCamera.actualCamera;

            FPMode = true;            
            frontCamera.actualCamera.enabled = true;            

            //TDCrosshair.GetComponent<Crosshair>().deactivate();
            frontCrosshair.GetComponent<Crosshair>().activate();

        }
    }

    public void resetRotation() {

        currentBody.transform.rotation = Quaternion.LookRotation(Vector3.forward);

        topCamera.resetRotation();

        if (!FPMode)
            topCrosshair.transform.rotation = currentBody.transform.rotation;
    }

    public void gizmoHover(GameObject gizmo) {

        hovering = true;

        foreach (Crosshair crosshair in activeCrosshairs)
            crosshair.deactivateCursor();
    }

    public void gizmoHoverExit() {

        hovering = false;

        foreach (Crosshair crosshair in activeCrosshairs)
            crosshair.reactivateCursor();
    }


    public void followerHover(Body follower) {

        hoveringBody = follower.gameObject;        
        hovering = true;

        foreach (Crosshair crosshair in activeCrosshairs)
            crosshair.selectorActive(follower.gameObject);
    }

    public void followerHoverExit() {

        hoveringBody = null;        
        hovering = false;

        foreach (Crosshair crosshair in activeCrosshairs)
            crosshair.selectorInactive();
    }

    void shootGrapple() {

        if (!grappleShot) {

            var vec = (hoveringBody.transform.position - currentBody.transform.position).normalized;

            GameObject grap = Instantiate(grapple, currentBody.transform.position + (vec * 16), Quaternion.identity) as GameObject;
            
            grap.transform.LookAt(hoveringBody.transform);
            grap.GetComponent<Rigidbody>().AddForce(grap.transform.forward * grappleShotForce, ForceMode.Impulse);
            
            grappleShot = true;
        }
    }

    public void grappleDeath() {

        grappleShot = false;
    }

    public void bodyHit(GameObject hitBody) {

        switchBody(hitBody.GetComponent<Body>());
        followerHoverExit();        
     
        grappleDeath();        
    }

}