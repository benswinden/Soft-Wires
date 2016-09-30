using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class CameraGizmo : MonoBehaviour {

    public Material matBlack;
    public Material matGreen;

    public GameObject cameraDisplay;

    public List<MeshRenderer> thingsToChangeColor;

    Animation _animation;
    bool animating;

    public bool activated { get; set; }


    void Awake() {

        
        _animation = GetComponent<Animation>();
    }

    void Start() {
        Manager.user.fpGizmo = this;
    }


    void Update() {

        // Waiting for toggle animation to finish
        if (animating && !_animation.isPlaying) {

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

                _animation["Giz_FPCam_Activate"].speed = -1;
                if (!_animation.isPlaying) {
                    _animation["Giz_FPCam_Activate"].time = _animation["Giz_FPCam_Activate"].length;
                    _animation.Play();
                }
                animating = true;

                cameraDisplay.SetActive(false);

                if (Manager.user.FPMode)
                    Manager.user.toggleMode();
            }
            // TURN ON
            else {

                activated = true;
                _animation["Giz_FPCam_Activate"].speed = 1;
                if (!_animation.isPlaying) _animation.Play();
                animating = true;
            }        
    }
}