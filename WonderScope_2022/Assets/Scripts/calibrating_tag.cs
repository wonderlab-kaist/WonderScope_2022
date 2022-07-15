using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using UnityEngine.SceneManagement;


public class calibrating_tag : MonoBehaviour
{
    private bool measuring = false;
    private float measuredDistance = 0f;
    private int xSum, ySum = 0;
    private int trial = 0;
    float[] mouseVal = new float[5];
    float[] tagVal = new float[4];
    private float trial_time;

    public int maxTrial = 3;
    public float standardDistance = 15;
    public TextMeshProUGUI mouse, rfid, value, trialNum;
    public TextMeshProUGUI measuringStatus, savedValue, avgValue;
    public TextMeshProUGUI gain_recommendation;
    public TextMeshProUGUI avg_time_text;

    public TextMeshProUGUI gain_current, gain_average;
    private Vector3 origin_pos;

    // camera_movement.cs 에서 받아올 데이터들
    public camera_movement cm; // camera_movement에 접근해서 데이터 받아오i
    private bool connected = false;
    private byte[] tag_id;
    private float mouse_x, mouse_y;

    public Transform leftend, rightend;
    private float gain_rec;
    private float gain_avg=0f;

    private void Start()
    {
        origin_pos = cm.cam.position;
        cm.move_activation = false;
    }

    void Update()
    {

        if (connected)
        {
            mouse.text = "Mouse : " + (int)mouse_x + ",  " + (int)mouse_y;
            savedValue.text = mouseVal[0] + " | " + mouseVal[1] + " | " + mouseVal[2];
            gain_current.text = "Cur Gain: " + cm.gain;
            if (measuring)
                measuringStatus.text = "Measuring: true";
            else
                measuringStatus.text = "Measuring: false";

            if(gain_avg == 0)
                gain_average.text = "Avg Gain: " + cm.gain;
            else
                gain_average.text = "Avg Gain: " + gain_avg;


            if (!(tag_id[0] == 0 && tag_id[1] == 0 && tag_id[2] == 0 && tag_id[3] == 0)) //tag 감지
            {

                rfid.text = "RFID : " + Convert.ToString(tag_id[0], 16) + ":" + Convert.ToString(tag_id[1], 16) + ":" + Convert.ToString(tag_id[2], 16) + ":" + Convert.ToString(tag_id[3], 16);

                if (measuring == false) //measuring이 시작되지 않은 상태면 시작
                {
                    if ((tagVal[0] != tag_id[0] || tagVal[1] != tag_id[1] || tagVal[2] != tag_id[2] || tagVal[3] != tag_id[3]))
                    {
                        if (trial < maxTrial)
                        {
                            measuring = true;
                            cm.move_activation = true; //measuring하는 동안만 움직이게
                            dataInput.flush();
                            cm.cam.position = origin_pos;
                            cm.cam.rotation = Quaternion.identity;

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
                        cm.move_activation = false; //measuring하는 동안만 움직이게

                        mouseVal[trial] = Mathf.Sqrt(Mathf.Pow(xSum, 2) + Mathf.Pow(ySum, 2)); // 측정 값 저장

                        tagVal[0] = tag_id[0];
                        tagVal[1] = tag_id[1];
                        tagVal[2] = tag_id[2];
                        tagVal[3] = tag_id[3];
                        xSum = 0;
                        ySum = 0;

                        avg_time_text.text = "" + trial_time;
                        trial_time = 0;

                        trial++;
                        if (trial < maxTrial)
                        {
                            trialNum.text = "( " + (trial + 1) + " / " + maxTrial + " )";
                        }
                        else
                        {
                            for (int i = 0; i < maxTrial; i++) measuredDistance += mouseVal[i];
                            measuredDistance /= maxTrial; // trial 측정값 평균
                            avgValue.text = "Avg Dist: " + Convert.ToString(measuredDistance);
                            //기준길이(15cm: 변수명 standardDistance)랑 measuredDistance 값이랑 비교해서 gain 값 구하기
                            gain_rec = Vector3.Distance(leftend.position, rightend.position) / measuredDistance;
                            gain_recommendation.text = "Gain: " + Convert.ToString(gain_rec);

                            gain_avg = (cm.gain+gain_rec)/2;
                            cm.gain = gain_rec;

                            trial =0;
                            xSum =0;
                            ySum=0;
                            measuredDistance = 0f;
                            rfid.text = "RFID : 0:0:0:0";
                            mouseVal[0]=0f; mouseVal[1]=0f; mouseVal[2]=0f;
                        }
                    }
                }

            }
            if (measuring == true)
            {
                xSum += (int)mouse_x; // 측정 값 누적
                ySum += (int)mouse_y;
                value.text = "Value: " + xSum + " ," + ySum;

                trial_time += Time.deltaTime;
            }


        }

        ///reload the scene (임시)
        if(Input.touchCount > 2)
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
        if(Input.touchCount == 2) // trial initialize
        {
            cm.cam.position = origin_pos;
            cm.cam.rotation = Quaternion.identity;
            cm.move_activation = false; //measuring하는 동안만 움직이게
            xSum =0;
            ySum=0;
            measuring=false;
            tagVal[0] = 0; tagVal[1] = 0; tagVal[2] = 0; tagVal[3] = 0;
        }

    }

    private void LateUpdate() // it updates at the end of frames.
    {
        if (cm.isitConnected())
        {
            connected = true;
            mouse_x = cm.mouse_value().x;
            mouse_y = cm.mouse_value().y;
            tag_id = cm.rfid_value();
            //Debug.Log(tag_id);
            //print(tag_id);
        }
    }
}