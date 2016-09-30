using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class RandomNumbers : MonoBehaviour {

    public float delay;

    TextMeshPro textMesh;

    void Awake() {

        textMesh = GetComponent<TextMeshPro>();
    }

    void Start() {

        StartCoroutine("blah");
    }

    IEnumerator blah() {

        yield return new WaitForSeconds(delay);


        var num = Random.Range(100.0f, 400.0f);
        textMesh.SetText("" + Mathf.Round(num * 1000.0f) / 1000.0f);

        StartCoroutine("blah");
    }
}