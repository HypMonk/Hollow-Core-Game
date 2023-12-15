using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIContextSolver : MonoBehaviour
{
    [SerializeField]
    bool showGizmo = true;

    //gizmo parameters
    float[] interestGizmo = new float[0];
    Vector2 resultDir = Vector2.zero;
    float rayLength = 1;

    private void Start()
    {
        interestGizmo = new float[8];
    }

    public Vector2 GetDirectionToMove(List<AISteeringBehavior> behaviors, AIMovementData aiData)
    {
        float[] danger = new float[8];
        float[] interest = new float[8];

        //loop through each behavior
        foreach (AISteeringBehavior behavior in behaviors)
        {
            (danger, interest) = behavior.GetSteering(danger, interest, aiData);
        }

        //subtract danger values from interest array
        for (int i = 0; i < 8; i++)
        {
            interest[i] = Mathf.Clamp01(interest[i] - danger[i]);
        }

        interestGizmo = interest;

        //get the average direction
        Vector2 outputDir = Vector2.zero;
        for (int i = 0; i < 8; i++)
        {
            outputDir += Directions.eightDirections[i] * interest[i];
        }
        outputDir.Normalize();

        resultDir = outputDir;

        //return the selected movement direction
        return resultDir;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmo) return;

        if (Application.isPlaying)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, resultDir * rayLength);
        }
    }
}
