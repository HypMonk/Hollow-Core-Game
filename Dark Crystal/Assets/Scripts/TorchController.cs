using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchController : MonoBehaviour
{
    [SerializeField]
    bool isStationary;
    [SerializeField]
    bool isOn;
    [SerializeField]
    bool canDecay;
    [SerializeField]
    Light2D areaLight;
    [SerializeField]
    CircleCollider2D areaLightCollider;

    [SerializeField]
    CapsuleCollider2D[] torchColliders = new CapsuleCollider2D[2];

    DarkCrystalConversions darkCrystalConversions;

    [HideInInspector]
    public bool beingCarried;
    GameObject _carrier;

    [HideInInspector]
    public Torch torch;
    float lightLevel;

    public class Torch
    {
        public float maxOuterRadius, maxInnerRadius, maxLightLevel, decayRate;
        public bool isStationary;

        public Torch(float MaxOuterRadius, float MaxInnerRadius, float MaxLightLevel, float DecayRate, bool IsStationary)
        {
            maxOuterRadius = MaxOuterRadius;
            maxInnerRadius = MaxInnerRadius;
            maxLightLevel = MaxLightLevel;
            decayRate = DecayRate;
            isStationary = IsStationary;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        darkCrystalConversions = new DarkCrystalConversions();

        if (isStationary)
        {
            torch = new Torch(5, 1, 100, 1, isStationary);
        } else
        {
            torch = new Torch(2.5f, .5f, 100, 1, isStationary);
        }

        if (!isOn)
        {
            lightLevel = 0;
            areaLight.gameObject.SetActive(false);
        } else
        {
            lightLevel = darkCrystalConversions.PercentageValue(torch.maxLightLevel);
            areaLight.gameObject.SetActive(true);
        }

        areaLight.pointLightOuterRadius = torch.maxOuterRadius * lightLevel;
        areaLight.pointLightInnerRadius = torch.maxInnerRadius * lightLevel;
        areaLightCollider.radius = areaLight.pointLightOuterRadius;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (canDecay && isOn)
        {
            areaLight.pointLightOuterRadius = torch.maxOuterRadius * lightLevel;
            areaLight.pointLightInnerRadius = torch.maxInnerRadius * lightLevel;

            lightLevel -= darkCrystalConversions.PercentageValue(torch.decayRate) * Time.deltaTime;

            if (lightLevel <= .20)
            {
                //Debug.Log("Fuse Blown");
                
                ToggleLight();

                lightLevel = 0;
            }
        } else if (!canDecay && isOn)
        {
            lightLevel = darkCrystalConversions.PercentageValue(torch.maxLightLevel);
            areaLight.pointLightOuterRadius = torch.maxOuterRadius * lightLevel;
            areaLight.pointLightInnerRadius = torch.maxInnerRadius * lightLevel;
        }

        areaLightCollider.radius = areaLight.pointLightOuterRadius;

    }

    private void LateUpdate()
    {
        if (beingCarried)
        {
            transform.position = _carrier.transform.position;
        } 
    }

    void ToggleLight()
    {
        if (!isOn)
        {
            lightLevel = darkCrystalConversions.PercentageValue(torch.maxLightLevel);
            areaLight.gameObject.SetActive(true);
            isOn = true;
        }
        else
        {
            areaLight.gameObject.SetActive(false);
            isOn = false;
        }
    }

    //Torch recieves Power (for now just from projectiles)
    public void ToggleLight(float LightLevel)
    {
        if (!isStationary && !isOn) { return; }

        lightLevel += darkCrystalConversions.PercentageValue(LightLevel);

        if (lightLevel >= darkCrystalConversions.PercentageValue(torch.maxLightLevel)) 
        { 
            lightLevel = darkCrystalConversions.PercentageValue(torch.maxLightLevel); 
        }

        if (!isOn)
        {
            areaLight.gameObject.SetActive(true);
            isOn = true;
        }
    }

    public void PickedUp(GameObject Carrier)
    {
        _carrier = Carrier;
        beingCarried = true;
        torchColliders[0].enabled = false;
    }

    public void Dropped()
    {
        beingCarried = false;
        _carrier = null;
        torchColliders[0].enabled = true;
    }

    public void LoseLight(float value)
    {
        lightLevel -= darkCrystalConversions.PercentageValue(value);
    }
}
