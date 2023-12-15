using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovementData : MonoBehaviour
{
    public Collider2D[] obstacles = null;

    public Transform target;

    public bool freeMovement;
    public Vector2 targetPosition;

}
