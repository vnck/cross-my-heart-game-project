using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverviewCam : MonoBehaviour
{
    public GameObject camera;
    public GameObject player;
    private bool playerInRange = false;
    public bool viewing = false;

    void Update()
    {
        if (Input.GetKeyDown("j") && playerInRange && !viewing) {
            viewing = true;
        } else if (Input.GetKeyDown("j") && playerInRange && viewing) {
            viewing = false;
        }
        if (viewing) {
            camera.GetComponents<FollowPlayer>()[0].enabled = false;
            camera.GetComponents<FollowPlayer>()[1].enabled = true;
            camera.GetComponent<Camera>().orthographicSize = 12;
            player.GetComponent<PlayerMovement>().speed = 0;
        } else {
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
