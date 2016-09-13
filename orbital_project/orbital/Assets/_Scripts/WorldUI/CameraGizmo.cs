using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class CameraGizmo : MonoBehaviour {

    public Material matBlack;
    public Material matGreen;

    public GameObject cameraDisplay;

    public List<MeshRenderer> thingsToChangeColor;

    Animation animation;
    bool animating;

    public bool activated { get; set; }


    void Awake() {

        
        animation = GetComponent<Animation>();
    }

    void Start() {
        Manager.user.fpGizmo = this;
    }


    void Update() {

        // Waiting for toggle animation to finish
        if (animating && !animation.isPlaying) {

            animating = false;

            if (activated) {

                Manager.user.toggleMode();
                cameraDisplay.SetActive(true);


                foreach (MeshRenderer obj in thingsToChangeColor) {

                    obj.material = matGreen;
                }
            }            

        }
    }

    public void Toggle() {
        
            // TURN OFF
            if (activated) {
                
                foreach (MeshRenderer obj in thingsToChangeColor) {

                    obj.material = matBlack;
                }
                
                activated = false;

                animation["Giz_FPCam_Activate"].speed = -1;
                if (!animation.isPlaying) {
                    animation["Giz_FPCam_Activate"].time = animation["Giz_FPCam_Activate"].length;
                    animation.Play();
                }
                animating = true;

                cameraDisplay.SetActive(false);

                if (Manager.user.FPMode)
                    Manager.user.toggleMode();
            }
            // TURN ON
            else {

                activated = true;
                animation["Giz_FPCam_Activate"].speed = 1;
                if (!animation.isPlaying) animation.Play();
                animating = true;
            }        
    }
}