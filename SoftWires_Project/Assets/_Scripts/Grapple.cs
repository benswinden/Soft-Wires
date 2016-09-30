using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Grapple : MonoBehaviour {

    public float dieAfterDistance;

    public LineRenderer lineRen;


    void Update() {

        lineRen.SetPosition(0, transform.position);
        lineRen.SetPosition(1, Manager.user.currentBody.transform.position);

        if (Vector3.Distance(Manager.user.currentBody.transform.position, transform.position) > dieAfterDistance) {

            Manager.user.grappleDeath();
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision coll) {

        if (coll.collider.tag.Equals("Body") && !coll.collider.GetComponentInParent<Body>().activated) {

            Manager.user.bodyHit(coll.collider.transform.parent.gameObject); 
            Destroy(gameObject);
        }
    }
}