  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;


public class camera_movement : MonoBehaviour
{
    public float gain;
    public float movement_threshold = 10f;
    public float distance_threshold = 130f;

    public TextMeshProUGUI raw_data; //debugging text, monitoring raw data from module
    private stethoscope_data data;
    public Transform cam;
    public Transform rig;
    public bool isthisWatch;
    public bool use_gravity; // checking for calibrating by gravity from mobile device data
    public bool move_activation = true; // movement activation, default = true
    public bool reset_by_dist = true;
    public bool recalibrate_while_movement = true;
    public string[] reset_RFIDs;
    public Transform[] reset_points;



    private float[] q; //Quaternion container (temporal)
    private Quaternion origin;
    private bool originated = false;
    private int reset_count = 0;
    private int goback_count = 0;
    private int reconnect_duration = 0;

    /// <summary>
    /// added for human-body calibration
    /// </summary>
    private int reset_index;
    private float threshold = 2f;

    // Start is called before the first frame update
    void Start()
    {
        if (use_gravity)
        {
            Input.gyro.enabled = true;
            Vector3 gdir = Input.gyro.gravity;
            origin = Quaternion.EulerAngles(0, 0, -Mathf.Atan(gdir.y / gdir.x));

            if (gdir.x > 0) origin = origin * Quaternion.Euler(0, 0, -90);
            else origin = origin * Quaternion.Euler(0, 0, 90);

            rig.rotation = (origin);
            cam.localRotation = Quaternion.Euler(new Vector3(0, 0, rig.localEulerAngles.z));
        }

        q = new float[4];
        Debug.Log(address.GetLastRFID());
        for(int i = 0; i < reset_RFIDs.Length;i++)
        {
            if (address.GetLastRFID().Equals(reset_RFIDs[i])) cam.position = reset_points[i].position;
        }

    }

    // Update is called once per frame
    void Update()
    {
        byte[] income = dataInput.getData();

        if (income != null)
        {
            data = new stethoscope_data(income, 2);
            //Debug.Log(data.mouse_x + " " + data.mouse_y);

            if (recalibrate_while_movement && !(data.tag_id[0] == 0 && data.tag_id[1] == 0 && data.tag_id[2] == 0 && data.tag_id[3] == 0))
            {
                string id = System.BitConverter.ToString(data.tag_id).Replace("-", ":");
                reset_index = Array.FindIndex(reset_RFIDs, element => element == id);
                if (!address.GetLastRFID().Equals(id) && Vector3.Distance(cam.position, reset_points[reset_index].position) > threshold)
                {
                    address.SetLastRFID(id);
                    cam.position = reset_points[reset_index].position;
                }

            }


            if (data.distance <= distance_threshold)
            {
                //Displacement values
                float x = 0;
                float y = 0;
                if (Mathf.Abs(data.mouse_x) > movement_threshold) x = data.mouse_x;
                if (Mathf.Abs(data.mouse_y) > movement_threshold) y = data.mouse_y;
                //Vector3 delta = new Vector3(-y,x,0);// version 2
                Vector3 delta = new Vector3(x, y, 0) * -1f;

                //Debug.Log(delta);
                //Quaternion for rotation
                //for (int i = 0; i < 3; i++) q[i + 1] = data.q[i] / 1073741824f; // version 1
                for (int i = 0; i < 3; i++) q[i + 1] = data.q[i];


                if (1 - Mathf.Pow(q[1], 2) - Mathf.Pow(q[2], 2) - Mathf.Pow(q[3], 2) > 0 && Mathf.Abs(q[1]) < 1 && Mathf.Abs(q[2]) < 1 && Mathf.Abs(q[3]) < 1)
                {
                    q[0] = Mathf.Sqrt(1 - Mathf.Pow(q[1], 2) - Mathf.Pow(q[2], 2) - Mathf.Pow(q[3], 2));
                    Quaternion rot = new Quaternion(q[2], -q[1], -q[3], -q[0]);

                    float angle = Quaternion.Angle((origin * rot), rig.rotation);

                    if (!originated && !use_gravity)
                    {
                        originated = true;

                        origin = Quaternion.Inverse(rot);
                    }
                    else if (use_gravity)
                    {
                        ///Gravity Indicator, Rotation///
                        Vector3 gdir = Input.gyro.gravity;
                        origin = Quaternion.EulerAngles(0, 0, -Mathf.Atan(gdir.y / gdir.x));

                        if (gdir.x > 0) origin = origin * Quaternion.Euler(0, 0, -90);
                        else origin = origin * Quaternion.Euler(0, 0, 90);

                        origin = origin * Quaternion.Inverse(rot);
                        //Debug.Log(Vector3.Distance(cam.position, cam.GetChild(0).transform.position));
                        //Debug.Log(cam.GetChild(0).transform.localRotation);
                    }

                    if (angle < 40)
                    {
                        rig.rotation = (origin * rot);
                    }
                    else if (angle >= 40)
                    {
                        reset_count++;
                    }

                    if (reset_count > 20)
                    {
                        rig.rotation = (origin * rot);
                        reset_count = 0;
                    }

                }

                if (move_activation)
                {
                    rotate_smooth(new Vector3(0, 0, rig.localEulerAngles.z));

                    delta = cam.localRotation * delta;
                    move_smooth(delta);
                }
            }
            else if (data.distance >= distance_threshold && reset_by_dist)
            {
                if (!isthisWatch)
                {
                    //SceneManager.LoadScene("1_RFID_waiting", LoadSceneMode.Single); /// go back to rfid waiting scene...
                    SceneManager.LoadScene(0, LoadSceneMode.Single);
                }
                else SceneManager.LoadScene("0_watch_start", LoadSceneMode.Single);


            }



            reconnect_duration = 0;
        }
        else if (income == null)
        {
            //Debug.Log(reconnect_duration++);
            reconnect_duration++;
            //GameObject.Find("BLEcontroller").GetComponent<aarcall>().connect();
        }

        if (reconnect_duration >= 200)
        {
            //Debug.Log("reconnecting...");
            GameObject.Find("BLEcontroller").GetComponent<aarcall>().connect();
            reconnect_duration = 0;
        }

    }

    void move_smooth(Vector3 delta_distance)
    {
        StartCoroutine("moveSmooth", delta_distance * gain);

    }

    void rotate_smooth(Vector3 delta_angle)
    {
        StartCoroutine("rotateSmooth", delta_angle);

    }

    IEnumerator moveSmooth(Vector3 d)
    {
        for (int i = 0; i < 3; i++)
        {
            cam.position -= d / 3f;
            yield return new WaitForSeconds(0.02f / 3f);
        }
    }

    IEnumerator rotateSmooth(Vector3 d)
    {
        Quaternion start = cam.localRotation;
        Quaternion end = Quaternion.Euler(0, 0, d.z);

        for (int i = 0; i < 3; i++)
        {
            //cam.transform.localRotation = cam.transform.localRotation * Quaternion.Euler(0, 0, d.z / 3f);
            cam.localRotation = Quaternion.Slerp(start, end, (float)(1f / 3f * (i + 1)));

            yield return new WaitForSeconds(0.01f);
        }

    }

    /// <summary>
    /// added for get the data from this script.
    /// recommend to use it in the lateUpdate()
    /// </summary>
    /// <returns></returns>
    public Vector2 mouse_value()
    {
        Vector2 value = new Vector2(data.mouse_x, data.mouse_y);
        return value;
    }

    public byte[] rfid_value()
    {
        byte[] value = data.tag_id;
        return value;
    }

    public bool isitConnected()
    {
        if (data == null) return false;
        else return true;
    }
}