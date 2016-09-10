using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Manager : MonoBehaviour {

    
    public static Rea rea;

    public static Camera currentCamera;

    public static Crosshair currentCrosshair;

    public static Manager manager;

    public static List<GameObject> pointsOfInterest = new List<GameObject>();

    public static List<string> nameList = new List<string>();

    bool waitting;
    bool startReporting;

    public TextAsset textFile;


    void Awake() {
        
        makeNameList();

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

    void makeNameList() {

        string textAsString = textFile.text;
        string[] fLines = Regex.Split ( textAsString, "\n|\r|\r\n" );
 
        for ( int i=0; i < fLines.Length; i++ ) {
 
            string word = fLines[i];

            if (!word.Equals(""))
                Manager.nameList.Add(word);
        }
    }
    
}