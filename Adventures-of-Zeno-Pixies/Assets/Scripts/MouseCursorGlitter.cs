using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorGlitter : MonoBehaviour {

    public static Color glitterColor_;
    public static Color defaultColor_;
    public static float glitterSpeed_;
    public static float defaultSpeed_;
    private ParticleSystem particleSystem_;

	// Use this for initialization
	void Start () {

        particleSystem_ = GetComponent<ParticleSystem>();
        glitterColor_ = particleSystem_.startColor;
        defaultColor_ = glitterColor_;
        glitterSpeed_ = particleSystem_.startSpeed;
        defaultSpeed_ = glitterSpeed_;
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = (Camera.main.ScreenToWorldPoint(Input.mousePosition));
        particleSystem_.startColor = glitterColor_;
        particleSystem_.startSpeed = glitterSpeed_;
        particleSystem_.emissionRate = glitterSpeed_;
        Mathf.Clamp(particleSystem_.startSpeed, 0f, 10f);
        Mathf.Clamp(particleSystem_.emissionRate, 0f, 10f);

    }
}
