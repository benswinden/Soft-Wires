using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Rea : MonoBehaviour {


    public float TDturnSpeed = 1;
    public float FPturnSpeed = 1;
    public float moveSpeed = 1;

    public float moveDrag = 4;
    public float driftDrag = 1;

    public GameObject mesh;
    public Crosshair crosshair;

    public Camera topDownCamera;
    public Camera FPCamera;
    public Camera RenderCamera;

    public GameObject TDCrosshair;
    public GameObject FPCrosshair;

    public bool FPMode { get; set; }

    public float grappleShotForce = 1000;
    public GameObject grapple;

    public List<GameObject> thingsToMake;    
    public float timeToMake;

    public bool useRenderCamera;


    bool hovering;
    GameObject hoveringFollower;

    bool grappleShot;
    bool making;


    List<GameObject> followerList = new List<GameObject>();

    //values for internal use
    private Quaternion _lookRotation;
    private Vector3 _direction;

    Quaternion rotationTowardsTarget;    
    new Rigidbody rigidbody;


    void Awake() {

        Manager.rea = this;
        Manager.currentCamera = topDownCamera;

        rigidbody = GetComponent<Rigidbody>();
    }

    void Update() {

        Debug.DrawRay(transform.position, transform.forward * 1000, Color.blue);
        Debug.DrawRay(transform.position, transform.up * 1000, Color.green);
        Debug.DrawRay(transform.position, transform.right * 1000, Color.red);

        mesh.transform.Rotate(new Vector3(0, 0.2f, 0));

        if (Input.GetMouseButtonUp(1))
            toggleMode();        

    }

    void FixedUpdate() {

        if (Manager.currentCrosshair.mouseDown && !hovering) {

            rigidbody.drag = moveDrag;

            if (!FPMode) {

                if (!making) 
                    StartCoroutine("make");

                rotationTowardsTarget = Quaternion.AngleAxis(Mathf.Atan2(crosshair.transform.position.z - transform.position.z, crosshair.transform.position.x - transform.position.x) * 180 / Mathf.PI - 90, -transform.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotationTowardsTarget, Time.deltaTime * TDturnSpeed);                
            }
            else {
                var targetVector = (Manager.currentCrosshair.transform.position - transform.position).normalized;
                var rotationTarget = Quaternion.LookRotation(targetVector);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotationTarget, Time.deltaTime * FPturnSpeed);
            }

            rigidbody.AddForce(transform.forward * moveSpeed);

        }
        else if (Input.GetMouseButtonDown(0) && hovering) {


            if (followerList.Contains(hoveringFollower)) {

                hoveringFollower.GetComponent<Follower>().deactivate();
                followerList.Remove(hoveringFollower);

                //Manager.currentCrosshair.selectorInactive();
            }
            else
                shootGrapple();
        }
        else {

            rigidbody.drag = driftDrag;

            StopCoroutine("make");
            making = false;
        }                
    }

    void toggleMode() {


        if (hovering) {

            Manager.currentCrosshair.selectorInactive();
            hovering = false;
            hoveringFollower.GetComponent<Follower>().hoverExit();
        }

        if (FPMode) {

            Manager.currentCamera = topDownCamera;

            topDownCamera.transform.position = transform.position + (transform.up * 1000);
            topDownCamera.transform.rotation = transform.rotation;
            topDownCamera.transform.Rotate(new Vector3(90, 0, 0));

            FPMode = false;
            topDownCamera.enabled = true;
            FPCamera.enabled = false;
            if (useRenderCamera) RenderCamera.enabled = false;

            TDCrosshair.GetComponent<Crosshair>().activate();
            TDCrosshair.transform.rotation = transform.rotation;
            FPCrosshair.GetComponent<Crosshair>().deactivate();
        }
        else {

            Manager.currentCamera = FPCamera;

            FPMode = true;
            topDownCamera.enabled = false;
            FPCamera.enabled = true;
            if (useRenderCamera) RenderCamera.enabled = true;

            TDCrosshair.GetComponent<Crosshair>().deactivate();
            FPCrosshair.GetComponent<Crosshair>().activate();

        }
    }

    public void followerHover(Follower follower) {

        hoveringFollower = follower.gameObject;
        Manager.currentCrosshair.selectorActive(follower.gameObject);        
        hovering = true;
    }

    public void followerHoverExit() {

        Manager.currentCrosshair.selectorInactive();
        hovering = false;
    }

    void shootGrapple() {

        if (!grappleShot) {

            GameObject grap = Instantiate(grapple, transform.position, Quaternion.identity) as GameObject;
            
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
        Manager.currentCrosshair.selectorInactive();
        hoveringFollower.GetComponentInParent<Follower>().activate();        
        grappleDeath();        
    }

    IEnumerator make() {

        if (thingsToMake.Count > 0) {
            making = true;

            yield return new WaitForSeconds(timeToMake);

            GameObject obj = Instantiate(thingsToMake[Random.Range(0, thingsToMake.Count)], transform.position, Quaternion.identity) as GameObject;
            obj.GetComponent<Rigidbody>().AddForce(-transform.forward * Random.Range(100, 300), ForceMode.Impulse);

            StartCoroutine("make");
        }
    }
}