using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;


public class CaveGameController : MonoBehaviour
{
    public aarcall aar;

    public GameObject Cam;
    public Transform Crystals;
    public GameObject effect1;
    public GameObject effect2;
    public AudioSource crystalSound;
    public TextMeshProUGUI scoreTxt;

    private GameObject Crystal;
    private int score=0;
    private float distance=70f;


    void Update()
    {
        scoreTxt.text = "SCORE: " + Convert.ToString(score);

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            var ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                var rig = hitInfo.collider.GetComponent<Rigidbody>();
                if (rig != null)
                {
                    Crystal = GameObject.Find(rig.name);
                    crystalSound.time = 1;
                    crystalSound.Play();
                    Instantiate(effect1, Crystal.transform.position, Quaternion.identity);
                    Destroy(GameObject.Find("Effect_" + Crystal.name));
                    Crystal.tag = "EffectOff";
                    Crystal.SetActive(false);
                    score++;
                    aar.vibrate_phone();
                }
            }
        }

        if (Input.touchCount == 3) SceneManager.LoadScene(0, LoadSceneMode.Single);


        #region ForDebugging
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 touchpos = Input.mousePosition;
        //    var ray = Camera.main.ScreenPointToRay(touchpos);
        //    RaycastHit hitInfo;
        //    if (Physics.Raycast(ray, out hitInfo))
        //    {
        //        var rig = hitInfo.collider.GetComponent<Rigidbody>();
        //        if (rig != null)
        //        {
        //            Crystal = GameObject.Find(rig.name);
        //            crystalSound.time = 1;
        //            crystalSound.Play();
        //            Instantiate(effect1, Crystal.transform.position, Quaternion.identity);
        //            //Instantiate(twinkle, touchpos, Quaternion.identity);
        //            Destroy(GameObject.Find("Effect_" + Crystal.name));
        //            Crystal.tag = "EffectOff";
        //            Crystal.SetActive(false);
        //            score++;
        //        }
        //    }
        //}
        #endregion


        foreach (Transform childCrystal in Crystals)
        {
            Crystal = GameObject.Find(childCrystal.name);
            if (Vector3.Distance(Crystal.transform.position, Cam.transform.position) < distance && Crystal.activeSelf)
            {
                if (Crystal.tag != "EffectOn")
                {
                    GameObject effect = Instantiate(effect2, Crystal.transform.position, Quaternion.identity);
                    effect.gameObject.name = "Effect_" + childCrystal.name;

                    Crystal.tag = "EffectOn";
                }
            }
            else if (Vector3.Distance(Crystal.transform.position, Cam.transform.position) >= distance && Crystal.activeSelf)
            {
                Destroy(GameObject.Find("Effect_" + childCrystal.name));
                Crystal.tag = "EffectOff";
            }
        }
    }
}
