using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Grapple : MonoBehaviour {

    public float dieAfterDistance;

    public LineRenderer lineRen;


    void Update() {

        lineRen.SetPosition(0, transform.position);
        lineRen.SetPosition(1, Manager.rea.transform.position);

        if (Vector3.Distance(Manager.rea.transform.position, transform.position) > dieAfterDistance) {

            Manager.rea.grappleDeath();
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision coll) {
        
        if (coll.collider.tag.Equals("Follower")) {

            Manager.rea.followerHit(coll.collider.transform.parent.gameObject); 
            Destroy(gameObject);
        }
    }
}