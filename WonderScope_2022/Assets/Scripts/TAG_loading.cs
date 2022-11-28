﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TAG_loading : MonoBehaviour 
{   //태그 인식되면, 컨텐츠 로딩하는 스크립트

    /// <summary>
    /// Inspector 창에서 RFID_address에 원하는 RFID 태그의 주소값을 입력해주세요.
    /// 해당 RFID 태그 주소의 index + 1의 scene id로 자동 대응되어 넘어갑니다.
    /// 혹은 아래 update 구분의 아래에, 직접 tag id와 scene builid index를 대응시켜 주세요.
    ///
    ///
    /// 이 스크립틑 벌 특별전 _용 스크립
    /// </summary>
    public TextMeshProUGUI heading;
    public TextMeshProUGUI explain;

    public string[] RFID_address_life, RFID_address_anatomy, RFID_address_hive;

    public GameObject scienscope_illust;
    public Transform touchPoint;
    public GameObject ps_effect;
    public Transform ps_origin;

    private bool scene_detected = false;
    private Vector3 sc_illust_origin;

    public bool scene_transition_test = false;

    public string detected_expression;

    
    void Start()
    {
        address.SetLastRFID("");
        sc_illust_origin = scienscope_illust.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        stethoscope_data tmp = new stethoscope_data(dataInput.getData(),2);

        if (tmp!=null&&Application.platform == RuntimePlatform.Android)
        {
            //heading.text = "rea";
            Debug.Log(tmp.distance);
            //Debug.Log(tmp.tag_id[0]);
            scienscope_illust.transform.position = Vector3.Lerp(sc_illust_origin, touchPoint.position, (float)((255 - tmp.distance) / 255f));            

            if (!(tmp.tag_id[0] == 0&& tmp.tag_id[1] == 0&& tmp.tag_id[2] == 0&& tmp.tag_id[3] == 0) && !scene_detected)
            {
                //Instantiate(ps_effect, ps_origin).transform.localPosition = Vector3.zero;

                if (scene_transition_test)
                {
                    scene_detected = true;

                    SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
                }
                //scene_detected = true;


                //SceneManager.LoadSceneAsync(1, LoadSceneMode.Single); ///i값을 원하는 scene의 build index로 대체
                Debug.Log(System.BitConverter.ToString(tmp.tag_id).Replace("-", ":"));
                string id = System.BitConverter.ToString(tmp.tag_id).Replace("-", ":");
                address.SetLastRFID(id); //save RFID Address for load in next scene
                //explain.text = id;

                for (int i = 0; i < RFID_address_anatomy.Length; i++)
                {
                    if (RFID_address_anatomy[i].Equals(id))
                    {
                        explain.text = detected_expression;
                        Instantiate(ps_effect, ps_origin).transform.localPosition = Vector3.zero;

                        /// move on contents scenes ///
                        scene_detected = true;
                        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single); ///i값을 원하는 scene의 build index로 대체
                        
                    }
                }

                for (int i = 0; i < RFID_address_life.Length; i++)
                {
                    if (RFID_address_life[i].Equals(id))
                    {
                        explain.text = detected_expression;
                        Instantiate(ps_effect, ps_origin).transform.localPosition = Vector3.zero;


                        /// move on contents scenes ///
                        scene_detected = true;
                        SceneManager.LoadSceneAsync(3+i, LoadSceneMode.Single); ///i값을 원하는 scene의 build index로 대체

                    }
                }
                for (int i = 0; i < RFID_address_hive.Length; i++)
                {
                    if (RFID_address_hive[i].Equals(id))
                    {
                        explain.text = detected_expression;
                        Instantiate(ps_effect, ps_origin).transform.localPosition = Vector3.zero;
                        //ps_effect.Play();


                        /// move on contents scenes ///
                        scene_detected = true;
                        SceneManager.LoadSceneAsync(2, LoadSceneMode.Single); ///i값을 원하는 scene의 build index로 대체

                    }
                }




            }
        }

        
    }
}
