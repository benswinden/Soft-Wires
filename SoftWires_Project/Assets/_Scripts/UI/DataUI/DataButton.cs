using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class DataButton : MonoBehaviour {

    

    public Sprite image;    
    public GameObject particle;

    public bool activated { get; set; }


    public void Toggle() {

        if (activated) {

            Manager.firstImageButtonToggle = true;
            StopCoroutine("makeParticle");

            Manager.worldUI.worldImageHolder.gameObject.SetActive(false);

            activated = false;

        }
        else {

            if (!Manager.firstImageButtonToggle)
                StartCoroutine("makeParticle");

            Manager.worldUI.worldImageHolder.gameObject.SetActive(true);
            Manager.worldUI.worldImageHolder.GetComponentInChildren<SpriteRenderer>().sprite = image;

            activated = true;
        }        
    }

    IEnumerator makeParticle() {

        yield return new WaitForSeconds(2.0f);

        Instantiate(particle,transform.position,Quaternion.identity);

        StartCoroutine("makeParticle");
    }
}