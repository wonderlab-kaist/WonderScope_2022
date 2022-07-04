using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class Calibrating_init : MonoBehaviour
{
    private bool measuring = false;
    private float measuredDistance = 0f;
    private int xSum, ySum = 0;
    private int trial = 0;
    float[] mouseVal = new float[5];
    float[] tagVal = new float[4];

    public int maxTrial = 0;
    public float standardDistance = 0f;
    public TextMeshProUGUI mouse, rfid, value, trialNum;
    public TextMeshProUGUI measuringStatus, savedValue,avgValue;

    public GameObject calibratingPanel;

    void Start()
    {
        calibratingPanel.SetActive(false);
    }

    void Update()
    {
        print("maxTrial: "+maxTrial);
        print("standardDistance: "+standardDistance);

        if(trial == 0) trialNum.text = "( 1 / " + maxTrial +" )";
        if (dataInput.isConnected())
        {
            stethoscope_data tmp = new stethoscope_data(dataInput.getData());
            mouse.text = "Mouse : " + (int)tmp.mouse_x + ",  " + (int)tmp.mouse_y;
            if (measuring)
                measuringStatus.text = "Measuring: true";
            else
                measuringStatus.text = "Measuring: false";
            savedValue.text = mouseVal[0] + " | " + mouseVal[1] + " | " + mouseVal[2] +" | " + mouseVal[3] + " | " + mouseVal[4];

            if (!(tmp.tag_id[0] == 0 && tmp.tag_id[1] == 0 && tmp.tag_id[2] == 0 && tmp.tag_id[3] == 0)) //tag 감지
            {

                rfid.text = "RFID : " + Convert.ToString(tmp.tag_id[0], 16) + ":" + Convert.ToString(tmp.tag_id[1], 16) + ":" + Convert.ToString(tmp.tag_id[2], 16) + ":" + Convert.ToString(tmp.tag_id[3], 16);

                if (measuring == false) //measuring이 시작되지 않은 상태면 시작
                {
                    if ((trial == 0) || ((trial > 0) && (tagVal[0] != tmp.tag_id[0] || tagVal[1] != tmp.tag_id[1] || tagVal[2] != tmp.tag_id[2] || tagVal[3] != tmp.tag_id[3])))
                    {
                        if (trial < maxTrial)
                        {
                            measuring = true;
                            tagVal[0] = tmp.tag_id[0];
                            tagVal[1] = tmp.tag_id[1];
                            tagVal[2] = tmp.tag_id[2];
                            tagVal[3] = tmp.tag_id[3];
                        }
                    }
                }
                else //measuring 중이면 (measuring == true)
                {
                    if (tagVal[0] != tmp.tag_id[0] || tagVal[1] != tmp.tag_id[1] || tagVal[2] != tmp.tag_id[2] || tagVal[3] != tmp.tag_id[3]) //새로운 tag(두번째) 감지되면
                    {
                        measuring = false; //measuring 중지
                        mouseVal[trial] = Mathf.Sqrt(Mathf.Pow(xSum, 2) + Mathf.Pow(ySum, 2)); // 측정 값 저장
                        tagVal[0] = tmp.tag_id[0];
                        tagVal[1] = tmp.tag_id[1];
                        tagVal[2] = tmp.tag_id[2];
                        tagVal[3] = tmp.tag_id[3];
                        xSum = 0;
                        ySum = 0;
                        trial++;
                        if (trial < maxTrial)
                        {
                            trialNum.text = "( " + (trial + 1) + " / " + maxTrial +" )";
                        }
                        else
                        {
                            for (int i = 0; i < maxTrial; i++) measuredDistance += mouseVal[i];
                            measuredDistance /= maxTrial; // trial 측정값 평균
                            avgValue.text = Convert.ToString(measuredDistance);
                                                  //기준길이(10cm)랑 measureDistance 값이랑 비교 
                        }
                    }
                }

            }
            if (measuring == true)
            {
                xSum += (int)tmp.mouse_x; // 측정 값 누적
                ySum += (int)tmp.mouse_y;
                value.text = "Value: " + xSum + " ," + ySum;
            }


        }
        // if(resetBtn 클릭)
        // {
        //     trial -= 1;
        //     measuring = false;
        //     xSum = 0;
        //     ySum = 0;
        // }
    }
}
