using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValueUpdate : MonoBehaviour
{
    [SerializeField]
    TMP_Text valueDisplay;
    [SerializeField] Slider slider;

    public void UpdateValueText()
    {
        valueDisplay.text = slider.value.ToString();
    }
}
