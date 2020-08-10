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

    public enum Direction 
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
    private Direction[] waitDirections;
    private int currentKeyPointIndex = 0;
    private float waitTime;
    private Direction waitDirection;

    private Vector3 lastPos;
    public Direction currentDirection;

    private Animator animator;

    private AudioSource alertSFX;
    private AudioSource susSFX;

    public GameObject alertBoxContainer;
    public GameObject questionBoxContainer;
    private SpriteRenderer alertBox;
    private SpriteRenderer questionBox;
    public Vector3 hitPoint;
    public float hitDistance;

    void Start()
    {
        state = State.Idle;

        keyPoints = new Vector2[keyObjects.Length];
        waitTimes = new float[keyObjects.Length];
        waitDirections = new Direction[keyObjects.Length];

        for (int i = 0; i < keyObjects.Length; i++)
        {
           keyPoints[i] = keyObjects[i].gameObject.transform.position;
           waitTimes[i] = keyObjects[i].GetComponent<WaitStore>().WaitTime;
           waitDirections[i] = keyObjects[i].GetComponent<WaitStore>().WaitDirection;
           keyObjects[i].SetActive(false);
        }

        waitTime = waitTimes[currentKeyPointIndex];
        waitDirection = waitDirections[currentKeyPointIndex];

        PriestManager.investigableTrigger += InvestigableTrigger;
        SetSpeed(speed);
        SetNextKeyPoint();
        lastPos = transform.position;
        animator = GetComponent<Animator>();
        alertSFX = GetComponents<AudioSource>()[0];
        susSFX = GetComponents<AudioSource>()[1];
        alertBox = alertBoxContainer.GetComponent<SpriteRenderer>();
        questionBox = questionBoxContainer.GetComponent<SpriteRenderer>();
    }

    void Update() 
    {
        UpdateCurrentDirection();
    }

    void FixedUpdate()
    {
        if (state != State.Chasing && state != State.Startled) 
        {
            CheckPlayer();
        } else {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) {
                Debug.Log("DISTANCE " + Vector3.Distance(player.transform.position, transform.position));
                if (Vector3.Distance(player.transform.position, transform.position) < 3f) {
                    player.GetComponent<PlayerMovement>().Kill();
                }
            }
        }
        if (GetComponent<AILerp>().reachedDestination || GetComponent<AILerp>().reachedEndOfPath || GetComponent<AILerp>().speed == 0)
        {
            if (state == State.Patrolling)
            {
                state = State.Idle;
                currentDirection = waitDirection;
                if (waitDirection == Direction.Up){ SetAnimMovement(0, 1); }
                else if (waitDirection == Direction.Down){ SetAnimMovement(0, -1); }
                else if (waitDirection == Direction.Right){ SetAnimMovement(1, 0); }
                else if (waitDirection == Direction.Left){ SetAnimMovement(-1, 0); }
            }
            if (waitTime > 0) 
            {
                waitTime -= Time.deltaTime;
                return;
            }

            if (state == State.Suspicious) 
            { 
                Debug.Log("Investigating!!");
                state = State.Investigating; 
                questionBox.enabled = false;
                waitTime = investigationWaitTime;
                SetSpeed(investigationSpeed);
            }
            else if (state == State.Investigating) { 
                Debug.Log("Patrolling!!");
                state = State.Patrolling; 
            }
            else if (state == State.Startled) 
            { 
                state = State.Chasing; 
                alertBox.enabled = false;
                GetComponent<AILerp>().repathRate = 0.2f;
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

        RaycastHit2D ray = Physics2D.Linecast(transform.position + lineTo, transform.position + lineTo * 5);

        if (ray.collider != null) 
        {
            hitPoint = ray.point;
            hitDistance = Vector3.Distance(transform.position, hitPoint);
            if (ray.collider.gameObject.CompareTag("Player"))
            {
                Debug.Log("Saw player!!!");
                PlayerPossession p = ray.collider.gameObject.GetComponent<PlayerPossession>();
                PlayerMovement pm = ray.collider.gameObject.GetComponent<PlayerMovement>();
                if (p.isPossessed && pm.IsStill()) { return; }
                state = State.Startled;
                alertSFX.Play(0);
                alertBox.enabled = true;
                waitTime = startledWatiTime;
                animator.SetBool("moving", false);
                SetTarget(ray.collider.gameObject.transform);
                SetSpeed(0);
                PriestManager.chasing = true;
            }
        } else {
            hitPoint = Vector3.zero;
            hitDistance = 0;
        }
    }

    void SetTarget(Transform trsfrm) { GetComponent<AIDestinationSetter>().target = trsfrm; }
    void SetSpeed(float spd) { GetComponent<AILerp>().speed = spd; }

    void UpdateCurrentDirection()
    {
        if (state == State.Startled || state == State.Suspicious) { return; }
        var velocity = transform.position - lastPos;
        lastPos = transform.position;
        if (velocity.y > 0){ currentDirection = Direction.Up; SetAnimMovement(0, 1);}
        else if (velocity.y < 0){ currentDirection = Direction.Down; SetAnimMovement(0, -1);}
        else if (velocity.x > 0){ currentDirection = Direction.Right; SetAnimMovement(1, 0);}
        else if (velocity.x < 0){ currentDirection = Direction.Left; SetAnimMovement(-1, 0);}
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
        waitDirection = waitDirections[currentKeyPointIndex];
        SetTarget(keyObjects[currentKeyPointIndex].transform);
    }

    void InvestigableTrigger(Transform susTransform) // Called when a distraction event occurs.
    {
        if (state == State.Chasing || state == State.Startled) { return; }
        if (Vector3.Distance(transform.position, susTransform.position) < 12)
        {
            Debug.Log("SUSPICIOUS!!");
            susSFX.Play(0);
            waitTime = suspiciousWaitTime;
            SetTarget(susTransform);
            SetSpeed(0);
            animator.SetBool("moving", false);
            state = State.Suspicious;
            questionBox.enabled = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("COMPAREDDD");
        if (other.CompareTag("Furniture") || other.CompareTag("Item")) {
            if (transform.position.y > other.transform.position.y) {
                GetComponent<SpriteRenderer>().sortingOrder = -1;
            } else {
                GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
        }
    }

    public bool isChasing() {
        return (state == State.Chasing);
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
        Gizmos.DrawSphere(hitPoint, 0.2f);
    }

}
