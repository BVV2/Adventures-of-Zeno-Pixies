using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorGlitter : MonoBehaviour {

    public static Color glitterColor_;
    public static Color defaultColor_;
    public static float glitterSpeed_;
    public static float defaultSpeed_;
    public ParticleSystem ps_glitter_;
    public ParticleSystem ps_glow_;

	// Use this for initialization
	void Start () {
        glitterColor_ = ps_glitter_.startColor;
        defaultColor_ = glitterColor_;
        glitterSpeed_ = ps_glitter_.startSpeed;
        defaultSpeed_ = glitterSpeed_;
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = (Camera.main.ScreenToWorldPoint(Input.mousePosition));
        ps_glitter_.startColor = glitterColor_;
        ps_glow_.startColor = glitterColor_;
        ps_glitter_.startSpeed = glitterSpeed_;
        ps_glitter_.emissionRate = glitterSpeed_;
        Mathf.Clamp(ps_glitter_.startSpeed, 0f, 10f);
        Mathf.Clamp(ps_glitter_.emissionRate, 0f, 10f);

    }
}
