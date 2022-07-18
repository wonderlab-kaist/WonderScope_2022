using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;


public class CaveGameController : MonoBehaviour
{
    public GameObject Cam;
    //public Transform Crystals;
    private GameObject Crystal;
    //Transform[] allChildren = GetComponentsInChildren<Transform>();

    //private int score;
    //private int distance;
    private int count = 0;
    //private float camdist;

    //public ParticleSystem twinkle;
    public GameObject twinkle;
    public TextMeshProUGUI score;

    // Start is called before the first frame update
    void Start()
    {
        //score = 0;
        //distance = 30;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            var ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                var rig = hitInfo.collider.GetComponent<Rigidbody>();
                if (rig != null)
                {
                    print(rig.name);
                    Crystal = GameObject.Find(rig.name);
                    Instantiate(twinkle, Crystal.transform.position, Quaternion.identity);
                    Crystal.SetActive(false);
                    count++;
                }
            }
        }

        if(Input.touchCount == 3) SceneManager.LoadScene(0, LoadSceneMode.Single);

        score.text = "SCORE: " + Convert.ToString(count);

        //if (Input.GetMouseButtonDown(0))
        //{
        //    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hitInfo;
        //    if (Physics.Raycast(ray, out hitInfo))
        //    {
        //        var rig = hitInfo.collider.GetComponent<Rigidbody>();
        //        if (rig != null)
        //        {
        //            print(rig.name);
        //            Crystal = GameObject.Find(rig.name);
        //            Instantiate(twinkle, Crystal.transform.position, Quaternion.identity);
        //            Crystal.SetActive(false);
        //        }
        //    }
        //}


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
        //foreach (Transform childCrystal in Crystals)
        //{
        //    Crystal = GameObject.Find(childCrystal.name);
        //    camdist = Mathf.Sqrt(Mathf.Pow((Crystal.transform.position.x - Cam.transform.position.x),2)+ Mathf.Pow((Crystal.transform.position.z - Cam.transform.position.z), 2));

        //    //if ((Vector3.Distance(Crystal.transform.position, Cam.transform.position) < distance) && Crystal.activeSelf)
        //    if ((camdist < distance) && Crystal.activeSelf)
        //        {
        //        //_editableshape.position = crystal.transform.position;

        //        Instantiate(twinkle, Crystal.transform.position, Quaternion.identity);
        //        //twinkle.transform.position = crystal.transform.position;
        //        //twinkle.play();
        //        Crystal.SetActive(false);

        //    }
        //    //crystal = gameobject.find(childcrystal.name);
        //    //print(crystal.name);
        //    ////print(childcrystal.);
        //}
    }
}
