using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class DataDisplayFrame : MonoBehaviour {


    public LineRenderer lineRenderer;

    public float moveSpeed = 1;

    public GameObject holder;

    public DataUI dataUI;

    DataButton targetButton;
    

    public void startAnimation(int imageNum) {
        
        targetButton = dataUI.dataButtons[imageNum];
        targetButton.GetComponent<UIButton>().seek();        

        lineRenderer.SetVertexCount(2);

        lineRenderer.SetPosition(0, lineRenderer.transform.position);        
        lineRenderer.SetPosition(1, targetButton.transform.position);        
        
    }
}