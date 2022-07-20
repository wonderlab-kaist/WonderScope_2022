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
    public GameObject up;
    public GameObject down;
    public GameObject left;
    public GameObject right;

    private GameObject Crystal;
    private int score=0;
    private float distance=70f;
    private float cryDist;
    private List<Vector3> closeCrystals = new List<Vector3>();

    void Start()
    {
        up.SetActive(false);
        down.SetActive(false);
        left.SetActive(false);
        right.SetActive(false);
        closeCrystals.Add(Vector3.zero);

    }

    void Update()
    {
        scoreTxt.text = Convert.ToString(score);

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

            cryDist = Vector3.Distance(Crystal.transform.position, Cam.transform.position);
            if (cryDist < distance && Crystal.activeSelf)
            {
                if (Crystal.tag != "EffectOn")
                {
                    GameObject effect = Instantiate(effect2, Crystal.transform.position, Quaternion.identity);
                    effect.gameObject.name = "Effect_" + childCrystal.name;

                    Crystal.tag = "EffectOn";
                }
            }
            else if (cryDist >= distance && Crystal.activeSelf)
            {
                Destroy(GameObject.Find("Effect_" + childCrystal.name));
                Crystal.tag = "EffectOff";
            }

            if (Crystal.activeSelf && cryDist > (distance + 10f))
            {
                if (closeCrystals[0] == Vector3.zero)
                {
                    closeCrystals[0] = Crystal.transform.position;
                    closeCrystals.Add(Crystal.transform.position);
                    closeCrystals.Add(Crystal.transform.position);
                }
                else if (cryDist < Vector3.Distance(closeCrystals[0], Cam.transform.position))
                {
                    closeCrystals[2] = closeCrystals[1];
                    closeCrystals[1] = closeCrystals[0];
                    closeCrystals[0] = Crystal.transform.position;
                }
                else if ((Vector3.Distance(closeCrystals[0], Cam.transform.position) <= cryDist) && (Vector3.Distance(closeCrystals[1], Cam.transform.position) > cryDist))
                {
                    closeCrystals[2] = closeCrystals[1];
                    closeCrystals[1] = Crystal.transform.position;
                }
                else if ((Vector3.Distance(closeCrystals[1], Cam.transform.position) <= cryDist) && (Vector3.Distance(closeCrystals[2], Cam.transform.position) > cryDist))
                {
                    closeCrystals[2] = Crystal.transform.position;
                }
            }
        }


        // 방향 구하기
        foreach (Vector3 crystal in closeCrystals)
        {
            //Vector3 v = Cam.transform.forward-crystal;
            //Vector3 v = Cam.transform.forward*10- crystal;

            //Vector3 v = Cam.transform.position - crystal;
            //Vector3 v = crystal - Cam.transform.position;

            //var degree = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            //float degree = Vector3.Angle(Cam.transform.forward, crystal);


            float degree = Vector3.SignedAngle(transform.up, crystal - Cam.transform.position, -transform.forward);
            if (degree >= -45 && degree < 45)
            {
                right.SetActive(true);
                up.SetActive(false);
                left.SetActive(false);
                down.SetActive(false);
            }
            else if (degree >= 45 && degree < 135)
            {
                right.SetActive(false);
                up.SetActive(true);
                left.SetActive(false);
                down.SetActive(false);
            }
            else if ((degree >= 135 && degree <= 180) || (degree >= -180 && degree < -135))
            {
                right.SetActive(false);
                up.SetActive(false);
                left.SetActive(true);
                down.SetActive(false);
            }
            else if (degree >= -135 && degree < -45)
            {
                right.SetActive(false);
                up.SetActive(false);
                left.SetActive(false);
                down.SetActive(true);
            }
        }
        
    }
}
