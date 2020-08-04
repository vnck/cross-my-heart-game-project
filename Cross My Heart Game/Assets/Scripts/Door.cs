using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isLocked;
    public Sprite unlockedSprite;

    // Start is called before the first frame update
    void Start()
    {
        isLocked = true;
    }

    // void Update() {
    //     if (isLocked == true) {
    //         GetComponent<RoomSwitcher>().enabled = false;
    //     }
    // }
}
