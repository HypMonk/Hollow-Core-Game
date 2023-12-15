using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameBrightness : MonoBehaviour
{
    [SerializeField] Light2D _globalLight;

    public void SetLightValue(float value)
    {
        _globalLight.intensity = value;
    }
}
