using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class RandomMesh : MonoBehaviour {

    public List<Mesh> meshList;

    void Awake() {
        
        GetComponentInChildren <MeshFilter>().mesh = meshList[Random.Range(0, meshList.Count)];        
    }

}