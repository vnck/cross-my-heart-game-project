using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    // Item attributes
    public float health;
    public float moveSpeed;
    public bool isKey;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StationaryAction() {
        // need to attract enemies in range
        // if (Vector3.Distance(transform.position, enemy.transform.position) <= range) {
        //     //go to player
        // }
        // health -= 1;
        PriestManager.TriggerSuspicious();
        Debug.Log("HI");
    }
}
