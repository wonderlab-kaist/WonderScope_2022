using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SizeManager : MonoBehaviour
{

    public Slider sizeSlider;
    public SpriteRenderer rulerImg;
    public TextMeshProUGUI scaleTxt;

    public void ChangeSize()
    {
        float scale = sizeSlider.value;
        rulerImg.transform.localScale = new Vector3(scale, scale, 1);
    }

    void Update()
    {
        scaleTxt.text = Convert.ToString(sizeSlider.value);
    }

}
