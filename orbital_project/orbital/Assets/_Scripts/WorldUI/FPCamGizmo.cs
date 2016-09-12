using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class FPCamGizmo : MonoBehaviour {

    public Material matBlack;
    public Material matGreen;

    public List<MeshRenderer> thingsToChangeColor;

    Animation animation;
    bool animating;

    public bool activated { get; set; }


    void Awake() {

        
        animation = GetComponent<Animation>();
    }

    void Start() {
        Manager.rea.fpGizmo = this;
    }


    void Update() {

        if (animating && !animation.isPlaying) {
            animating = false;

            if (activated) {

                Manager.rea.toggleMode();

                foreach (MeshRenderer obj in thingsToChangeColor) {

                    obj.material = matGreen;
                }
            }            

        }
    }

    public void Toggle() {
        
        if (!animating) {
            
            if (activated) {
                
                foreach (MeshRenderer obj in thingsToChangeColor) {

                    obj.material = matBlack;
                }
                
                activated = false;
                animation["Giz_FPCam_Activate"].speed = -1;
                animation["Giz_FPCam_Activate"].time = animation["Giz_FPCam_Activate"].length;
                animation.Play();
                animating = true;

                if (Manager.rea.FPMode)
                    Manager.rea.toggleMode();
            }
            else {

                activated = true;
                animation["Giz_FPCam_Activate"].speed = 1;
                animation.Play();
                animating = true;
            }
        }
    }
}