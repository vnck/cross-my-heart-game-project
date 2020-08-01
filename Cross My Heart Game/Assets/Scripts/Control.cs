using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    private void Update() 
    {
        if (Input.GetKeyDown("escape"))
        {
            SaySmt.Line("", "Paused");
        }
    }
}