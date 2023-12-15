using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISeekBehavior : AISteeringBehavior
{
    float targetReachedThreshold = .5f;

    [SerializeField]
    bool showGizmo = true;

    bool reachedLastTarget = true;

    //gizmo parameters
    Vector2 targetPositionCached;
    float[] interestsTemp;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIMovementData aiData)
    {
        if (aiData.freeMovement && aiData.targetPosition != Vector2.zero) 
        {
            
            //cache the last position only if we still see the target
            if (aiData.targetPosition != Vector2.zero) targetPositionCached = aiData.targetPosition;

            //first check if we have reached the target
            if (Vector2.Distance(transform.position, targetPositionCached) < targetReachedThreshold)
            {
                aiData.targetPosition = Vector2.zero;
                return (danger, interest);
            }

            //if we havent yet reached the target do the main logic of finding the interest directions
            Vector2 dirToFreeTarget = (targetPositionCached - (Vector2)transform.position);
            for (int i = 0; i < interest.Length; i++)
            {
                float result = Vector2.Dot(dirToFreeTarget.normalized, Directions.eightDirections[i]);

                //accept only directions at the less than 90 degrees to the target direction
                if (result > 0)
                {
                    float valueToPutIn = result;
                    if (valueToPutIn > interest[i])
                    {
                        interest[i] = valueToPutIn;
                    }
                }
            }
            interestsTemp = interest;
            return (danger, interest);
        }

        //if we dont have a target stop seeking
        if (reachedLastTarget)
        {
            if (aiData.target == null) return (danger, interest); else reachedLastTarget = false;
        }
        //cache the last position only if we still see the target
        if (aiData.target != null) targetPositionCached = aiData.target.position;

        //first check if we have reached the target
        if (Vector2.Distance(transform.position, targetPositionCached) < targetReachedThreshold)
        {

            reachedLastTarget = true;
            aiData.target = null;
            return (danger, interest);
        }

        //if we havent yet reached the target do the main logic of finding the interest directions
        Vector2 dirToTarget = (targetPositionCached - (Vector2)transform.position);
        for (int i = 0; i < interest.Length; i++)
        {
            float result = Vector2.Dot(dirToTarget.normalized, Directions.eightDirections[i]);

            //accept only directions at the less than 90 degrees to the target direction
            if (result > 0)
            {
                float valueToPutIn = result;
                if (valueToPutIn > interest[i])
                {
                    interest[i] = valueToPutIn;
                }
            }
        }
        interestsTemp = interest;
        return (danger, interest);
    }

    private void OnDrawGizmos()
    {
        if (showGizmo == false) return;

        if (Application.isPlaying && interestsTemp != null)
        {
            if (interestsTemp != null)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < interestsTemp.Length; i++)
                {
                    Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * interestsTemp[i]);
                }
                if (reachedLastTarget == false)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(targetPositionCached, .1f);
                }
            }
        }
    }
}
