using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void MakeButton()
    {
        Vector3 pos = new Vector3();
        pos.x = 0f + Random.Range(1, 280);
        pos.y = 0f + Random.Range(1, 200);
        pos.z = 0f;
        GameObject Canvas = GameObject.Find("Canvas");
        GameObject Button = GameObject.Find("TestPrefabButton");
        GameObject NewButton = Instantiate(Button, pos, Quaternion.identity) as GameObject;
        NewButton.transform.SetParent(Canvas.transform, false);
        NewButton.gameObject.name += Random.Range(1, 100).ToString();
    }
}
