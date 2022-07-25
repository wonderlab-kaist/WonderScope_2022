using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Camera_Movement_Moon : MonoBehaviour
{
    public float gain;

    float movement_threshold = 200;
    float distance_threshold = 60;
    float distance_limitation = 150;

    public Text altitude;
    public Text raw_data; //debugging text, monitoring raw data from module
    public Text direction; //orientation check
    private stethoscope_data data;
    public Transform cam;
    public Transform rig;
    public bool isthisWatch;
    public bool use_gravity; // checking for calibrating by gravity from mobile device data

    public float start_angle;

    //public Transform rotate_tester;

    private float[] q; //Quaternion container (temporal)
    private Quaternion origin;
    private bool originated = false;
    private int reset_count = 0;
    private int goback_count = 0;
    private int reconnect_duration = 0;
    private float cam_original_height;

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
        cam_original_height = cam.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        byte[] income = dataInput.getData();

        if (income != null && income != null)
        {
            //string[] data = income.Split(' ');
            data = new stethoscope_data(income);
            string monitoring = "";
            monitoring += data.q[0] + " " + data.q[1] + " " + data.q[2];
            monitoring += " " + data.distance;
            monitoring += " " + data.mouse_x + " " + data.mouse_y;
            //Debug.Log(monitoring);

            if (data.distance <= distance_limitation)
            {
            //Displacement values
            float x = 0;
                float y = 0;
                float z = 0;
                if (Mathf.Abs(data.mouse_x) > movement_threshold) x = data.mouse_x;
                if (Mathf.Abs(data.mouse_y) > movement_threshold) y = data.mouse_y;
                if (Mathf.Abs(data.distance) > distance_threshold) z = data.distance;
                Vector3 delta = new Vector3(x, 0, -y);

                //Quaternion for ratation
                for (int i = 0; i < 3; i++) q[i + 1] = data.q[i] / 1073741824f;

                if (1 - Mathf.Pow(q[1], 2) - Mathf.Pow(q[2], 2) - Mathf.Pow(q[3], 2) > 0 && Mathf.Abs(q[1]) < 1 && Mathf.Abs(q[2]) < 1 && Mathf.Abs(q[3]) < 1)
                {
                    q[0] = Mathf.Sqrt(1 - Mathf.Pow(q[1], 2) - Mathf.Pow(q[2], 2) - Mathf.Pow(q[3], 2));
                    Quaternion rot = new Quaternion(q[2], -q[1], -q[3], -q[0]);
                    //Quaternion rot = new Quaternion(q[0], q[1], q[2], q[3]);

                    float angle = Quaternion.Angle((origin * rot), rig.rotation);
                    direction.text = "" + rot.ToEuler().z / Mathf.PI * 180f;

                    if (!originated && !use_gravity)
                    {
                        originated = true;

                        //origin = Quaternion.Inverse(rot);
                        origin = rot;
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

                rotate_smooth(new Vector3(0, -rig.localEulerAngles.z, 0));

                delta = cam.localRotation * Quaternion.Euler(0,0,90f) * delta;
                move_smooth(delta);
                
            }else if (data.distance >= distance_limitation)
            {
                //SceneManager.LoadScene("1_RFID_waiting_moon", LoadSceneMode.Single); /// go back to rfid waiting scene...               
                SceneManager.LoadScene(0, LoadSceneMode.Single); /// go back to rfid waiting scene...      
            }



            reconnect_duration = 0;
        }
        else if (income == null || income == null)
        {
            //Debug.Log(reconnect_duration++);
            reconnect_duration++;
            //GameObject.Find("BLEcontroller").GetComponent<aarcall>().connect();
        }

        if (reconnect_duration >= 200)
        {
            Debug.Log("reconnecting...");
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
        cam.position = new Vector3(cam.position.x, cam_original_height + data.distance/3, cam.position.z);
        altitude.text = string.Format("{0:N2}", data.distance); //altitude
        raw_data.text = cam.position.ToString();
    }

    IEnumerator rotateSmooth(Vector3 d)
    {
        Quaternion start = cam.localRotation;
        Quaternion end = Quaternion.Euler(d.x, d.y, d.z);

        for (int i = 0; i < 3; i++)
        {
            //cam.transform.localRotation = cam.transform.localRotation * Quaternion.Euler(0, 0, d.z / 3f);
            cam.localRotation = Quaternion.Slerp(start, end, (float)(1f / 3f * (i + 1)));

            yield return new WaitForSeconds(0.01f);
        }

    }
}
