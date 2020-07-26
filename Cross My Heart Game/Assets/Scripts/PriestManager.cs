using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestManager : MonoBehaviour
{
    public delegate void InvestigableTrigger(Transform transform);
    public static event InvestigableTrigger investigableTrigger;

    public static void TriggerSuspicious()
    {
        Transform t = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject o = new GameObject();
        o.transform.position = new Vector2(Mathf.Round(t.position.x), Mathf.Round(t.position.y));
        PriestManager.investigableTrigger(o.transform);
    }
}
