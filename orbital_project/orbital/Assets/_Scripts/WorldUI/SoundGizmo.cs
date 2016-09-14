using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class SoundGizmo : MonoBehaviour {

    public bool startActivated;

    public Material matBlack;
    public Material matGreen;

    public List<MeshRenderer> thingsToChangeColor;

    Animation _animation;    

    public bool activated { get; set; }

    string animationName;

    void Awake() {

        _animation = GetComponent<Animation>();
    }

    void Start() {

        if (_animation) {
            animationName = _animation.clip.name;
            _animation[animationName].speed = 0;
        }

        if (startActivated)
            Toggle();
    }

    void Update() {

    }

    public void Toggle() {
        
        if (activated) {
                
            foreach (MeshRenderer obj in thingsToChangeColor) {

                obj.material = matBlack;
            }
           
            activated = false;
            
            if (_animation)
                _animation[animationName].speed = 0;            
        }
        else {

            foreach (MeshRenderer obj in thingsToChangeColor) {

                obj.material = matGreen;
            }

            activated = true;

            if (_animation)
                _animation[animationName].speed = 1;            
        }        
    }
}