using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSwitcher : MonoBehaviour
{
    public int switchId;
    public int destinationSwitchId;
    public GameObject destinationSwitch;
    static GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        if (player == null) { player = GameObject.FindGameObjectWithTag("Player"); }
        // if (currentDestinationSwitchId == switchId)
        // {
        //     player.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        // }
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            Debug.Log("Room Switch " + switchId + " triggered, going to Room Switch " + destinationSwitchId);
            // currentDestinationSwitchId = destinationSwitchId;
            Debug.Log("Switching: " + destinationSwitch.transform.position);
            player.transform.position = destinationSwitch.transform.position;
        }
    }
}
