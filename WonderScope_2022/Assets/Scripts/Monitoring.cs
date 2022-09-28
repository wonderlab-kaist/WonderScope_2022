using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Monitoring : MonoBehaviour
{
    public TextMeshProUGUI address, connection, distance, rfid, mouse, rotation;
    public GameObject rotation_indicator;
    Quaternion q;

    void Start()
    {
        address.text = "Address : " + GameObject.Find("BLEcontroller").GetComponent<aarcall>().default_ble_address;
    }

    // Update is called once per frame
    void Update()
    {
        if (dataInput.isConnected())
        {
            connection.text = "Connection : True";

            stethoscope_data tmp = new stethoscope_data(dataInput.getData(),1);
            Debug.Log(tmp.q[0]+ " " + tmp.q[1] + " " + tmp.q[2]);


            distance.text = "Distance : " + (float)tmp.distance;
            if (!(tmp.tag_id[0] == 0 && tmp.tag_id[1] == 0 && tmp.tag_id[2] == 0 && tmp.tag_id[3] == 0))
                rfid.text = "RFID : " + Convert.ToString(tmp.tag_id[0],16)+":"+ Convert.ToString(tmp.tag_id[1], 16)+":"+ Convert.ToString(tmp.tag_id[2], 16)+":"+ Convert.ToString(tmp.tag_id[3], 16);
            mouse.text = "Mouse : " + (float)tmp.mouse_x*100 + ",  " + (float)tmp.mouse_y*100;


            ////rotation/////
            for (int i = 0; i < 3; i++) q[i + 1] = tmp.q[i] / 1073741824f;

            if (1 - Mathf.Pow(q[1], 2) - Mathf.Pow(q[2], 2) - Mathf.Pow(q[3], 2) > 0 && Mathf.Abs(q[1]) < 1 && Mathf.Abs(q[2]) < 1 && Mathf.Abs(q[3]) < 1)
            {
                q[0] = Mathf.Sqrt(1 - Mathf.Pow(q[1], 2) - Mathf.Pow(q[2], 2) - Mathf.Pow(q[3], 2));
                Quaternion rot = new Quaternion(q[2], -q[1], -q[3], -q[0]);

                rotation_indicator.transform.rotation = rot;
            }
            else
            {
                connection.text = "Connection : False";
            }

            rotation.text = "Angle : " + rotation_indicator.transform.localEulerAngles;
            //////////////////
        }
    }
}
