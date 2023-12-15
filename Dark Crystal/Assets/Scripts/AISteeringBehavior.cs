using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AISteeringBehavior : MonoBehaviour
{
    public abstract (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIMovementData aiData);
}
