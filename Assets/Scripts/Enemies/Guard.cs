using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Guard : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float reachDistance = 0.1f;
    [SerializeField] private float rotationSpeed = 1f;
    private int currentWaypointIndex = 0;
    private bool isReversing = false;
    public bool isStopped = false;
    public float puddleImmunityTimer = 0f;
    [SerializeField] private float slowDifference = 2f;
    private Rigidbody2D body;
    [SerializeField] private GameObject lightPivot;
    private GuardLight light;   
    private GuardAnimatorController animatorController;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animatorController = GetComponent<GuardAnimatorController>();
        light = lightPivot.GetComponentInChildren<GuardLight>();
    }

    private void FixedUpdate()
    {
        if (puddleImmunityTimer > 0f) {
            puddleImmunityTimer -= Time.deltaTime;
        }
        Move();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "MopSign") {
            ReversePath();
        }
    }

    public void StopMovement(float duration) {
        isStopped = true;
        puddleImmunityTimer = 10f;
        speed -= slowDifference;
        StartCoroutine("RestoreSpeed", 0f);
        animatorController.PlaySlipAnimation();
        light.DisableLight();
    }

    public void Move() {
        if (waypoints.Length == 0 || isStopped)
        {
            body.velocity = Vector2.zero;
            return;
        }
        Vector3 targetPosition = waypoints[currentWaypointIndex].transform.position;
        Vector3 movementDirection = targetPosition - transform.position;
        body.velocity = movementDirection.normalized * (speed);
        animatorController.setLastAxis(movementDirection.x,movementDirection.y);
        if (movementDirection.magnitude > 0.01)
        {
            animatorController.setAxis(movementDirection.x,movementDirection.y);
        }
        if (body.velocity.sqrMagnitude > 0.01f) // Avoid jittering when velocity is near zero
        {
            float angle = Mathf.Atan2(body.velocity.y, body.velocity.x) * Mathf.Rad2Deg;
            lightPivot.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
            // float targetAngle = Mathf.Atan2(body.velocity.y, body.velocity.x) * Mathf.Rad2Deg; // Target rotation angle
            // Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle-90);       // Target rotation quaternion
            // light.transform.rotation = Quaternion.RotateTowards(light.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        if (Vector3.Distance(transform.position, targetPosition) <= reachDistance && waypoints.Length > 1)
        {
            if (!isReversing)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    isReversing = true;
                    currentWaypointIndex -=2;
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
        if (waypoints.Length > 1) {
            if (isReversing && currentWaypointIndex > 0 )
            {
                currentWaypointIndex--;
            }
            else
            {
                currentWaypointIndex++;
            }
        }
    }

    public void CatchFrau()
    {
        CameraTargetSelector.Instance.setTargetGroup(transform);
        isStopped = true;
        animatorController.PlayCatchFrau();
    }

    private IEnumerator RestoreSpeed() {
        yield return new WaitForSeconds(5f);
        speed += slowDifference;
    }

    public void setIsStoped()
    {
        isStopped = false;
        light.EnableLight();
    }
}
