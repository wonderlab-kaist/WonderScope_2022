using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_controller : MonoBehaviour
{
    private RawImage image; //header image
    private float rotationValue; //scienscope rotaion value -180~180
    public float unitValue;
    public float imageUnitCnt;

    float reciprocal;
    void Awake()
    {
        image = GetComponent<RawImage>();
        reciprocal = 1 / unitValue / imageUnitCnt;
    }

    private void Update()
    {
        rotationValue = GameObject.Find("Moon_scene_control").GetComponent<Camera_Movement_Moon>().direction;
        SetUV(rotationValue);
    }

    public void SetUV(float value)
    {
        float remainder = value % unitValue;
        image.uvRect = new Rect(remainder*reciprocal, 0, 1, 1);
    }

}
