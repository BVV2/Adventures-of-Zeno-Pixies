using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdater : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Slider Slider;
    public Text Text;

    public void TextUpdate()
    {
        Text.text = Slider.value.ToString();

    }
}
