using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Manager : MonoBehaviour {

    public static Rea rea;

    public static Camera currentCamera;

    public static Crosshair currentCrosshair;


    void Awake() {

        Cursor.visible = false;
    }



}