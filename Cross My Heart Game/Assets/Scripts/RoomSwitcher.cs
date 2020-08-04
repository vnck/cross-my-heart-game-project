using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSwitcher : MonoBehaviour
{
    public int switchId;
    public int destinationSwitchId;
    public GameObject destinationSwitch;
    static GameObject player;
    public int destinationOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        if (player == null) { player = GameObject.FindGameObjectWithTag("Player"); }
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            if (GetComponent<Door>() != null) {
                if (GetComponent<Door>().isLocked == false) {
                    Debug.Log("Room Switch " + switchId + " triggered, going to Room Switch " + destinationSwitchId);
                    player.transform.position = new Vector3(destinationSwitch.transform.position.x, destinationSwitch.transform.position.y + destinationOffset, 0);
                }
            } else if (GetComponent<Door>() == null) {
                Debug.Log(destinationOffset);
                Debug.Log("Room Switch " + switchId + " triggered, going to Room Switch " + destinationSwitchId);
                player.transform.position = new Vector3(destinationSwitch.transform.position.x, destinationSwitch.transform.position.y + destinationOffset, 0);
            }
        }
    }
}
