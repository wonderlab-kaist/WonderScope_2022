using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SizeManager : MonoBehaviour
{

    public Slider sizeSlider;
    // public Sprite rulerImg;
    // public Image rulerImg;
    public SpriteRenderer rulerImg;
    public TextMeshProUGUI scaleTxt;

    public void ChangeSize()
    {
        float scale = sizeSlider.value;
        // rulerImg.texture.Reinitialize(scale, scale);
        // rulerImg.texture.Apply();

        // rulerImg.rectTransform.localScale = new Vector3(rulerImg.Height*scale,scale,1);
        rulerImg.transform.localScale = new Vector3(scale, scale, 1);


    }

    void Update()
    {
        scaleTxt.text = Convert.ToString(sizeSlider.value);
    }

    // void Start()
    // {

    // }

    // void Update()
    // {

    // }
}
