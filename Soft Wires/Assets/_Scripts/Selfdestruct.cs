using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Selfdestruct : MonoBehaviour {

    public float maxTime;

    void Start() {

        Destroy(gameObject, Random.Range(0, maxTime));
    }

}