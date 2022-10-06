using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bee_random_movement : MonoBehaviour
{
    private float theta, psy, yaw; // angles for spheral coordinate
    private int bee_status = 0; // 0: stop, 1:move forward, 2: turn left, 3: turn right

    private Transform body;
    private GameObject honeycomb;
    private Animation bee_movement;

    private float movement_radius;
    

    void Start()
    {
        // initialize first position
        //theta = - Mathf.PI/2f;
        //psy = 0;
        //yaw = 0;

        // initialize variables
        body = this.transform;
        bee_movement = this.GetComponent<Animation>();
        honeycomb = GameObject.Find("honeycombs_smooth");
        movement_radius = honeycomb.GetComponent<Collider>().bounds.size.x / 2f;

        StartCoroutine("random_move");
    }


    void Update()
    {
        
         body.position = honeycomb.transform.position + new Vector3(movement_radius * Mathf.Cos(theta) * Mathf.Cos(psy), movement_radius * Mathf.Sin(psy), movement_radius * Mathf.Cos(psy) * Mathf.Sin(theta));
         body.up = (body.position - honeycomb.transform.position).normalized;
         body.rotation *= Quaternion.EulerRotation(0, yaw, 0);

        #if UNITY_EDITOR
        if (Input.GetKey(KeyCode.W))
        {
            go_forward(2f);
            if (!bee_movement.IsPlaying("bee_walk")) bee_movement.Play();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            go_forward(-2f);
            if (!bee_movement.IsPlaying("bee_walk")) bee_movement.Play();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            turn_dir(-Mathf.PI/ 90f);
            if (!bee_movement.IsPlaying("bee_walk")) bee_movement.Play();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            turn_dir(Mathf.PI/ 90f);
            if (!bee_movement.IsPlaying("bee_walk")) bee_movement.Play();
        }
        #endif

        switch (bee_status)
        {
            case 0:

                break;
            case 1:
                turn_dir(-Mathf.PI / 90f);
                if (!bee_movement.IsPlaying("bee_walk")) bee_movement.Play();
                break;
            case 2:
                go_forward(1f);
                if (!bee_movement.IsPlaying("bee_walk")) bee_movement.Play();
                break;
            case 3:
                turn_dir(Mathf.PI / 90f);
                if (!bee_movement.IsPlaying("bee_walk")) bee_movement.Play();
                break;
        }
    }

    public void go_forward(float speed)
    {
        Vector3 goal_pos = body.position + body.forward * speed;
        float tmp_radius = (goal_pos - honeycomb.transform.position).magnitude;

        float goal_psy = Mathf.Asin((goal_pos - honeycomb.transform.position).y / tmp_radius);
        float goal_theta = Mathf.Acos((goal_pos - honeycomb.transform.position).x / (tmp_radius * Mathf.Cos(goal_psy)));
        //Debug.Log(theta+","+psy + " // " + goal_theta+","+goal_psy);
        //Debug.Log((goal_pos - honeycomb.transform.position).x / (tmp_radius * Mathf.Cos(goal_psy)) + "  " + goal_theta + "  "+(goal_pos - honeycomb.transform.position).z);

        if ((goal_pos - honeycomb.transform.position).y / tmp_radius >= 1) goal_psy = Mathf.PI / 2f;
        else if ((goal_pos - honeycomb.transform.position).y / tmp_radius <= -1) goal_psy = -Mathf.PI / 2f;
        if ((goal_pos - honeycomb.transform.position).x / (tmp_radius * Mathf.Cos(goal_psy)) >= 1) goal_theta = 0f;
        else if((goal_pos - honeycomb.transform.position).x / (tmp_radius * Mathf.Cos(goal_psy)) <= -1) goal_theta = Mathf.PI;

        if ((goal_pos - honeycomb.transform.position).z < 0) theta = -goal_theta;
        else theta = goal_theta;
        psy = goal_psy;

    }

    public void turn_dir(float angle)
    {
        //body.Rotate(body.up, angle);
        yaw += angle;
    }

    IEnumerator random_move()
    {
        while(true)
        {
            float gap = Random.value * 2.0f;
            float random_seed = Random.value;

            if (random_seed < 0.2f){
                bee_status = 1;
            }
            else if(random_seed >= 0.2f && random_seed < 0.6f){
                bee_status = 2;
            }else if(random_seed >= 0.6f && random_seed < 0.8f){
                bee_status = 3;
            }else{
                bee_status = 0;
            }
            
            yield return new WaitForSeconds(gap);
        }

    }

    public void random_pos()
    {
        
        theta = (Random.value - 0.5f) * 2 * Mathf.PI * 2f;
        psy = (Random.value - 0.5f) * 2 * Mathf.PI * 2f;

        yaw = (Random.value - 0.5f) * 2 * Mathf.PI * 2f;

        //Debug.Log(theta+","+psy+","+yaw);
    }
}
