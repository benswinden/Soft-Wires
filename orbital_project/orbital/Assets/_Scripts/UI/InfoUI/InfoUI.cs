using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class InfoUI : MonoBehaviour {

    public List<UIButton> infoButtons;

    public UIButton talkButton;
    public UIButton connectButton;


    public void buttonToggledOn(UIButton button) {

        if (button == talkButton) {

        }
        else if (button == connectButton) {

        }
    }
}