using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveGameController : MonoBehaviour
{
    public GameObject Cam;
    public Transform Crystals;
    private GameObject Crystal;
    //Transform[] allChildren = GetComponentsInChildren<Transform>();

    private int score;
    private int distance;
    private int count = 0;

    //public ParticleSystem twinkle;
    public GameObject twinkle;


    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        distance = 5;
    }

    // Update is called once per frame
    void Update()
    {
        ////ParticleSystem pinkSystem = GetComponent<ParticleSystem>();
        ////pinkSystem.transform.position= Crystal.transform.position;
        ////ParticleSystem.ShapeModule _editableShape = twinkle.shape;
        //Crystal = GameObject.Find("crystal_05 (37)");
        //twinkle.transform.position = Crystal.transform.position;

        ////_editableShape.position = Crystal.transform.position;
        //if (Crystal.activeSelf)
        //{
        //    twinkle.Play();
        //    Crystal.SetActive(false);

        //}
        //if (Crystals != null)
        //{
        //    foreach (Transform childCrystal in Crystals)
        //    {
        //        Crystal = GameObject.Find(childCrystal.name);
        //        //Instantiate(twinkle, Crystal.transform.position, Quaternion.identity);

        //        if (Crystal.activeSelf)
        //        {
        //            //_editableShape.position = Crystal.transform.position;

        //            Instantiate(twinkle, Crystal.transform.position, Quaternion.identity);

        //            //twinkle.transform.position = Crystal.transform.position;
        //            //twinkle.Play();
        //            Crystal.SetActive(false);

        //        }
        //    }
        //}

        // crystal의 child들 다 선회해서 active true인 것들 중에 cam이랑 거리값이 10 이내인 것?은 효과 주고 active false로 바꾸기. 그리고 score +1
        foreach (Transform childCrystal in Crystals)
        {
            Crystal = GameObject.Find(childCrystal.name);
            if ((Vector3.Distance(Crystal.transform.position, Cam.transform.position) < distance) && Crystal.activeSelf)
            {
                //_editableshape.position = crystal.transform.position;

                Instantiate(twinkle, Crystal.transform.position, Quaternion.identity);
                //twinkle.transform.position = crystal.transform.position;
                //twinkle.play();
                Crystal.SetActive(false);

            }
            //crystal = gameobject.find(childcrystal.name);
            //print(crystal.name);
            ////print(childcrystal.);
        }
    }
}
