using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IronPython;

public class ScriptLoader2 : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

    public void Runner()
    {
        ScriptLoader Loader = new ScriptLoader();
        Loader.Runner("Assets/Python/test.py");

    }

    public void Buttoner()
    {
        ScriptLoader Loader = new ScriptLoader();
        Loader.Runner("Assets/Python/test2.py");
    }
}
