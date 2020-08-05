using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
 public struct Line {
     public string speaker;
     public string message;
 }
public class Npc : MonoBehaviour
{
    public Line[] lines; 
    private bool spoken;

    public Line endLine;

    // Start is called before the first frame update
    void Start()
    {
        spoken = false;
        foreach (var line in lines)
        {
            SaySmt.PrepLine(line.speaker != "" ? line.speaker : "", line.message);
        }
    }

    // Update is called once per frame
    public void Speak() {
        if (!spoken) {
            SaySmt.PlayLines();
            spoken = true;
        } else {
            SaySmt.Line(endLine.speaker, endLine.message);
        }
    }
}
