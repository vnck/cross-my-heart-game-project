using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public int scene_index;
    public GameObject player;
    public float starting_pos_Y;
    static Vector3 playerPosition;
    static bool sceneSwitched = false;

    private void Start() {
        if (sceneSwitched) {
            player.transform.position = playerPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            sceneSwitched = true;
            playerPosition = new Vector3(player.transform.position.x, starting_pos_Y, 0);
            SceneManager.LoadScene(scene_index);
        }
    }
}
