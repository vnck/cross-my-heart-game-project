using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovementLoop : MonoBehaviour
{
    public enum State
    {
        Idle,
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

    void Update() {
        UpdateCurrentDirection();
    }

    void FixedUpdate()
    {
        Vector3 lineTo = Vector3.right;
        if (currentDirection == Direction.Right) { lineTo = Vector3.right; }
        if (currentDirection == Direction.Left) { lineTo = Vector3.left; }
        if (currentDirection == Direction.Up) { lineTo = Vector3.up; }
        if (currentDirection == Direction.Down) { lineTo = Vector3.down; }

        RaycastHit2D ray = Physics2D.Linecast(transform.position + lineTo / 2, transform.position + lineTo * 5);

        if (ray.collider != null) 
        {
            if (ray.collider.gameObject.CompareTag("Player"))
            {
                state = State.Startled;
                waitTime = startledWatiTime;
                GetComponent<AILerp>().enableRotation = true;
                SetTarget(ray.collider.gameObject.transform);
                SetSpeed(0);
            }

        }

        if (GetComponent<AILerp>().reachedDestination || GetComponent<AILerp>().reachedEndOfPath || GetComponent<AILerp>().speed == 0)
        {
            if (waitTime > 0) 
            {
                waitTime --;
                return;
            }

            if (state == State.Suspicious) 
            { 
                state = State.Investigating; 
                waitTime = investigationWaitTime;
                SetSpeed(investigationSpeed);
            }
            else if (state == State.Investigating) { state = State.Idle; }
            else if (state == State.Startled) 
            { 
                state = State.Chasing; 
                SetSpeed(chaseSpeed);
            }
            else if (state == State.Idle) { 
                SetSpeed(speed);
                SetNextKeyPoint(); 
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

    void InvestigableTrigger(Transform transform) // Called when a distraction event occurs.
    {
        waitTime = suspiciousWaitTime;
        SetTarget(transform);
        SetSpeed(0);
        state = State.Suspicious;
    }

}
