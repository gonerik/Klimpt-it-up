using System;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public GameObject[] waypoints;
    public float speed = 2f;
    public float reachDistance = 0.1f;
    private int currentWaypointIndex = 0;
    private bool isReversing = false;
    private bool isStopped = false;
    private float stopTimer = 0f;
    private float puddleImmunityTimer = 0f;
    public Action slip;

    private void FixedUpdate()
    {
        if (puddleImmunityTimer > 0f) {
            puddleImmunityTimer -= Time.deltaTime;
        }

        if (isStopped)
        {
            stopTimer -= Time.deltaTime;
            if (stopTimer <= 0)
            {
                isStopped = false;
            }
            return;
        }

        Move();
    }

    public void StopMovement(float duration) {
        isStopped = true;
        stopTimer = duration;
        if (puddleImmunityTimer > 0f) {
            isStopped = false;
        } else puddleImmunityTimer = 10f;
    }

    public void Move() {
        if (waypoints.Length == 0) return;

        Vector3 targetPosition = waypoints[currentWaypointIndex].transform.position;
        Vector3 movementDirection = targetPosition - transform.position;
        transform.position += movementDirection.normalized * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPosition) < reachDistance)
        {
            if (!isReversing)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    isReversing = true;
                    currentWaypointIndex = waypoints.Length - 2;
                }
            }
            else
            {
                currentWaypointIndex--;
                if (currentWaypointIndex < 0)
                {
                    isReversing = false;
                    currentWaypointIndex = 1;
                }
            }
        }
    }

    public void ReversePath() {
        isReversing = !isReversing;
        if (isReversing && currentWaypointIndex > 0 )
        {
            currentWaypointIndex--;
        }
        else
        {
            currentWaypointIndex++;
            
        }
        
    }

    public int GetCurrentWaypointIndex() {
        return currentWaypointIndex;
    }
}
