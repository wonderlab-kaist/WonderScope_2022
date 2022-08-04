using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class siggraph_coloring : MonoBehaviour
{
    public float Speed = 1;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        rend.material.SetColor("_Color", Color.HSVToRGB(Mathf.PingPong(Time.time * Speed, 1), 1, 1));
    }
}