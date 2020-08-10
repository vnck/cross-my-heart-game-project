using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BookPoint : MonoBehaviour 
{

    public bool isTarget = false;
    public bool hasBook = false;

    public static void BookPlaced() {
        FinalLevelManager.PlaceBook();
    }

}