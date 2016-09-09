using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


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


    float moveSpeed;

    bool making;

    bool thrusting;
    GameObject target;
    Vector3 targetPosition;
    
    Quaternion _lookRotation;
    Vector3 _direction;

    Quaternion rotationTowardsTarget;    
    new Rigidbody rigidbody;


    void Awake() {

        targetPosition = Vector3.zero;
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start() {

        moveSpeed = maxMoveSpeed;

        target = Manager.rea.gameObject;
    }

    void Update() {

        mesh.transform.Rotate(new Vector3(0, 0.2f, 0));       
    }

    void FixedUpdate() {

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

        if (target != null) {

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, target.transform.position);
        }
        else if (targetPosition != Vector3.zero) {
            
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, target.transform.position);
        }
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