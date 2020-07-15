using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovementLoop : MonoBehaviour
{
    public enum State
    {
        Idle,
        Patrolling,
        Suspicious,
        Investigating,
        Startled,
        Chasing,
        Stunned
    }

    private enum Direction 
    {
        Up,
        Down,
        Left,
        Right
    }

    public State state;

    // Key markers that define the path of the priest.
    public GameObject[] keyObjects; 

    // Typical speed of the priest.
    public float speed; 
    public float investigationSpeed; 
    public float chaseSpeed; 

    public float suspiciousWaitTime; 
    public float investigationWaitTime; 
    public float startledWatiTime; 

    private Vector2[] keyPoints; 
    private float[] waitTimes;
    private int currentKeyPointIndex = 0;
    private float waitTime;

    private Vector3 lastPos;
    private Direction currentDirection;

    private Animator animator;

    void Start()
    {
        state = State.Idle;

        keyPoints = new Vector2[keyObjects.Length];
        waitTimes = new float[keyObjects.Length];

        for (int i = 0; i < keyObjects.Length; i++)
        {
           keyPoints[i] = keyObjects[i].gameObject.transform.position;
           waitTimes[i] = keyObjects[i].GetComponent<WaitStore>().WaitTime;
           keyObjects[i].SetActive(false);
        }

        waitTime = waitTimes[currentKeyPointIndex];

        PriestManager.investigableTrigger += InvestigableTrigger;
        SetSpeed(speed);
        SetNextKeyPoint();
        lastPos = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update() 
    {
        UpdateCurrentDirection();
    }

    void FixedUpdate()
    {
        if (state != State.Chasing) 
        {
            CheckPlayer();
        }
        if (GetComponent<AILerp>().reachedDestination || GetComponent<AILerp>().reachedEndOfPath || GetComponent<AILerp>().speed == 0)
        {
            if (state == State.Patrolling)
            {
                state = State.Idle;
            }
            if (waitTime > 0) 
            {
                waitTime -= Time.deltaTime;
                return;
            }

            if (state == State.Suspicious) 
            { 
                state = State.Investigating; 
                waitTime = investigationWaitTime;
                SetSpeed(investigationSpeed);
            }
            else if (state == State.Investigating) { state = State.Patrolling; }
            else if (state == State.Startled) 
            { 
                state = State.Chasing; 
                SetSpeed(chaseSpeed);
            }
            else if (state == State.Idle) { 
                state = State.Patrolling;
                SetSpeed(speed);
                SetNextKeyPoint(); 
            }
        }
    }

    void CheckPlayer()
    {
        Vector3 lineTo = Vector3.right;
        if (currentDirection == Direction.Right) { lineTo = Vector3.right; }
        if (currentDirection == Direction.Left) { lineTo = Vector3.left; }
        if (currentDirection == Direction.Up) { lineTo = Vector3.up; }
        if (currentDirection == Direction.Down) { lineTo = Vector3.down; }

        RaycastHit2D ray = Physics2D.Linecast(transform.position + lineTo*2, transform.position + lineTo * 20);

        if (ray.collider != null) 
        {
            Debug.Log("Collided against " + ray.collider.gameObject);
            if (ray.collider.gameObject.CompareTag("Player"))
            {
                Debug.Log("Saw player!!!");
                state = State.Startled;
                waitTime = startledWatiTime;
                SetTarget(ray.collider.gameObject.transform);
                SetSpeed(0);
            }
        }
    }

    void SetTarget(Transform trsfrm) { GetComponent<AIDestinationSetter>().target = trsfrm; }
    void SetSpeed(float spd) { GetComponent<AILerp>().speed = spd; }

    void UpdateCurrentDirection()
    {
        var velocity = transform.position - lastPos;
        lastPos = transform.position;
        if (velocity.y > 0){ currentDirection = Direction.Up; SetAnimMovement(0, 1);}
        else if (velocity.y < -0){ currentDirection = Direction.Down; SetAnimMovement(0, -1);}
        else if (velocity.x > 0){ currentDirection = Direction.Right; SetAnimMovement(1, 0);}
        else if (velocity.x < -0){ currentDirection = Direction.Left; SetAnimMovement(-1, 0);}
        else { animator.SetBool("moving", false); }
    }

    void SetAnimMovement(float x, float y)
    {
        animator.SetBool("moving", true);
        animator.SetFloat("moveX", x);
        animator.SetFloat("moveY", y);
    }

    void SetNextKeyPoint() // Set the destination to the next key point.
    {
        if (currentKeyPointIndex + 1 < keyPoints.Length) {
            currentKeyPointIndex ++;
        } else {
            currentKeyPointIndex = 0;
        }
        waitTime = waitTimes[currentKeyPointIndex];
        SetTarget(keyObjects[currentKeyPointIndex].transform);
    }

    void InvestigableTrigger(Transform susTransform) // Called when a distraction event occurs.
    {
        if (Vector3.Distance(transform.position, susTransform.position) < 12)
        {
            Debug.Log("SUSPICIOUS!!");
            waitTime = suspiciousWaitTime;
            SetTarget(transform);
            SetSpeed(0);
            state = State.Suspicious;
        }
    }

    private void OnDestroy() {
        PriestManager.investigableTrigger -= InvestigableTrigger;
    }

    private void OnDrawGizmos() {
        Vector3 lineTo = Vector3.right;
        if (currentDirection == Direction.Right) { lineTo = Vector3.right; }
        if (currentDirection == Direction.Left) { lineTo = Vector3.left; }
        if (currentDirection == Direction.Up) { lineTo = Vector3.up; }
        if (currentDirection == Direction.Down) { lineTo = Vector3.down; }
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + lineTo, transform.position + lineTo * 5);
    }

}
