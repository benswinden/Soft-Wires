using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Grid : MonoBehaviour {

    public GameObject dot;

    public float seperation;

    public int maxWidth;
    public int maxHeight;
    public int maxDepth;

    public float chanceToDestroy;

    void Awake() {

        var depth = Random.Range(0, maxDepth);
        var height = Random.Range(0, maxHeight);
        var width = Random.Range(0, maxWidth);

        for (int k = 0; k <= depth; k++) {
            for (int i = 0; i <= height; i++) {
                for (int j = 0; j <= width; j++) {

                    GameObject obj;
                    if (Random.value > chanceToDestroy) {
                        obj = Instantiate(dot, new Vector3(transform.position.x + (seperation * j), transform.position.y + (seperation * i), transform.position.z + (seperation * k)), Quaternion.identity) as GameObject;
                        obj.transform.parent = transform;
                    }

                }
            }
        }


    }
}
