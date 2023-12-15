using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIObstacleAvoidanceBehavior : AISteeringBehavior
{
    [SerializeField]
    float radius = 2, myColliderSize;

    [SerializeField]
    bool showGizmo = true;

    //gizmo parameters
    float[] dangersResultTemp = null;

    //abstract methods
    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIMovementData aiData)
    {
        foreach (Collider2D obstacleCollider in aiData.obstacles)
        {
            //prevent null exception when removing enemies on death
            if (obstacleCollider == null) continue;

            Vector2 dirToObstacle = obstacleCollider.ClosestPoint(transform.position) - (Vector2)transform.position;
            float distanceToObstacle = dirToObstacle.magnitude;

            //calculate weight based on the distance
            //This line of code is a way to short hand an if else statement 
            //weight  = ( distancetoobstacle <= mycollidersize(if) then(?) weight = 1 else(:) (radius - distanceToObstacle) / radius))
            float weight = distanceToObstacle <= myColliderSize ? 1 : (radius - distanceToObstacle) / radius;

            Vector2 dirToObstacleNormalized = dirToObstacle.normalized;

            //add obstacle parameters to the danger array
            for (int i = 0; i < Directions.eightDirections.Count; i++)
            {
                float result = Vector2.Dot(dirToObstacleNormalized, Directions.eightDirections[i]);

                float valueToPutIn = result * weight;

                //override value only if it is higher than the current one stored in the danger array
                if (valueToPutIn > danger[i])
                {
                    danger[i] = valueToPutIn;
                }
            }
        }
        dangersResultTemp = danger;
        return (danger, interest);
    }

    private void OnDrawGizmos()
    {
        if (showGizmo == false)
            return;

        if(Application.isPlaying && dangersResultTemp != null)
        {
            if (dangersResultTemp != null)
            {
                Gizmos.color = Color.red;
                for (int i = 0; i < dangersResultTemp.Length; i++)
                {
                    Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * dangersResultTemp[i]);
                }
            }
            else
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(transform.position, radius);
            }
        }
    }
}

public static class Directions
{
    public static List<Vector2> eightDirections = new List<Vector2>
    {
        new Vector2(0,1).normalized,
        new Vector2(1,1).normalized,
        new Vector2(1,0).normalized,
        new Vector2(1,-1).normalized,
        new Vector2(0,-1).normalized,
        new Vector2(-1,-1).normalized,
        new Vector2(-1,0).normalized,
        new Vector2(-1,1).normalized,
    };
}
