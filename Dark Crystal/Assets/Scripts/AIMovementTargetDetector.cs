using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovementTargetDetector : AIMovementDetector
{
    float targetDetectionRange = 5;

    public LayerMask obstaclesLayerMask, targetLayerMask;

    [SerializeField]
    bool showGizmos = true;

    Transform targetColliderTransform;

    public override void Detect(AIMovementData aiData)
    {
        if (aiData.freeMovement && aiData.targetPosition != Vector2.zero)
        {
            //check if you see target
            Vector2 direction = (aiData.targetPosition - (Vector2)transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, targetDetectionRange, obstaclesLayerMask);

            //make sure that the collider we see is on the "target" layer
            if (hit.collider != null)
            {
                aiData.targetPosition = Vector2.zero;
            }
            else
            {
                Debug.DrawRay(transform.position, direction * targetDetectionRange, Color.magenta);
            }
            return;
        }

        //check if target is near
        Collider2D targetCollider = Physics2D.OverlapCircle(transform.position, targetDetectionRange, targetLayerMask);

        if (targetCollider != null)
        {

            //check if you see target
            Vector2 direction = (targetCollider.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, targetDetectionRange, obstaclesLayerMask);
            
            //make sure that the collider we see is on the "target" layer
            if (hit.collider != null && (targetLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
            {
                Debug.DrawRay(transform.position, direction * targetDetectionRange, Color.magenta);
                targetColliderTransform = targetCollider.transform;
            } else
            {
                targetColliderTransform = null;
            }
        } else
        {
            targetColliderTransform = null;
        }
        aiData.target = targetColliderTransform;
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmos == false) { return; }

        Gizmos.DrawWireSphere(transform.position, targetDetectionRange);

        if (targetColliderTransform == null) { return; }

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(targetColliderTransform.position, .3f);
    }

}
