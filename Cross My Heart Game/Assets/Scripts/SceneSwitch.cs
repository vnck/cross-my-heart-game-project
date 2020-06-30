using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public int scene_index;
    public int switchId;
    public int destinationSwitchId;
    static GameObject player;
    static int currentDestinationSwitchId;

    private void Start() {
        if (player == null) { player = GameObject.FindGameObjectWithTag("Player"); }
        if (currentDestinationSwitchId == switchId)
        {
            player.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            Debug.Log(switchId + " triggered, going to " + destinationSwitchId);
            currentDestinationSwitchId = destinationSwitchId;
            SceneManager.LoadScene(scene_index);
        }
    }
}
