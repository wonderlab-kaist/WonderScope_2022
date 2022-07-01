using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class calibrating_tag : MonoBehaviour
{
    private bool measuring = false;
    private float measuredDistance = 0f;
    private int xSum, ySum = 0;
    private int trial = 0;
    float[] mouseVal = new float[5];
    float[] tagVal = new float[4];

    public int maxTrial = 3;
    public float standardDistance = 15;
    public TextMeshProUGUI mouse, rfid, value, trialNum;
    public TextMeshProUGUI measuringStatus, savedValue, avgValue;

    // camera_movement.cs 에서 받아올 데이터들
    public bool connected = false;
    public byte[] tag_id;
    public float mouse_x, mouse_y;
    //

    void Update()
    {
        print("maxTrial: " + maxTrial);
        print("standardDistance: " + standardDistance);

        if (connected)
        {
            mouse.text = "Mouse : " + (int)mouse_x + ",  " + (int)mouse_y;
            if (measuring)
                measuringStatus.text = "Measuring: true";
            else
                measuringStatus.text = "Measuring: false";
            savedValue.text = mouseVal[0] + " | " + mouseVal[1] + " | " + mouseVal[2] + " | " + mouseVal[3] + " | " + mouseVal[4];

            if (!(tag_id[0] == 0 && tag_id[1] == 0 && tag_id[2] == 0 && tag_id[3] == 0)) //tag 감지
            {

                rfid.text = "RFID : " + Convert.ToString(tag_id[0], 16) + ":" + Convert.ToString(tag_id[1], 16) + ":" + Convert.ToString(tag_id[2], 16) + ":" + Convert.ToString(tag_id[3], 16);

                if (measuring == false) //measuring이 시작되지 않은 상태면 시작
                {
                    if ((trial == 0) || ((trial > 0) && (tagVal[0] != tag_id[0] || tagVal[1] != tag_id[1] || tagVal[2] != tag_id[2] || tagVal[3] != tag_id[3])))
                    {
                        if (trial < maxTrial)
                        {
                            measuring = true;
                            tagVal[0] = tag_id[0];
                            tagVal[1] = tag_id[1];
                            tagVal[2] = tag_id[2];
                            tagVal[3] = tag_id[3];
                        }
                    }
                }
                else //measuring 중이면 (measuring == true)
                {
                    if (tagVal[0] != tag_id[0] || tagVal[1] != tag_id[1] || tagVal[2] != tag_id[2] || tagVal[3] != tag_id[3]) //새로운 tag(두번째) 감지되면
                    {
                        measuring = false; //measuring 중지
                        mouseVal[trial] = Mathf.Sqrt(Mathf.Pow(xSum, 2) + Mathf.Pow(ySum, 2)); // 측정 값 저장
                        tagVal[0] = tag_id[0];
                        tagVal[1] = tag_id[1];
                        tagVal[2] = tag_id[2];
                        tagVal[3] = tag_id[3];
                        xSum = 0;
                        ySum = 0;
                        trial++;
                        if (trial < maxTrial)
                        {
                            trialNum.text = "( " + (trial + 1) + " / " + maxTrial + " )";
                        }
                        else
                        {
                            for (int i = 0; i < maxTrial; i++) measuredDistance += mouseVal[i];
                            measuredDistance /= maxTrial; // trial 측정값 평균
                            avgValue.text = Convert.ToString(measuredDistance);
                            //기준길이(15cm: 변수명 standardDistance)랑 measuredDistance 값이랑 비교해서 gain 값 구하기
                        }
                    }
                }

            }
            if (measuring == true)
            {
                xSum += (int)mouse_x; // 측정 값 누적
                ySum += (int)mouse_y;
                value.text = "Value: " + xSum + " ," + ySum;
            }


        }

    }
}
