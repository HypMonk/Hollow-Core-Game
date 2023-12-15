using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovementObstacleDetector : AIMovementDetector
{
    float detectionRadius = 2;

    public LayerMask layerMask;

    [SerializeField]
    bool showGizmo = true;

    Collider2D[] colliders;

    public override void Detect(AIMovementData aiData)
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, layerMask);
        aiData.obstacles = colliders;
    }

    void OnDrawGizmos()
    {
        if(showGizmo == false) { return; }

        if (Application.isPlaying && colliders != null)
        {
            Gizmos.color = Color.red;
            foreach (Collider2D obstacleCollider in colliders)
            {
                Gizmos.DrawSphere(obstacleCollider.transform.position, .2f);
            }
        }
    }
}
