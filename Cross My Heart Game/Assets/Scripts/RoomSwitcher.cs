﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSwitcher : MonoBehaviour
{
    public int switchId;
    public int destinationSwitchId;
    public GameObject destinationSwitch;
    static GameObject player;
    static GameObject playerLight;
    static GameObject camera;
    public float destinationOffset;
    public bool activated;
    
    // Start is called before the first frame update
    void Start()
    {
        if (player == null) { player = GameObject.FindGameObjectWithTag("Player"); }
        if (playerLight == null) { playerLight = GameObject.FindGameObjectWithTag("PlayerLight"); }
        if (camera == null) { camera = GameObject.FindGameObjectWithTag("MainCamera"); }
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" && activated) {
            if (GetComponent<Door>() != null) {
                if (GetComponent<Door>().isLocked == false) {
                    Debug.Log("Room Switch " + switchId + " triggered, going to Room Switch " + destinationSwitchId);
                    player.transform.position = new Vector3(destinationSwitch.transform.position.x, destinationSwitch.transform.position.y + destinationOffset, 0);
                    playerLight.transform.position = new Vector3(destinationSwitch.transform.position.x, destinationSwitch.transform.position.y + destinationOffset, 0);
                    camera.transform.position = new Vector3(destinationSwitch.transform.position.x, destinationSwitch.transform.position.y + destinationOffset, -10);
                }
            } else if (GetComponent<Door>() == null) {
                Debug.Log(destinationOffset);
                Debug.Log("Room Switch " + switchId + " triggered, going to Room Switch " + destinationSwitchId);
                player.transform.position = new Vector3(destinationSwitch.transform.position.x, destinationSwitch.transform.position.y + destinationOffset, 0);
                playerLight.transform.position = new Vector3(destinationSwitch.transform.position.x, destinationSwitch.transform.position.y + destinationOffset, 0);
                camera.transform.position = new Vector3(destinationSwitch.transform.position.x, destinationSwitch.transform.position.y + destinationOffset, -10);
            }
        }
    }
}
