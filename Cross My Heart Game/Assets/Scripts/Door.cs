using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isLocked;
    public Sprite unlockedSprite;
    public string unlockingKey;
    public GameObject pBoxContainer;
    private SpriteRenderer pBox;

    // Start is called before the first frame update
    void Start()
    {
        isLocked = true;
        pBoxContainer = GameObject.Find("PBox");
        pBox = pBoxContainer.GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (isLocked) {
                if (other.GetComponent<PlayerPossession>().itemName == unlockingKey) {
                    GetComponent<SpriteRenderer>().sprite = unlockedSprite;
                    GetComponents<AudioSource>()[0].Play(0);
                    other.GetComponent<PlayerPossession>().Depossess();
                    StartCoroutine(waitForDepossess());
                    Destroy(GameObject.Find(unlockingKey));
                } else {
                    GetComponents<AudioSource>()[1].Play(0);
                    SaySmt.Line("Clyde", "The door is locked. I think I need a key.");
                    SaySmt.prepClose = true;
                    Debug.Log("Door locked! Need key");
                }
            }
        }     
    }

    IEnumerator waitForDepossess() {
        yield return new WaitForSeconds(0.5f);
        isLocked = false;
        pBox.enabled = false;
        yield return null;
    }
}
