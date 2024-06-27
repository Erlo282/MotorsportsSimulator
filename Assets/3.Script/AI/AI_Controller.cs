using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AI_Controller : MonoBehaviour
{
    Vector3 targetPosition = Vector3.zero;

    // Waypoints
    WaypointNodes currentWaypoint = null;
    WaypointNodes[] allWaypoints;
    public float maxSpeed = 25;

    // Components
    [SerializeField] private Car_Controller car_controller;

    private void Awake()
    {
        car_controller = GetComponent<Car_Controller>();
        allWaypoints = FindObjectsOfType<WaypointNodes>();
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = Vector2.zero;

        followWaypoints();

        inputVector.x = TargetAngle();
        inputVector.y = ThrottleControl(inputVector.x);

        car_controller.SetInput(inputVector);
    }

    void followWaypoints()
    {
        if (currentWaypoint == null)
        {
            currentWaypoint = ClosestWaypoint();
        }
        if (currentWaypoint != null)
        {
            targetPosition = currentWaypoint.transform.position;
            float distanceToWaypoint = (targetPosition - transform.position).magnitude;

            if (distanceToWaypoint <= currentWaypoint.minDistanceToReach)
            {
                if (currentWaypoint.maxSpeed > 0)
                {
                    maxSpeed = currentWaypoint.maxSpeed;
                }
                else
                {
                    maxSpeed = 25;
                }

                currentWaypoint = currentWaypoint.nextWaypointNode[0];
                // If multiple routes : 
                // currentWaypoint = currentWaypoint.nextWaypointNode[Random.Range(0, currentWaypoint.nextWaypointNode.Length)];
            }
        }
    }

    WaypointNodes ClosestWaypoint()
    {
        return allWaypoints.OrderBy(t => Vector3.Distance(transform.position, t.transform.position)).FirstOrDefault();
    }

    float TargetAngle()
    {
        Vector2 vectorToTarget = targetPosition - transform.position;
        vectorToTarget.Normalize();

        // Angle calculation
        float targetAngle = Vector2.SignedAngle(transform.up, vectorToTarget);
        targetAngle *= -1;
        // Weighted for if the angle is greater or lower than 45 degrees
        float steerWeight = targetAngle / 45.0f;
        steerWeight = Mathf.Clamp(steerWeight, -1.0f, 1.0f);
        return steerWeight;
    }

    float ThrottleControl(float inputX)
    {
        if (car_controller.GetVelo() > maxSpeed)
        {
            return 0;
        }
        // Apply throttle based on amount of turn
        return 1.05f - Mathf.Abs(inputX) / 1.0f;
    }
}
