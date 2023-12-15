using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIMovementDetector : MonoBehaviour
{
    public abstract void Detect(AIMovementData aiData);
}
