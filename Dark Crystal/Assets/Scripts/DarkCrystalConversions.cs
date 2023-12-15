using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkCrystalConversions
{
    public float AttackSpeed(float speed, float attackSpeed)
    {
        //Debug.Log("AttackSpeed: " + attackSpeed + " Player Speed: " + speed + " Calculated: " + (speed/attackSpeed));
        return speed/attackSpeed;
    }

    public float PercentageValue(float value)
    {
        return value * .01f;
    }
}
