using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Bee_life_TAG_loading : MonoBehaviour
{
    public TextMeshProUGUI heading;
    public TextMeshProUGUI explain;

    public string[] RFID_address;

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

    void Update()
    {
        stethoscope_data tmp = new stethoscope_data(dataInput.getData());

        if (tmp != null && Application.platform == RuntimePlatform.Android)
        {
            //heading.text = "rea";
            Debug.Log(tmp.distance);
            //Debug.Log(tmp.tag_id[0]);
            scienscope_illust.transform.position = Vector3.Lerp(sc_illust_origin, touchPoint.position, (float)((255 - tmp.distance) / 255f));

            if (!(tmp.tag_id[0] == 0 && tmp.tag_id[1] == 0 && tmp.tag_id[2] == 0 && tmp.tag_id[3] == 0) && !scene_detected)
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

                for (int i = 0; i < RFID_address.Length; i++)
                {
                    if (RFID_address[i].Equals(id))
                    {
                        explain.text = detected_expression;
                        Instantiate(ps_effect, ps_origin).transform.localPosition = Vector3.zero;
                        //ps_effect.Play();


                        /// move on contents scenes ///
                        scene_detected = true;
                        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single); ///i값을 원하는 scene의 build index로 대체

                    }
                }





            }
        }
    }
}
