using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class ImageButton : MonoBehaviour {

    public ImageUI imageUI;
    public Material matBlack;
    public Material matGreen;

    public Sprite image;

    public bool activated { get; set; }

    public List<MeshRenderer> thingsToChangeColor;

    public GameObject particle;

    public List<GameObject> thingsToDisable;


    Vector3 startMousePosition;
    bool hoverActive;
    bool waiting = true;
    bool broughtToLife;



    void Awake() {


        foreach (GameObject obj in thingsToDisable) {

            obj.SetActive(false);
        }
    }


    void Update() {


        if (broughtToLife && hoverActive) {


            var mousePos = Input.mousePosition;
            mousePos.z = Vector3.Distance(transform.position, Manager.worldUICamera.transform.position);
            mousePos = Manager.worldUICamera.ScreenToWorldPoint(mousePos);

            if (Vector3.Distance(startMousePosition, mousePos) > 30) {

                hoverExit();
            }

            if (Input.GetMouseButtonDown(0)) {

                Toggle();
            }
        }
    }


    void OnMouseEnter() {

        if (broughtToLife) {
            var mousePos = Input.mousePosition;
            mousePos.z = Vector3.Distance(transform.position, Manager.worldUICamera.transform.position);
            mousePos = Manager.worldUICamera.ScreenToWorldPoint(mousePos);

            startMousePosition = mousePos;

            imageUI.selectionHover(gameObject);

            hoverActive = true;
            Manager.rea.gizmoHover(gameObject);
        }
    }

    public void hoverExit() {

        if (broughtToLife) {

            imageUI.selectionHoverExit();

            hoverActive = false;
            Manager.rea.gizmoHoverExit();
        }
    }

    public void Toggle() {

        if (activated) {

            Manager.firstImageButtonToggle = true;
            StopCoroutine("makeParticle");

            foreach (MeshRenderer obj in thingsToChangeColor) {

                obj.material = matBlack;
            }

            Manager.worldUI.worldImageHolder.gameObject.SetActive(false);

            activated = false;

        }
        else {

            if (!Manager.firstImageButtonToggle)
                StartCoroutine("makeParticle");

            foreach (MeshRenderer obj in thingsToChangeColor) {

                obj.material = matGreen;
            }

            Manager.worldUI.worldImageHolder.gameObject.SetActive(true);
            Manager.worldUI.worldImageHolder.GetComponentInChildren<SpriteRenderer>().sprite = image;

            activated = true;

        }
    }

    public void create() {

        broughtToLife = true;

        foreach (GameObject obj in thingsToDisable) {

            obj.SetActive(true);
        }

        foreach (MeshRenderer obj in thingsToChangeColor) {
         
            obj.material = matBlack;
        }
    }

    IEnumerator makeParticle() {

        yield return new WaitForSeconds(2.0f);

        Instantiate(particle,transform.position,Quaternion.identity);

        StartCoroutine("makeParticle");
    }
}