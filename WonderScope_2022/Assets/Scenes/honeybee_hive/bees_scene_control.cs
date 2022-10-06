using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class bees_scene_control : MonoBehaviour
{
    public GameObject honey_bee;
    public GameObject Aim;
    public Material Sealing_Mat, Honey_Mat, Larva_Mat, Egg_Mat;

    public TextMeshProUGUI explanation;

    private Color Sealing_color, Honey_color, Larva_color, Egg_color;
    private bool glowing = false;

    [TextArea]
    public string Default, Larva, Honey, Egg, Sealing_larva, Sealing_honey;

    public int numb_of_bees;

    void Start()
    {
        Sealing_color = Sealing_Mat.color;
        Honey_color = Honey_Mat.color;
        Larva_color = Larva_Mat.color;
        Egg_color = Egg_Mat.color;

        for(int i = 0; i < numb_of_bees; i++)
        {
            Instantiate(honey_bee, GameObject.Find("indicator").transform).GetComponent<bee_random_movement>().random_pos();
        }
    }

    void Update()
    {

        RaycastHit hit;
        if(Physics.Raycast(Aim.transform.position, Aim.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            //Debug.Log(hit.collider);
            if (hit.collider.name == "Larva")
            {
                explanation.text = Larva;
                if (!glowing) StartCoroutine("glowing_mat", Larva_Mat);
            }
            else if (hit.collider.name == "Honey")
            {
                explanation.text = Honey;
                if (!glowing) StartCoroutine("glowing_mat", Honey_Mat);
            }
            else if (hit.collider.name == "Sealing_Larva")
            {
                explanation.text = Sealing_larva;
                if (!glowing) StartCoroutine("glowing_mat", Sealing_Mat);
            }
            else if (hit.collider.name == "Sealing_Honey")
            {
                explanation.text = Sealing_honey;
                if (!glowing) StartCoroutine("glowing_mat", Sealing_Mat);
            }
            else if (hit.collider.name == "Egg")
            {
                explanation.text = Egg;
                if(!glowing) StartCoroutine("glowing_mat", Egg_Mat);
            }
            else explanation.text = Default;
        }
        
    }


    IEnumerator glowing_mat(Material m)
    {
        glowing = true;

        Color c = m.color;
        int glowing_frame = 6;
        int repeat = 4;

        float gain = 0.02f;

        for(int j = 0; j < repeat; j++)
        {
            for (int i = 0; i < glowing_frame; i++)
            {
                if (j % 2 == 0) m.color = new Color(c.r + gain * i, c.g + gain * i, c.b + gain * i);
                else m.color = new Color(c.r + gain*1.5f * (glowing_frame - i), c.g + gain * (glowing_frame - i), c.b + gain * (glowing_frame - i));
                yield return new WaitForEndOfFrame();
            }
        }
        m.color = c;
        glowing = false;
    }

    public void OnApplicationQuit()
    {
        Sealing_Mat.color = Sealing_color;
        Honey_Mat.color = Honey_color;
        Egg_Mat.color = Egg_color;
        Larva_Mat.color = Larva_color;
    }

}
