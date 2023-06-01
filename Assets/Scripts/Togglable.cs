using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Togglable : MonoBehaviour { // best code fr
    public Text toggler;

    [SerializeField] private bool toggled = false;

    public void Awake() {
        toggled = PlayerPrefs.GetInt("HardMode", 0) == 1;
        
        if (toggled) {
            toggler.text = "x";
        } else {
            toggler.text = "";
        }

        PlayerPrefs.SetInt("HardMode", Convert.ToInt32(toggled));
    }

    public void toggle() {
        toggled = !toggled;
        if (toggled) {
            toggler.text = "x";
        } else {
            toggler.text = "";
        }
        PlayerPrefs.SetInt("HardMode", Convert.ToInt32(toggled));
    }
}
