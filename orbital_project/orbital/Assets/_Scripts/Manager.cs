using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Manager : MonoBehaviour {

    
    public static Rea rea;

    public static Camera currentCamera;

    public static Crosshair currentCrosshair;

    public static Manager manager;

    public static List<GameObject> pointsOfInterest = new List<GameObject>();

    bool waitting;

    bool startReporting;

    void Awake() {


        Manager.manager = this;
        Cursor.visible = false;
    }

    int count = 0;
    List<float> fpsList = new List<float>();

    void Start() {

        StartCoroutine("waitAtStart");
    }

    public void reportFPS(float fps) {
        if (startReporting) {
            if (!waitting && fps < 45) {

                waitting = true;
                
            }

            if (fpsList.Count < 60) {
                fpsList.Add(fps);
            }
            else
                fpsList[count] = fps;

            count++;

            if (fpsList.Count >= 60 && count == 60) {

                var sum = 0.0f;

                foreach (float f in fpsList) {
                    sum += f;
                }

                sum /= 60;
                //print(sum);
                count = 0;
            }
        }
    }

    IEnumerator waitAtStart() {

        yield return new WaitForSeconds(2);

        startReporting = true;
    }
}