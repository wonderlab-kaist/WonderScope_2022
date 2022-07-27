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
    public GameObject cry_andalusite;
    public GameObject cry_emerald;
    public GameObject cry_kunzite;
    public GameObject cry_peridot;
    public GameObject cry_ruby;
    public GameObject cry_yel;

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

        cry_andalusite.SetActive(false);
        cry_emerald.SetActive(false);
        cry_kunzite.SetActive(false);
        cry_peridot.SetActive(false);
        cry_ruby.SetActive(false);
        cry_yel.SetActive(false);

        closeCrystals.Add(Vector3.zero);

    }

    void Update()
    {
        scoreTxt.text = Convert.ToString(score);

        //if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        //{
        //    Touch touch = Input.GetTouch(0);
        //    var ray = Camera.main.ScreenPointToRay(touch.position);
        //    RaycastHit hitInfo;
        //    if (Physics.Raycast(ray, out hitInfo))
        //    {
        //        var rig = hitInfo.collider.GetComponent<Rigidbody>();
        //        if (rig != null)
        //        {
        //            Crystal = GameObject.Find(rig.name);
        //            crystalSound.time = 1;
        //            crystalSound.Play();

        //            Vector3 preloc = Crystal.transform.position;
        //            Instantiate(effect1, Crystal.transform.position, Quaternion.identity);
        //            Destroy(GameObject.Find("Effect_" + Crystal.name));
        //            Vector2 loc = new Vector2(0, 4);
        //            float theta = (Cam.transform.eulerAngles.y - 90) * Mathf.Deg2Rad;
        //            Vector3 rotloc = new Vector3(Cam.transform.position.x - loc.x * Mathf.Cos(theta) + loc.y * Mathf.Sin(theta), 38, Cam.transform.position.z + loc.x * Mathf.Sin(theta) + loc.y * Mathf.Cos(theta));
        //            StartCoroutine(move(Crystal, preloc, rotloc, Cam.transform.eulerAngles.y - 90));
        //            Crystal.tag = "EffectOff";
        //            //Crystal.SetActive(false);
        //            score++;
        //            aar.vibrate_phone();
        //        }
        //    }
        //}
        //if (Input.touchCount == 3) SceneManager.LoadScene(0, LoadSceneMode.Single);


        #region ForDebugging
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 touchpos = Input.mousePosition;
            var ray = Camera.main.ScreenPointToRay(touchpos);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                var rig = hitInfo.collider.GetComponent<Rigidbody>();
                if (rig != null)
                {
                    Crystal = GameObject.Find(rig.name);
                    crystalSound.time = 1;
                    crystalSound.Play();

                    Vector3 preloc = Crystal.transform.position;
                    Instantiate(effect1, Crystal.transform.position, Quaternion.identity);
                    Destroy(GameObject.Find("Effect_" + Crystal.name));
                    //Vector3 centerloc = new Vector3(Cam.transform.position.x, preloc.y + 18, Cam.transform.position.z + 5);

                    Vector2 loc = new Vector2(0, 5);
                    float theta = (Cam.transform.eulerAngles.y - 90) * Mathf.Deg2Rad;
                    Vector3 rotloc = new Vector3(Cam.transform.position.x - loc.x * Mathf.Cos(theta) + loc.y * Mathf.Sin(theta), Cam.transform.position.y - 10, Cam.transform.position.z + loc.x * Mathf.Sin(theta) + loc.y * Mathf.Cos(theta));
                    //Debug.Log(preloc.y + 18);
                    //Debug.Log(Cam.transform.position.y);
                    //move to center
                    StartCoroutine(move(Crystal, preloc, rotloc, Cam.transform.eulerAngles.y - 90));
                    Crystal.tag = "EffectOff";
                    //Crystal.SetActive(false);
                    score++;
                }
            }
        }
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

    IEnumerator move(GameObject crystal, Vector3 prev, Vector3 cur, float theta)
    {
        if (Convert.ToString(crystal.GetComponent<Renderer>().sharedMaterial.name) == "crystal_andalusite") cry_andalusite.SetActive(true);
        else if (Convert.ToString(crystal.GetComponent<Renderer>().sharedMaterial.name) == "crystal_emerald") cry_emerald.SetActive(true);
        else if (Convert.ToString(crystal.GetComponent<Renderer>().sharedMaterial.name) == "crystal_kunzite") cry_kunzite.SetActive(true);
        else if (Convert.ToString(crystal.GetComponent<Renderer>().sharedMaterial.name) == "crystal_peridot") cry_peridot.SetActive(true);
        else if (Convert.ToString(crystal.GetComponent<Renderer>().sharedMaterial.name) == "crystal_ruby") cry_ruby.SetActive(true);
        else if (Convert.ToString(crystal.GetComponent<Renderer>().sharedMaterial.name) == "crystal_yel_sapp") cry_yel.SetActive(true);
        Quaternion q = Quaternion.Euler(90, theta, -90);
        if (crystal.transform.position != cur)
        {
            for (float t = 0f; t <= 1f; t += 0.1f)
            {
                crystal.transform.position = Vector3.Lerp(prev, cur, t);
                crystal.transform.rotation = Quaternion.Lerp(Quaternion.identity, q, t);
                yield return new WaitForEndOfFrame();
            }

        }
        
       

        crystal.GetComponent<MeshCollider>().enabled = false;

        for (float timer = 4; timer >= 0; timer -= Time.deltaTime)
        {
            //update location
            Vector2 loc = new Vector2(0, 5);
            float alpha = (Cam.transform.eulerAngles.y - 90) * Mathf.Deg2Rad;
            crystal.transform.position = new Vector3(Cam.transform.position.x - loc.x * Mathf.Cos(alpha) + loc.y * Mathf.Sin(alpha), Cam.transform.position.y - 10, Cam.transform.position.z + loc.x * Mathf.Sin(alpha) + loc.y * Mathf.Cos(alpha));
            crystal.transform.rotation = Quaternion.Euler(90, Cam.transform.eulerAngles.y - 90, -90);

            //detect touch input
            if (Input.GetMouseButtonDown(0))
            //if (Input.touchCount == 1)
            {
                if (Convert.ToString(crystal.GetComponent<Renderer>().sharedMaterial.name) == "crystal_andalusite") cry_andalusite.SetActive(false);
                else if (Convert.ToString(crystal.GetComponent<Renderer>().sharedMaterial.name) == "crystal_emerald") cry_emerald.SetActive(false);
                else if (Convert.ToString(crystal.GetComponent<Renderer>().sharedMaterial.name) == "crystal_kunzite") cry_kunzite.SetActive(false);
                else if (Convert.ToString(crystal.GetComponent<Renderer>().sharedMaterial.name) == "crystal_peridot") cry_peridot.SetActive(false);
                else if (Convert.ToString(crystal.GetComponent<Renderer>().sharedMaterial.name) == "crystal_ruby") cry_ruby.SetActive(false);
                else if (Convert.ToString(crystal.GetComponent<Renderer>().sharedMaterial.name) == "crystal_yel_sapp") cry_yel.SetActive(false);

                Vector3 disPt = new Vector3(crystal.transform.position.x - 50, crystal.transform.position.y, crystal.transform.position.z);
                Vector3 centerPt = crystal.transform.position;
                if (crystal.transform.position != disPt)
                {
                    for (float t = 0f; t <= 1f; t += 0.05f)
                    {
                        crystal.transform.position = Vector3.Lerp(centerPt, disPt, t);
                        //yield return new WaitForSeconds(0.01f);
                        yield return new WaitForEndOfFrame();
                    }
                }
                crystal.transform.rotation = Quaternion.Euler(0, 0, 0);
                crystal.transform.position = prev;
                crystal.SetActive(false);
                yield break;
            }
            yield return null;
        }

        if (Convert.ToString(crystal.GetComponent<Renderer>().sharedMaterial.name) == "crystal_andalusite") cry_andalusite.SetActive(false);
        else if (Convert.ToString(crystal.GetComponent<Renderer>().sharedMaterial.name) == "crystal_emerald") cry_emerald.SetActive(false);
        else if (Convert.ToString(crystal.GetComponent<Renderer>().sharedMaterial.name) == "crystal_kunzite") cry_kunzite.SetActive(false);
        else if (Convert.ToString(crystal.GetComponent<Renderer>().sharedMaterial.name) == "crystal_peridot") cry_peridot.SetActive(false);
        else if (Convert.ToString(crystal.GetComponent<Renderer>().sharedMaterial.name) == "crystal_ruby") cry_ruby.SetActive(false);
        else if (Convert.ToString(crystal.GetComponent<Renderer>().sharedMaterial.name) == "crystal_yel_sapp") cry_yel.SetActive(false);

        Vector3 disappearPt = new Vector3(crystal.transform.position.x - 50, crystal.transform.position.y, crystal.transform.position.z);
        Vector3 ctrPt = crystal.transform.position;
        if (crystal.transform.position != disappearPt)
        {
            for (float t = 0f; t <= 1f; t += 0.05f)
            {
                crystal.transform.position = Vector3.Lerp(ctrPt, disappearPt, t);
                //yield return new WaitForSeconds(0.01f);
                yield return new WaitForEndOfFrame();
            }
        }
        crystal.transform.rotation = Quaternion.Euler(0, 0, 0);
        crystal.transform.position = prev;
        crystal.SetActive(false);
    }
}
