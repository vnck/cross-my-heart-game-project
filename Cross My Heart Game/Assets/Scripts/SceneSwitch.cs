using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public int scene_index;
    public GameObject player;
    public GameObject playerObj;
    public float starting_pos_X;
    public float starting_pos_Y;
    static Vector3 playerPosition;
    static bool sceneSwitched = false;

    private void Start() {
        if (sceneSwitched) {
            playerObj = GameObject.FindGameObjectsWithTag("Player")[0];
            playerObj.transform.position = playerPosition;
            // player.transform.position = playerPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            sceneSwitched = true;
            playerPosition = new Vector3(starting_pos_X, starting_pos_Y, 0);
            SceneManager.LoadScene(scene_index);
        }
    }
}
