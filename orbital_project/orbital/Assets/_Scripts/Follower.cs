using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.IO;




public class Follower : MonoBehaviour {

    public UISlot slot;

    public bool moveSlowdown = true;

    public bool canMove = true;

    public bool showName = true;

    public float turnSpeed = 1;    
    public float maxMoveSpeed = 1;

    public float moveDrag = 4;
    public float driftDrag = 1;

    public float distanceToBreakTether = 200;

    public bool spinMesh = true;
    public GameObject mesh;
    
    public List<GameObject> thingsToMake;    
    public float timeToMake;

    public bool randomTargets;

    public LineRenderer lineRenderer;

    public GameObject nameHolder;
    public TextMeshPro nameText;    
    public string followerName;

    public bool corrupt;
    public Material matLineRed;

    public Gizmo gizmo;

    public bool debug;

    float moveSpeed;

    bool making;

    bool hoverActive;

    bool thrusting;
    GameObject target;
    Vector3 targetPosition;
    
    Quaternion _lookRotation;
    Vector3 _direction;

    Quaternion rotationTowardsTarget;    
    new Rigidbody rigidbody;


    void Awake() {

        lineRenderer.SetVertexCount(0);
        if (corrupt)
            lineRenderer.material = matLineRed;

        targetPosition = Vector3.zero;
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start() {

        followerName = (Manager.nameList[Random.Range(0, Manager.nameList.Count)]);

        moveSpeed = maxMoveSpeed;        
    }

    void Update() {

        if (debug)
            Debug.Log("Target: " + target + "    Hover: " + hoverActive);

        if (spinMesh)
            mesh.transform.Rotate(new Vector3(0, 0.2f, 0));       
    }

    void FixedUpdate() {


        if (hoverActive) {


            if (showName) {
                var targetVector = (nameHolder.transform.position - Manager.rea.transform.position).normalized;
                var rotationTarget = Quaternion.LookRotation(-targetVector);
                nameHolder.transform.rotation = Quaternion.Slerp(nameHolder.transform.rotation, rotationTarget, Time.deltaTime * turnSpeed);
            }


            var mousePos = Input.mousePosition;
            mousePos.z = Vector3.Distance(transform.position, Manager.currentCamera.transform.position);
            mousePos = Manager.currentCamera.ScreenToWorldPoint(mousePos);

            if (Vector3.Distance(startMousePosition, mousePos) > 150) {

                hoverExit();
            }
        }

        if (target != null ) {

            var distance = Vector3.Distance(transform.position, target.transform.position);

            if (moveSlowdown) {
                if (target != null && distance > 50) {

                    thrusting = true;
                }

                if (distance < maxMoveSpeed / 10) {

                    moveSpeed = distance * 10;
                }

            }

            if (canMove && thrusting) {
                rigidbody.drag = moveDrag;
        
                if (!making) 
                    StartCoroutine("make");

            
                var targetVector = (target.transform.position - transform.position).normalized;
                var rotationTarget = Quaternion.LookRotation(targetVector);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotationTarget, Time.deltaTime * turnSpeed);
            
                rigidbody.AddForce(transform.forward * moveSpeed);
            }        
            else {

                rigidbody.drag = driftDrag;

                StopCoroutine("make");
                making = false;
            }

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, target.transform.position);


            if (Vector3.Distance(transform.position, target.transform.position) > distanceToBreakTether) {

                deactivate();
            }
        }
    }

    Vector3 startMousePosition;

    void OnMouseEnter() {

        if (Vector3.Distance(Manager.rea.transform.position, transform.position) > 50) {

            var mousePos = Input.mousePosition;
            mousePos.z = Vector3.Distance(transform.position, Manager.currentCamera.transform.position);
            mousePos = Manager.currentCamera.ScreenToWorldPoint(mousePos);
            startMousePosition = mousePos;

            if (showName) displayName();
            hoverActive = true;
            Manager.rea.followerHover(this);
        }
    }

    public void hoverExit() {

        clearName();
        hoverActive = false;
        Manager.rea.followerHoverExit();
    }

    public void activate() {

        bool spaceAvailable = true;

        if (slot)
            Manager.worldUI.addGizmo(gizmo, slot);
        else
            spaceAvailable = Manager.worldUI.addGizmo(gizmo);

        if (spaceAvailable) {

            if (GetComponent<PointOfInterest>())
                Manager.pointsOfInterest.Remove(gameObject);

            lineRenderer.SetVertexCount(2);
            target = Manager.rea.gameObject;
        }
        
    }

    public void deactivate() {

        if (GetComponent<PointOfInterest>())
            Manager.pointsOfInterest.Add(gameObject);

        lineRenderer.SetVertexCount(0);
        target = null;


        if (slot)
            Manager.worldUI.removeGizmo(gizmo, slot);
        else
            Manager.worldUI.removeGizmo(gizmo);
        
        
        
        gizmo.transform.parent = transform;
    }

    void displayName() {

        nameText.SetText(followerName);
    }

    void clearName() {

        nameText.SetText("");
    }

    public void kill() {

        if (GetComponent<PointOfInterest>())
            Manager.pointsOfInterest.Remove(gameObject);

        Destroy(gameObject);
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