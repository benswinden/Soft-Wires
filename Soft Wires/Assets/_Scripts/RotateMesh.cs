using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class RotateMesh : MonoBehaviour {

    public GameObject mesh;

    void Update() {

        mesh.transform.Rotate(new Vector3(0, 0.2f, 0));
    }

}