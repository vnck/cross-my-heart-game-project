using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Control : MonoBehaviour
{
    private void Update() 
    {
        if (Input.GetKeyDown("escape"))
        {
            SaySmt.Line("", "Press J to unpause,\nPress ESC to go to main menu", false, true);
        }
    }
}