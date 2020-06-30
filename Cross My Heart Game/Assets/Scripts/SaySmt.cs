using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaySmt : MonoBehaviour
{
    static bool speaking = false;
    static public void Line(string person, string message)
    {
        GameObject convo = GameObject.FindGameObjectWithTag("Convo");
        Debug.Log("CONVO", convo);
        convo.GetComponent<Canvas>().enabled = true;
        convo.GetComponentInChildren<Text>().text = person + ": " + message;
        speaking = true;
        Time.timeScale = 0;
    }

    private void Update() {
        if (speaking && Input.GetKeyDown("space")) 
        {
            GameObject convo = GameObject.FindGameObjectWithTag("Convo");
            convo.GetComponent<Canvas>().enabled = false;
            Time.timeScale = 1;
        }
    }
}
