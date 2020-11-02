using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public Camera cam1;
    public Camera cam2;
    public Camera cam3;
    public Camera cam4;

    private int active = 1;

    void Start()
    {
        cam2.enabled = false;
        cam3.enabled = false;
        cam4.enabled = false;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Switch(false);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Switch(true);
        }
    }

    void Switch(bool left)
    {
        switch (active)
        {
            case 1:
                cam1.enabled = false;
                if (left)
                {
                    EnableCamera(cam4, 4);
                }
                else
                {
                    EnableCamera(cam2, 2);
                }
                break;
            case 2:
                cam2.enabled = false;
                if (left)
                {
                    EnableCamera(cam1, 1);
                }
                else
                {
                    EnableCamera(cam3, 3);
                }
                break;
            case 3:
                cam3.enabled = false;
                if (left)
                {
                    EnableCamera(cam2, 2);
                }
                else
                {
                    EnableCamera(cam4, 4);
                }
                break;
            case 4:
                cam4.enabled = false;
                if (left)
                {
                    EnableCamera(cam3, 3);
                }
                else
                {
                    EnableCamera(cam1, 1);
                }
                break;
        }

        void EnableCamera(Camera camera, int new_active)
        {
            camera.enabled = true;
            active = new_active;
        }
    }
}
