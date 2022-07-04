using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GetValue : MonoBehaviour
{
    public Calibrating_init calibrating;
    public TMP_InputField trialInput, distInput;
    public GameObject settingPanel, calibratingPanel;

    public void ValueUpdate()
    {
        settingPanel.SetActive(false);
        calibratingPanel.SetActive(true);
        calibrating.maxTrial = int.Parse(trialInput.text);
        calibrating.standardDistance = float.Parse(distInput.text);
    }
}
