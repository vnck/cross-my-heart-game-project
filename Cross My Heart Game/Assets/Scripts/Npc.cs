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
    public List<Line> lines = new List<Line>(); 
    private bool spoken;

    public Line endLine;

    // Start is called before the first frame update
    void Start()
    {
        spoken = false;
    }

    // Update is called once per frame
    public void Speak() {
        if (!spoken && lines.Count > 0) {
            foreach (var line in lines)
            {
                SaySmt.PrepLine(line.speaker != "" ? line.speaker : "", line.message);
            }
            SaySmt.PlayLines();
            spoken = true;
        } else {
            SaySmt.Line(endLine.speaker, endLine.message);
        }
    }
}
