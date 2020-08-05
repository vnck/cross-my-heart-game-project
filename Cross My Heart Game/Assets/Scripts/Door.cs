using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isLocked;
    public Sprite unlockedSprite;
    public string unlockingKey;

    // Start is called before the first frame update
    void Start()
    {
        isLocked = true;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (isLocked) {
                if (other.GetComponent<PlayerPossession>().itemName == unlockingKey) {
                    isLocked = false;
                    GetComponent<SpriteRenderer>().sprite = unlockedSprite;
                } else {
                    SaySmt.Line("Me", "Door is locked!");
                    Debug.Log("Door locked! Need key");
                }
            }
        }     
    }
}
