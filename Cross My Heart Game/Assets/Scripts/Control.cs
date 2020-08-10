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
            SaySmt.Line("", "Press J to unpause,\nPress Q to go to quit to the main menu", false, true);
            SaySmt.prepClose = true;
        }
    }
}