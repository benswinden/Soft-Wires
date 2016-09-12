using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class WorldImageHolder : MonoBehaviour {


    public LineRenderer lineRenderer;

    public float moveSpeed = 1;

    public GameObject holder;

    public ImageUI imageUI;

    ImageButton targetButton;

    bool activated;

    void Update() {

        if (activated) {

        }
    }

    public void startAnimation(int imageNum) {

        targetButton = imageUI.imageButtons[imageNum];
        targetButton.create();
        targetButton.Toggle();

        lineRenderer.SetVertexCount(2);

        lineRenderer.SetPosition(0, lineRenderer.transform.position);        
        lineRenderer.SetPosition(1, targetButton.transform.position);        
        
    }
}