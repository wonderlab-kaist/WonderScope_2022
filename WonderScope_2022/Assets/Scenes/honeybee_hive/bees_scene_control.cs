using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bees_scene_control : MonoBehaviour
{
    public GameObject honey_bee;

    public int numb_of_bees;

    void Start()
    {
        for(int i = 0; i < numb_of_bees; i++)
        {
            Instantiate(honey_bee, GameObject.Find("indicator").transform).GetComponent<bee_random_movement>().random_pos();
        }
    }

    void Update()
    {
        
    }
}
