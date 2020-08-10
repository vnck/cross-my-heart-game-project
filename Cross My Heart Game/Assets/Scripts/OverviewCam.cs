using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverviewCam : MonoBehaviour
{
    public GameObject camera;
    public GameObject player;
    private bool playerInRange = false;

    void Update()
    {
        if (Input.GetKey("j") && playerInRange) {
            camera.GetComponents<FollowPlayer>()[0].enabled = false;
            camera.GetComponents<FollowPlayer>()[1].enabled = true;
            camera.GetComponent<Camera>().orthographicSize = 12;
            player.GetComponent<PlayerMovement>().speed = 0;
        }
        if (Input.GetKeyUp("j") && playerInRange) {
            player.GetComponent<PlayerMovement>().speed = 5;
            camera.GetComponents<FollowPlayer>()[0].enabled = true;
            camera.GetComponents<FollowPlayer>()[1].enabled = false;
            camera.GetComponent<Camera>().orthographicSize = 5;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerInRange = false;
        }
    }
}
