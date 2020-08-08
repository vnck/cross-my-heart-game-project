using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaySmt : MonoBehaviour
{
    public static bool speaking = false;
    static bool reset = false;
    static bool keyDown = false;

    static List<Dictionary<string,string>> lines = new List<Dictionary<string, string>>();

    static public void PrepLine(string person, string message) {
        Dictionary<string,string> line = new Dictionary<string, string>();
        line.Add("speaker", person);
        line.Add("message", message);
        lines.Add(line);
    }

    static public void PlayLines(bool reset = false) {
        GameObject convo = GameObject.FindGameObjectWithTag("Convo");
        Debug.Log("opening speech box");
        convo.GetComponent<Canvas>().enabled = true;

        if (lines[0]["speaker"] == "") {
        convo.GetComponentInChildren<Text>().text = lines[0]["message"];
        } else {
            convo.GetComponentInChildren<Text>().text = lines[0]["speaker"] + ": " + lines[0]["message"];
        }
        lines.RemoveAt(0);
        keyDown = true;
        speaking = true;
        Time.timeScale = 0;
        SaySmt.reset = reset;
    }
    static public void Line(string person, string message, bool reset = false)
    {
        GameObject convo = GameObject.FindGameObjectWithTag("Convo");
        Debug.Log("opening speech box");
        convo.GetComponent<Canvas>().enabled = true;
        if (person == "") {
            convo.GetComponentInChildren<Text>().text = message;
        } else {
            convo.GetComponentInChildren<Text>().text = person + ": " + message;
        }
        Time.timeScale = 0;
        keyDown = true;
        speaking = true;
        SaySmt.reset = reset;
    }

    private void Update() {
        
        if (speaking && Input.GetKeyDown("o") && !keyDown) 
        {
            Debug.Log("closing speech box");
            GameObject convo = GameObject.FindGameObjectWithTag("Convo");
            convo.GetComponent<Canvas>().enabled = false;
            Time.timeScale = 1;
            speaking = false;
            if (lines.Count > 0){ 
                PlayLines();
            }
            else if (reset) 
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else if (Input.GetKeyUp("o") && keyDown) {
            keyDown = false;
        }
    }
}
