using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class camera_movement_circular : MonoBehaviour
{
    public float gain;

    float movement_threshold = 250f;
    float distance_threshold = 100f;

    public TextMeshProUGUI raw_data; //debugging text, monitoring raw data from module
    private stethoscope_data data;
    public Transform cam;
    public Transform rig;
    public Transform target;

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

    private float theta, psy;
    private float cam_dis_target;

    /// <summary>
    /// added for human-body calibration
    /// </summary>
    private int reset_index;
    private float threshold = 2f;


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
        //Debug.Log(address.GetLastRFID());
        for (int i = 0; i < reset_RFIDs.Length; i++)
        {
            if (address.GetLastRFID().Equals(reset_RFIDs[i])) cam.position = reset_points[i].position;
        }

        // x,y,z to radial pos
        float tmp_radius = (cam.position - target.transform.position).magnitude;
        psy = Mathf.Asin((cam.position - target.transform.position).y / tmp_radius);
        theta = Mathf.Acos((cam.position - target.transform.position).x / (tmp_radius * Mathf.Cos(psy)));
        Debug.Log(theta + " " + psy);
        cam_dis_target = Vector3.Distance(cam.position, target.position);

    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.UpArrow))
        {
            psy += 0.01f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            psy -= 0.01f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            theta += 0.01f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            theta -= 0.01f;
        }
        cam.LookAt(target);
        cam.position = target.position + new Vector3(cam_dis_target * Mathf.Cos(theta) * Mathf.Cos(psy), cam_dis_target * Mathf.Sin(psy), cam_dis_target * Mathf.Cos(psy) * Mathf.Sin(theta));

#endif

        byte[] income = dataInput.getData();

        if (income != null)
        {
            //string[] data = income.Split(' ');
            data = new stethoscope_data(income, 1);
            string monitoring = "";
            monitoring += data.q[0] + " " + data.q[1] + " " + data.q[2];
            monitoring += "\n" + data.distance;
            monitoring += "\n" + data.mouse_x + " " + data.mouse_y;

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
                Vector3 delta = new Vector3(y, -x, 0);

                //Quaternion for rotation
                for (int i = 0; i < 3; i++) q[i + 1] = data.q[i] / 1073741824f;
                //for (int i = 0; i < 3; i++) q[i + 1] = data.q[i];


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
                    cam.LookAt(target);
                    rotate_smooth(new Vector3(0, 0, rig.localEulerAngles.z));

                    //delta = cam.localRotation * delta;
                    delta = cam.GetChild(0).localRotation * delta;
                    //Debug.Log(delta);
                    move_smooth(delta);

                }
                else
                {
                    cam.LookAt(target);

                    if(Input.touchCount == 1)
                    {
                        delta = Input.GetTouch(0).deltaPosition * -30f;
                        move_smooth(delta);
                    }
                }
            }
            else if (data.distance >= distance_threshold && reset_by_dist)
            {
                SceneManager.LoadScene(0, LoadSceneMode.Single);
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

    //In this application, it will be rotate
    void move_smooth(Vector3 delta_distance)
    {
        StartCoroutine("moveSmooth_rot", delta_distance * gain);

    }

    void rotate_smooth(Vector3 delta_angle)
    {
        StartCoroutine("rotateSmooth", delta_angle);

    }

    IEnumerator moveSmooth(Vector3 d)
    {
        /*
        for (int i = 0; i < 3; i++)
        {
            cam.position -= d / 3f;
            yield return new WaitForSeconds(0.02f / 3f);
        }*/

        Vector3 goal_pos = cam.position + d;
        float tmp_radius = (goal_pos - target.transform.position).magnitude;

        float goal_psy = Mathf.Asin((goal_pos - target.transform.position).y / tmp_radius);
        float goal_theta = Mathf.Acos((goal_pos - target.transform.position).x / (tmp_radius * Mathf.Cos(goal_psy)));
        //Debug.Log(theta+","+psy + " // " + goal_theta+","+goal_psy);
        //Debug.Log((goal_pos - honeycomb.transform.position).x / (tmp_radius * Mathf.Cos(goal_psy)) + "  " + goal_theta + "  "+(goal_pos - honeycomb.transform.position).z);

        if ((goal_pos - target.transform.position).y / tmp_radius >= 1) goal_psy = Mathf.PI / 2f;
        else if ((goal_pos - target.transform.position).y / tmp_radius <= -1) goal_psy = -Mathf.PI / 2f;
        if ((goal_pos - target.transform.position).x / (tmp_radius * Mathf.Cos(goal_psy)) >= 1) goal_theta = 0f;
        else if ((goal_pos - target.transform.position).x / (tmp_radius * Mathf.Cos(goal_psy)) <= -1) goal_theta = Mathf.PI;

        Debug.Log("(" + theta + "," + psy + ") -> " + "(" + goal_theta + "," + psy + ")" + "   "+ (goal_pos - target.transform.position).z);

        //if ((goal_pos - target.transform.position).z < 0f) theta = -goal_theta;
        theta = goal_theta;
        psy = goal_psy;

        //cam.position = target.position + new Vector3(cam_dis_target * Mathf.Cos(theta) * Mathf.Cos(psy), cam_dis_target * Mathf.Sin(psy), cam_dis_target * Mathf.Cos(psy) * Mathf.Sin(theta));
        cam.position = target.position + Quaternion.AngleAxis(psy * 180/Mathf.PI, Quaternion.Euler(0f, theta * 180 / Mathf.PI, 0f) * Vector3.forward) *(Quaternion.Euler(0f,theta * 180 / Mathf.PI,0f) * new Vector3(cam_dis_target, 0f, 0f));


        yield return new WaitForEndOfFrame();
    }

    IEnumerator moveSmooth_rot(Vector3 d)
    {
        Vector3 goal_pos = cam.position + cam.rotation * d;
        float tmp_radius = (goal_pos - target.position).magnitude;

        Vector3 goal_pos_vector = goal_pos - target.position;
        goal_pos_vector *= cam_dis_target / tmp_radius;

        cam.position = target.position + goal_pos_vector;

        psy = Mathf.Asin((goal_pos - target.transform.position).y / tmp_radius);
        theta = Mathf.Acos((goal_pos - target.transform.position).x / (tmp_radius * Mathf.Cos(psy)));
        
        if ((goal_pos - target.transform.position).y / tmp_radius >= 1) psy = Mathf.PI / 2f;
        else if ((goal_pos - target.transform.position).y / tmp_radius <= -1) psy = -Mathf.PI / 2f;
        if ((goal_pos - target.transform.position).x / (tmp_radius * Mathf.Cos(psy)) >= 1) theta = 0f;
        else if ((goal_pos - target.transform.position).x / (tmp_radius * Mathf.Cos(psy)) <= -1) theta = Mathf.PI;

        yield return new WaitForEndOfFrame();
    }

    IEnumerator rotateSmooth(Vector3 d)
    {
        Quaternion start = cam.GetChild(0).localRotation;
        Quaternion end = Quaternion.Euler(0, 0, d.z);

        for (int i = 0; i < 3; i++)
        {
            //cam.transform.localRotation = cam.transform.localRotation * Quaternion.Euler(0, 0, d.z / 3f);
            cam.GetChild(0).localRotation = Quaternion.Slerp(start, end, (float)(1f / 3f * (i + 1)));

            yield return new WaitForSeconds(0.01f);
        }

    }
}

