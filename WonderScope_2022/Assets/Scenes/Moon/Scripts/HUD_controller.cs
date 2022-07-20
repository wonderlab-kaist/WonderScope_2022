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

    public RectTransform[] texts; //N, NE, E, SE, S, SW, W, NW
    public float padding;

    float reciprocal; //for image
    float reciprocalTxt; //for text


    void Awake()
    {
        image = GetComponent<RawImage>();
        reciprocal = 1 / unitValue / imageUnitCnt;
        reciprocalTxt = (1.0f / 360.0f) * padding * texts.Length;
    }

    private void Update()
    {
        rotationValue = GameObject.Find("Moon_scene_control").GetComponent<Camera_Movement_Moon>().direction;
        SetUV(rotationValue);

        //TXT
        int index = 0;
        float passBackThreshold = padding * (texts.Length * 0.5f);
        foreach (RectTransform text in texts)
        {
            float localPosX = index * padding - rotationValue * reciprocalTxt;

            // Adjust Text position
            if (localPosX > passBackThreshold)
            {
                localPosX -= padding * (texts.Length);
            }
            else if (localPosX < -passBackThreshold)
            {
                localPosX += padding * (texts.Length);
            }
            text.anchoredPosition = new Vector2(localPosX, 655);

            index++;
        }


    }

    public void SetUV(float value) //move image along scienscope rotation value
    {
        float remainder = value % unitValue;
        image.uvRect = new Rect(remainder*reciprocal, 0, 1, 1);
    }

}
