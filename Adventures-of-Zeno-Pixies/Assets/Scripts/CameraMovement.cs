using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    // Use this for initialization
    float speed = 7.0f;
    int boundary = 1;
    int width;
    int height;

    public bool dragControl = false;
    GameObject Cam;

    void Start () {
        width = Screen.width;
        height = Screen.height;
        Cam = GameObject.Find("Main Camera");
    }
	
	// Update is called once per frame
	void Update () {
        if (dragControl)
        {

            if (Input.GetMouseButton(1))
            {
                if (Input.GetAxis("Mouse X") > 0)
                {
                    Cam.transform.position += new Vector3(Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed,
                                               0.0f, Input.GetAxisRaw("Mouse Y") * Time.deltaTime * speed);
                }

                else if (Input.GetAxis("Mouse X") < 0)
                {
                    Cam.transform.position += new Vector3(Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed,
                                               0.0f, Input.GetAxisRaw("Mouse Y") * Time.deltaTime * speed);
                }
            }
        }
        else
        {

            if (Input.mousePosition.x > width - boundary)
            {
                if(Cam.transform.position.x < 14f)
                    {
                    Cam.transform.position += new Vector3(Time.deltaTime * speed,
                                               0.0f, 0.0f);
                    }
            }

            if (Input.mousePosition.x < 0 + boundary)
            {
                if (Cam.transform.position.x > -14f)
                {
                    Cam.transform.position -= new Vector3(Time.deltaTime * speed,
                                           0.0f, 0.0f);
                }
            }

            if (Input.mousePosition.y > height - boundary)
            {
                if (Cam.transform.position.y < 14f)
                {
                    Cam.transform.position += new Vector3(0.0f,
                                            Time.deltaTime * speed, 0.0f);
                }
            }

            if (Input.mousePosition.y < 0 + boundary)
            {
                if (Cam.transform.position.y > -14f)
                {
                    Cam.transform.position -= new Vector3(0.0f,
                                            Time.deltaTime * speed, 0.0f);
                }
            }
        }
    }




}
