using System;
using System.Collections;
using Intertables.Enemies;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Guard : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private bool goInRounds;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float reachDistance = 0.1f;

    [SerializeField] private float sleepTime;
    // [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private Direction direction = Direction.Right;
    private int currentWaypointIndex = 0;
    private bool isReversing = false;
    public bool isStopped = false;
    public float puddleImmunityTimer = 0f;
    [SerializeField] private float slowDifference = 2f;
    private Rigidbody2D body;
    [SerializeField] private GameObject lightPivot;
    private GuardLight light;   
    private GuardAnimatorController animatorController;
    private bool seenSign;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] guardSoundsHuh;
    [SerializeField] private AudioClip[] guardSoundsStop;
    [SerializeField] private AudioClip[] guardSoundsSlip;
    private int diffrence =1;

    private void Start()
    {
        MopSign.OnPickUp += MopSignOnOnPickUp;
        body = GetComponent<Rigidbody2D>();
        animatorController = GetComponent<GuardAnimatorController>();
        light = lightPivot.GetComponentInChildren<GuardLight>();
        Vector2 directionVector = Vector2.right;
        switch (direction)
        {
            case Direction.Right: directionVector = new Vector2(1, 0);
                break;
            case Direction.Left: directionVector = new Vector2(-1,0);
                break;
            case Direction.Front: directionVector = new Vector2(0,-1);
                break;
            case Direction.Back: directionVector = new Vector2(0,1);
                break;
            
        }
        animatorController.setLastAxis(directionVector.x,directionVector.y);
        if (direction == Direction.Back || direction == Direction.Front)
        {
            directionVector *= -1;
        }
        float angle = (Mathf.Atan2(directionVector.x, directionVector.y)) * Mathf.Rad2Deg;
        lightPivot.transform.rotation = Quaternion.Euler(0, 0, angle-180f);
        
    }

    private void MopSignOnOnPickUp(object sender, EventArgs e)
    {
        seenSign = false;
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

    public void StopMovement() {
        isStopped = true;
        puddleImmunityTimer = 3f;
        speed -= slowDifference;
        StartCoroutine("RestoreSpeed");
        animatorController.PlaySlipAnimation();
        light.DisableLight();
        if (guardSoundsSlip.Length > 0 && audioSource != null)
        {
            int randomIndex = Random.Range(0, guardSoundsSlip.Length);
            audioSource.PlayOneShot(guardSoundsSlip[randomIndex]);
        }
    }

    public void Move() {
        if (waypoints.Length == 0 || isStopped)
        {
            body.velocity = Vector2.zero;
            return;
        }

        if (currentWaypointIndex < 0 )
        {
            currentWaypointIndex = waypoints.Length - 1;
        }
        else if (currentWaypointIndex >= waypoints.Length)
        {
            currentWaypointIndex = 0;
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
            if (goInRounds)
            {
                currentWaypointIndex+=diffrence;
                Debug.Log("Current waypoint: "+currentWaypointIndex);
                if (isReversing)
                {
                    if (currentWaypointIndex == 0)
                    {
                        currentWaypointIndex =0;
                    }
                }
                else if (currentWaypointIndex == waypoints.Length-1)
                {
                    currentWaypointIndex =waypoints.Length-1;
                }
                
            }
            else
            {
                if (!isReversing)
                {
                    currentWaypointIndex++;
                    if (currentWaypointIndex == waypoints.Length)
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
    }

    public void ReversePath() {
        if (goInRounds)
        {
            
            diffrence *= -1;
            currentWaypointIndex += diffrence;
            Debug.Log("Difference: "+diffrence +" Current waypoint"+currentWaypointIndex);
        }
        else
        {
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
        if (guardSoundsHuh.Length > 0 && audioSource != null && !seenSign)
        {
            int randomIndex = Random.Range(0, guardSoundsHuh.Length);
            audioSource.PlayOneShot(guardSoundsHuh[randomIndex]);
            seenSign = true;
        }
    }

    public void CatchFrau()
    {
        CameraTargetSelector.Instance.setTargetGroup(transform);
        isStopped = true;
        animatorController.PlayCatchFrau();
        if (guardSoundsStop.Length > 0 && audioSource != null)
        {
            int randomIndex = Random.Range(0, guardSoundsStop.Length);
            audioSource.PlayOneShot(guardSoundsStop[randomIndex]);
        }
        else
        {
            Debug.LogWarning("No sounds assigned or AudioSource is missing!");
        }
    }

    private IEnumerator RestoreSpeed() {
        yield return new WaitForSeconds(sleepTime);
        speed += slowDifference;
    }

    public void setIsStoped()
    {
        isStopped = false;
        light.EnableLight();
    }
}
