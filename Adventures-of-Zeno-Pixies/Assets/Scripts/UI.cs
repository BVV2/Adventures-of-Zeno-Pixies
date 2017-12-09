using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour {

    public GameObject winPanel_;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowWin()
    {
        winPanel_.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
