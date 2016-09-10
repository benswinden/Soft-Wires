using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.IO;




public class Follower : MonoBehaviour {


    public float turnSpeed = 1;    
    public float maxMoveSpeed = 1;

    public float moveDrag = 4;
    public float driftDrag = 1;

    public GameObject mesh;
    
    public List<GameObject> thingsToMake;    
    public float timeToMake;

    public bool randomTargets;

    public LineRenderer lineRenderer;

    public TextMeshPro nameText;    
    public string followerName;



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

        targetPosition = Vector3.zero;
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start() {

        followerName = (Manager.nameList[Random.Range(0, Manager.nameList.Count)]);

        moveSpeed = maxMoveSpeed;        
    }

    void Update() {

        mesh.transform.Rotate(new Vector3(0, 0.2f, 0));       
    }

    void FixedUpdate() {


        if (hoverActive) {

            var mousePos = Input.mousePosition;
            mousePos.z = Vector3.Distance(transform.position, Manager.currentCamera.transform.position);

            mousePos = Manager.currentCamera.ScreenToWorldPoint(mousePos);

            if (Vector3.Distance(transform.position, mousePos) > 150) {

                hoverExit();
            }
        }

        if (target != null ) {

            var distance = Vector3.Distance(transform.position, target.transform.position);

            if (target != null && distance > 50) {

                thrusting = true;
            }

            if (distance < maxMoveSpeed / 10) {

                moveSpeed = distance * 10;
            }


            if (thrusting) {
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
        }
    }

    void OnMouseEnter() {

        displayName();
        hoverActive = true;
        Manager.rea.followerHover(this);
    }

    public void hoverExit() {

        clearName();
        hoverActive = false;
        Manager.rea.followerHoverExit();
    }

    public void activate() {

        lineRenderer.SetVertexCount(2);
        target = Manager.rea.gameObject;
    }

    public void deactivate() {

        lineRenderer.SetVertexCount(0);
        target = null;
    }

    void displayName() {

        nameText.SetText(followerName);
    }

    void clearName() {

        nameText.SetText("");
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