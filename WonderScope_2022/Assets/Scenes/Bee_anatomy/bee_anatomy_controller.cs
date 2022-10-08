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

    public TextMeshProUGUI organ_name;
    public TextMeshProUGUI explanation;

    public GameObject Cam;

    private Vector3 cam_pos;
    private float viz_btn;

    public Transform[] organ_points;

    // Start is called before the first frame update
    void Start()
    {
        explanation.text = "";
        organ_name.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        cam_pos = new Vector3(Cam.transform.position.x, Cam.transform.position.y + 5, 0);
        Transform close_organ = GetClosestOrgan(organ_points);
        Debug.Log(close_organ.name);
        if (Vector3.Distance(close_organ.position, Vector3.ProjectOnPlane(Cam.transform.position, new Vector3(0, 0, 1))) <= 3f) {
            if (close_organ.name == "org_brain")
            {
                organ_name.text = "뇌";
                explanation.text = "뇌를 제외하고 대부분의 운동기관은 신경절 의 지배를 받는다. 머리가 잘린 벌은 다리와 날개를 움직일 수 있다.";
            }
            else if (close_organ.name == "org_pharynx")
            {
                organ_name.text = "인두";
                explanation.text = "소화관 중 입과 식도 사이에 있는 부분이다.";
            }
            else if (close_organ.name == "org_salivary")
            {
                organ_name.text = "타액관";
                explanation.text = "침샘과 결합된 관으로 침을 입소으로 운반한다.";
            }
            else if (close_organ.name == "org_salivarygrand")
            {
                organ_name.text = "침샘/타액선";
                explanation.text = "먹이의 소화를 돕는 기관으로 꿀벌의 소화를 돕기 위한 타액/침을 분비하는 샘이다.";
            }
            else if (close_organ.name == "org_nerve")
            {
                organ_name.text = "신경색";
                explanation.text = "신경구 사이를 연결하며, 몸 전체에 뻗어 있는 신경계의 주 요소이다.";
            }
            else if (close_organ.name == "org_honeystock")
            {
                organ_name.text = "꿀주머니";
                explanation.text = "소화관의 일부. 식도에 이어진 부분으로서 얇은 벽이 부풀어서 먹이를 일시 저장하는 기관이라 소화작용은 일어나지 않는다.";
            }
            else if (close_organ.name == "org_throat")
            {
                organ_name.text = "식도";
                explanation.text = "꿀벌의 식도는 인두와 꿀주머니의 연결부위중에 가장 얇다. 꿀과 꽃가루를 먹는데 척추 동물처럼 강력한 근육은 필요 없다.";
            }
        } else
        {
            explanation.text = "";
            organ_name.text = "";
        }
    }

    public Transform GetClosestOrgan(Transform[] organs)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = Vector3.ProjectOnPlane(Cam.transform.position, new Vector3(0,0,1));
        foreach (Transform t in organs)
        {
            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
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
