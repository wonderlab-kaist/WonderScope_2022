using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class inside_body_scenecontrol : MonoBehaviour
{
    public GameObject respiratory_system;
    //public Animator respiratory_movement;
    //public GameObject heart;
    public AudioSource heart_beat;
    public TextMeshProUGUI heart_rate_txt;
    public TextMeshProUGUI explanation;
    public AudioClip sleeping, normal, running;

    private int heart_rate = 73;
    private int random;

    string running_exp = "When you are running, the heart rate increased to deliver more oxygen.";
    string normal_exp = "Our heart beats, on average, between 60 and 90 times per minute.";
    string sleeping_exp = "When you are sleeping, your metabolism slows down and your heart rate also decreased.";


    private float heart_beat_speed;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(respiratory_system.GetComponent<Animation>().clip.name);
        StartCoroutine("random_bpm");
        //respiratory_system.GetComponent<Animation>()["CINEMA_4D_Main"].speed = 3.0f;
        explanation.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        heart_rate_txt.text = "" + (heart_rate + random);

    }

    public void set_beat_speed(float beating)
    {
        heart_beat_speed = beating;
        respiratory_system.GetComponent<Animation>()["CINEMA_4D_Main"].speed = heart_beat_speed;
        heart_beat.pitch = heart_beat_speed;

        if(beating < 1)
        {
            heart_rate = 50;
            this.gameObject.GetComponent<AudioSource>().clip = sleeping;
            explanation.text = sleeping_exp;
        }else if(beating > 1)
        {
            heart_rate = 110;
            this.gameObject.GetComponent<AudioSource>().clip = running;
            explanation.text = running_exp;
        }
        else
        {
            heart_rate = 73;
            this.gameObject.GetComponent<AudioSource>().clip = normal;
            explanation.text = normal_exp;

        }
        this.gameObject.GetComponent<AudioSource>().Play();

    }

    IEnumerator random_bpm()
    {
        for(; ; )
        {
            random = (int)(Random.value * 3);
            yield return new WaitForSeconds(1.5f);
        }
    }
}
