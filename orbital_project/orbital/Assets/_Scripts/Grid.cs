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

    public bool dolines;

    public LineRenderer lineRenderer;
    

    List<Vector3> positionsList = new List<Vector3>();



    void Start() {
  
        StartCoroutine("generateChildren");        
    }

    IEnumerator generateChildren() {

        var depth = Random.Range(0, maxDepth);
        var height = Random.Range(0, maxHeight);
        var width = Random.Range(0, maxWidth);

        List<Vector3> currentPositionsList = new List<Vector3>();

        for (int k = 0; k <= depth; k++) {

            for (int i = 0; i <= height; i++) {
                for (int j = 0; j <= width; j++) {

                    GameObject obj;
                    if (Random.value > chanceToDestroy) {

                        yield return new WaitForSeconds(Random.Range(0, 2));

                        obj = Instantiate(dot, new Vector3(transform.position.x + (seperation * j), transform.position.y + (seperation * i), transform.position.z + (seperation * k)), Quaternion.identity) as GameObject;
                        obj.transform.parent = transform;
                        positionsList.Add(obj.transform.position);

                        if (dolines) {
                            lineRenderer.SetVertexCount(positionsList.Count);
                            lineRenderer.SetPosition(positionsList.Count - 1, positionsList[positionsList.Count - 1]);
                        }
                    }

                }
            }
        }
        
        StartCoroutine("checkChildren");
    }

    IEnumerator checkChildren() {

        int count = 0;
        foreach (Transform child in transform) {

            count++;
        }

        if (count == 0) {

            StartCoroutine("generateChildren");
            yield break;
        }

        yield return new WaitForSeconds(3.0f);

        StartCoroutine("checkChildren");
    }

    GameObject createLineRenderer() {

        GameObject lineRenderer = new GameObject();
        lineRenderer.AddComponent<LineRenderer>();
        lineRenderer.transform.parent = transform;

        lineRenderer.GetComponent<LineRenderer>().SetWidth(0.25f, 0.25f);

        return lineRenderer;
    }
}
