using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Tuning : MonoBehaviour {


    public Material matBlack;
    public Material matGreen;

    public float distanceFromPlayer = 500;

    public LineRenderer lineRenderer;
    public GameObject sphere;

    bool activated;
    GameObject closest;


    public List<MeshRenderer> thingsToChangeColor;


    void Start() {

        StartCoroutine("checkForPoints");
    }

    void Update() {


        if (activated && closest != null) {

            Vector3 vec = (closest.transform.position - Manager.rea.transform.position).normalized;
            Vector3 target = Manager.rea.transform.position + (vec * distanceFromPlayer);

            //Debug.DrawLine(Manager.rea.transform.position, target, Color.cyan);

            sphere.transform.position = target;

            lineRenderer.SetPosition(0, sphere.transform.position);
            lineRenderer.SetPosition(1, Manager.rea.transform.position);

        }
        else if (activated && closest == null) {

            Toggle();
        }

    }

    public void Toggle() {

        if (activated) {

            foreach (MeshRenderer obj in thingsToChangeColor) {

                obj.material = matBlack;
            }

            activated = false;
            lineRenderer.SetVertexCount(0);
            sphere.SetActive(false);
        }
        else {

            foreach (MeshRenderer obj in thingsToChangeColor) {
                
                obj.material = matGreen;
            }

            activated = true;

            lineRenderer.SetVertexCount(2);
            lineRenderer.SetPosition(0, sphere.transform.position);
            lineRenderer.SetPosition(1, Manager.rea.transform.position);

            sphere.SetActive(true);
        }

    }


    IEnumerator checkForPoints() {

        yield return new WaitForSeconds(2.0f);

        if (Manager.pointsOfInterest.Count > 0) {

            closest = Manager.pointsOfInterest[0];

            for (int i = 1; i < Manager.pointsOfInterest.Count; i++) {

                if (Vector3.Distance(transform.position, Manager.pointsOfInterest[i].transform.position) < Vector3.Distance(transform.position, closest.transform.position))
                    closest = Manager.pointsOfInterest[i];
            }
        }
        else {
            closest = null;
        }

        StartCoroutine("checkForPoints");
    }
}