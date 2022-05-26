using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class calibrating_ruler : MonoBehaviour
{
    public TextMeshProUGUI gain;
    public TextMeshProUGUI indicator_distance;
    public camera_movement cam_control;
    public Transform indicator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(cam_control != null)
        {
            gain.text = "Gain: "+ cam_control.gain;
            indicator_distance.text = "z-depth: " + indicator.position.z; 
        }

       if(Input.touchCount > 0)
        {
            if (Input.GetTouch(0).deltaPosition.x > 0) cam_control.gain += 0.00001f;
            else if (Input.GetTouch(0).deltaPosition.x < 0) cam_control.gain -= 0.00001f;

        }
    }
}
