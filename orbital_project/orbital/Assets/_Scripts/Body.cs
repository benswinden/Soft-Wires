using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.IO;




public class Body : MonoBehaviour {

    [Header("Variables")]
    
    public bool canMove = true;
    public bool moveSlowdown = true;
        
    public float turnSpeed = 1;    
    public float maxMoveSpeed = 1;

    public float moveDrag = 4;
    public float driftDrag = 1;

    public bool spinMesh = true;

    [Header("Name")]
    public bool showName = true;
    public GameObject nameHolder;
    public TextMeshPro nameText;    
    public string followerName;

    public string description;

    [Header("Containers")]    
    public GameObject meshToSpin;
    public LineRenderer lineRenderer;    

    [Space]

    public bool debug;



    float moveSpeed;    
    bool hoverActive;
    bool thrusting;
    GameObject target;
    
    Quaternion _lookRotation;
    Vector3 _direction;

    Quaternion rotationTowardsTarget;    
    Rigidbody rigidbodyComponent;


    public bool activated { get; set; }


    void Awake() {

        lineRenderer.SetVertexCount(0);
        rigidbodyComponent = GetComponent<Rigidbody>();
    }
    
    void Start() {

        // Get a name
        var num = Random.Range(0, Manager.nameList.Count);
        followerName = (Manager.nameList[num]);
        Manager.nameList.RemoveAt(num);

        // Get a description


        moveSpeed = maxMoveSpeed;        
    }

    void Update() {

        if (debug)
            Debug.Log("Target: " + target + "    Hover: " + hoverActive);

        if (spinMesh)
            meshToSpin.transform.Rotate(new Vector3(0, 0.2f, 0));       
    }

    void FixedUpdate() {

        if (!activated) {

            if (hoverActive) {


                if (showName) {
                    var targetVector = (nameHolder.transform.position - Manager.user.currentBody.transform.position).normalized;
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

            if (target != null) {

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
                    rigidbodyComponent.drag = moveDrag;


                    var targetVector = (target.transform.position - transform.position).normalized;
                    var rotationTarget = Quaternion.LookRotation(targetVector);

                    transform.rotation = Quaternion.Slerp(transform.rotation, rotationTarget, Time.deltaTime * turnSpeed);

                    rigidbodyComponent.AddForce(transform.forward * moveSpeed);
                }
                else {

                    rigidbodyComponent.drag = driftDrag;
                }

                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, target.transform.position);


            }
        }
    }

    Vector3 startMousePosition;

    void OnMouseEnter() {
        
        if (Vector3.Distance(Manager.user.currentBody.transform.position, transform.position) > 50) {

            var mousePos = Input.mousePosition;
            mousePos.z = Vector3.Distance(transform.position, Manager.currentCamera.transform.position);
            mousePos = Manager.currentCamera.ScreenToWorldPoint(mousePos);
            startMousePosition = mousePos;

            if (showName) displayName();
            hoverActive = true;
            Manager.user.bodyHover(this);
        }
    }

    public void hoverExit() {

        clearName();
        hoverActive = false;
        Manager.user.bodyHoverExit();
    }

    public void activate() {

        activated = true;
        clearName();
        //bool spaceAvailable = true;

        //if (slot)
        //    Manager.worldUI.addGizmo(gizmo, slot);
        //else
        //    spaceAvailable = Manager.worldUI.addGizmo(gizmo);

        //if (spaceAvailable) {

        //    if (GetComponent<PointOfInterest>())
        //        Manager.pointsOfInterest.Remove(gameObject);

        //    lineRenderer.SetVertexCount(2);
        //    target = Manager.user.body;
        //}
        
    }

    public void deactivate() {

        activated = false;
        displayName();

        //if (GetComponent<PointOfInterest>())
        //    Manager.pointsOfInterest.Add(gameObject);

        //lineRenderer.SetVertexCount(0);
        //target = null;


        //if (slot)
        //    Manager.worldUI.removeGizmo(gizmo, slot);
        //else
        //    Manager.worldUI.removeGizmo(gizmo);



        //gizmo.transform.parent = transform;
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
}