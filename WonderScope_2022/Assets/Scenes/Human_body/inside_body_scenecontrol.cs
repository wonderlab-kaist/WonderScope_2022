using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inside_body_scenecontrol : MonoBehaviour
{
    public GameObject respiratory_system;
    //public Animator respiratory_movement;
    //public GameObject heart;
    public AudioSource heart_beat;


    private float heart_beat_speed;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(respiratory_system.GetComponent<Animation>().clip.name);

        //respiratory_system.GetComponent<Animation>()["CINEMA_4D_Main"].speed = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void set_beat_speed(float beating)
    {
        heart_beat_speed = beating;
        respiratory_system.GetComponent<Animation>()["CINEMA_4D_Main"].speed = heart_beat_speed;
        heart_beat.pitch = heart_beat_speed;
    }


}
