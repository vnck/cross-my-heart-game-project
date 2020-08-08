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
                    GetComponent<SpriteRenderer>().sprite = unlockedSprite;
                    other.GetComponent<PlayerPossession>().depossessing();
                    GameObject.Find(unlockingKey).SetActive(false);
                    StartCoroutine(waitForDepossess());
                } else {
                    SaySmt.Line("Me", "Door is locked!");
                    Debug.Log("Door locked! Need key");
                }
            }
        }     
    }

    IEnumerator waitForDepossess() {
        yield return new WaitForSeconds(0.5f);
        isLocked = false;
        yield return null;
    }
}
