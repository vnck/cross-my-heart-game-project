using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaySmt : MonoBehaviour
{
    static bool speaking = false;
    static bool reset = false;
    static public void Line(string person, string message, bool reset = false)
    {
        GameObject convo = GameObject.FindGameObjectWithTag("Convo");
        Debug.Log("CONVO", convo);
        convo.GetComponent<Canvas>().enabled = true;
        if (person == "") {
            convo.GetComponentInChildren<Text>().text = message;
        } else {
            convo.GetComponentInChildren<Text>().text = person + ": " + message;
        }
        SaySmt.reset = reset;
        speaking = true;
        Time.timeScale = 0;
    }

    private void Update() {
        if (speaking && Input.GetKeyDown("space")) 
        {
            GameObject convo = GameObject.FindGameObjectWithTag("Convo");
            convo.GetComponent<Canvas>().enabled = false;
            Time.timeScale = 1;
            speaking = false;
            if (reset) 
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
