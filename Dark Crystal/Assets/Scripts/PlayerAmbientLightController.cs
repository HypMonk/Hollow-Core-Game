using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerAmbientLightController : MonoBehaviour
{
    [SerializeField]
    PlayerStats playerStats;
    [SerializeField]
    Light2D ambientLight;
    [SerializeField]
    CircleCollider2D lightCollider;
    DarkCrystalConversions darkCrystalConversions;

    const float _MAXRADIUS = 2f;

    private void Start()
    {
        darkCrystalConversions = new DarkCrystalConversions();
    }

    // Update is called once per frame
    void Update()
    {
        ambientLight.pointLightOuterRadius = _MAXRADIUS * darkCrystalConversions.PercentageValue(playerStats.LightLevel);
        lightCollider.radius = ambientLight.pointLightOuterRadius;
    }
}
