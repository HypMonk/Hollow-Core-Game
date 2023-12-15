using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SliderValueDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text valueDisplay;
    [SerializeField] Slider slider;

    private void Start()
    {
        valueDisplay.text = Mathf.RoundToInt(slider.value * 100).ToString() + "%";
    }

    public void UpdateValue()
    {
        valueDisplay.text = Mathf.RoundToInt(slider.value * 100).ToString() + "%";
    }
}
