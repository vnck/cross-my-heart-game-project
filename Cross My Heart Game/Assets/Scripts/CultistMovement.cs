
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CultistMovement : MonoBehaviour
{
    private bool hasBook = false;
    private GameObject targetBook;
    private GameObject targetBookPoint;
    private Vector3 lastPos;
    private Animator animator;

    void Start() {
        lastPos = transform.position;
        animator = GetComponent<Animator>();
        targetBook = FindNearestBook();
        if (targetBook != null) {
            targetBook.GetComponent<Book>().isTarget = true;
            SetTarget(targetBook.transform);     
        }
    }
    void SetTarget(Transform trsfrm) { GetComponent<AIDestinationSetter>().target = trsfrm; }

    private GameObject FindNearestBook() {
        GameObject[] books = GameObject.FindGameObjectsWithTag("Book");
        float closestDistance = 100;
        GameObject closestBook = null;
        foreach (var book in books) {
            float distance = Vector3.Distance(book.transform.position, transform.position);
            if (distance < closestDistance && book.GetComponent<Book>().isTarget == false) {
                closestDistance = distance;
                closestBook = book;
            }
        }
        return closestBook;
    }

    private GameObject FindNearestEmptyBookPoint() {
        GameObject[] bookPoints = GameObject.FindGameObjectsWithTag("BookPoint");
        float closestDistance = 100;
        GameObject closestBookPoint = null;
        foreach (var bookPoint in bookPoints) {
            float distance = Vector3.Distance(bookPoint.transform.position, transform.position);
            if (distance < closestDistance && bookPoint.GetComponent<BookPoint>().hasBook == false && bookPoint.GetComponent<BookPoint>().isTarget == false) {
                closestDistance = distance;
                closestBookPoint = bookPoint;
            }
        }
        return closestBookPoint;
    }

    void FixedUpdate() {
        UpdateCurrentDirection();
        if (hasBook) {
            targetBook.transform.position = transform.position;
        }
    }
    void UpdateCurrentDirection()
    {
        var velocity = transform.position - lastPos;
        lastPos = transform.position;
        if (velocity.y > 0){ SetAnimMovement(0, 1);}
        else if (velocity.y < 0){ SetAnimMovement(0, -1);}
        else if (velocity.x > 0){ SetAnimMovement(1, 0);}
        else if (velocity.x < 0){ SetAnimMovement(-1, 0);}
        else { animator.SetBool("moving", false); }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!hasBook && other.gameObject == targetBook) {
            targetBook.GetComponent<Book>().carried = true;
            hasBook = true;
            targetBookPoint = FindNearestEmptyBookPoint();
            if (targetBookPoint != null) {
                targetBookPoint.GetComponent<BookPoint>().isTarget = true;
                SetTarget(targetBookPoint.transform);
            }
        }
        if (hasBook && other.gameObject == targetBookPoint) {
            Vector3 newPos = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, targetBook.transform.position.z);
            targetBook.transform.position = newPos;
            BookPoint.BookPlaced();
            hasBook = false;
            targetBook = FindNearestBook();
            if (targetBook != null) {
                targetBook.GetComponent<Book>().isTarget = true;
                SetTarget(targetBook.transform);     
            }
        }
    }

    void SetAnimMovement(float x, float y)
    {
        animator.SetBool("moving", true);
        animator.SetFloat("moveX", x);
        animator.SetFloat("moveY", y);
    }
}