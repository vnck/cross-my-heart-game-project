using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BookPoint : MonoBehaviour 
{

    public bool isTarget = false;
    public bool hasBook = false;
    private static int booksPlaced = 0;

    public static void BookPlaced() {
        booksPlaced += 1;
        if (booksPlaced == 8) {
            // Add completion-of-ritual code here.
        }
    }

}