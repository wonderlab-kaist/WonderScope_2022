using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class bee_anatomy_controller : MonoBehaviour
{
    public GameObject bee_anatomy_default;
    public GameObject bee_anatomy_resp;
    public GameObject bee_anatomy_digest;
    public GameObject bee_anatomy_nerve;

    public TextMeshProUGUI anatomy_sys_txt;
    public TextMeshProUGUI explanation;

    private float viz_btn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void set_visualize(float viz)
    {
        viz_btn = viz;
        if (viz == 0)
        {
            bee_anatomy_default.SetActive(true);
            bee_anatomy_resp.SetActive(true);
            bee_anatomy_digest.SetActive(true);
            bee_anatomy_nerve.SetActive(true);
        }
        else if (viz == 1)
        {
            bee_anatomy_default.SetActive(true);
            bee_anatomy_resp.SetActive(true);
            bee_anatomy_digest.SetActive(false);
            bee_anatomy_nerve.SetActive(false);
        }
        else if (viz ==2)
        {
            bee_anatomy_default.SetActive(true);
            bee_anatomy_resp.SetActive(false);
            bee_anatomy_digest.SetActive(true);
            bee_anatomy_nerve.SetActive(false);
        }
        else if (viz == 3)
        {
            bee_anatomy_default.SetActive(true);
            bee_anatomy_resp.SetActive(false);
            bee_anatomy_digest.SetActive(false);
            bee_anatomy_nerve.SetActive(true);
        }
    }
}
