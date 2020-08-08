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
    static GameObject playerLight;
    static GameObject camera;
    static int currentDestinationSwitchId = -1;

    public string conditionalItem;
    public bool possessionCheck;


    private void Start() {
        if (player == null) { player = GameObject.FindGameObjectWithTag("Player"); }
        if (playerLight == null) { playerLight = GameObject.FindGameObjectWithTag("PlayerLight"); }
        if (camera == null) { camera = GameObject.FindGameObjectWithTag("MainCamera"); }
        if (currentDestinationSwitchId == switchId)
        {
            Debug.Log("transporting player to scenechanger " + switchId);
            player.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            playerLight.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            camera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            if (possessionCheck){
                if (other.GetComponent<PlayerPossession>().itemName == conditionalItem){
                    changeScene();
                    Debug.Log(other.GetComponent<PlayerPossession>().itemName);
                }
                else {
                    SaySmt.PrepLine("Priest", "Hey, no ghosts allowed!");
                    SaySmt.PrepLine("Clyde", "I think I need a disguise to get in...");
                    SaySmt.PlayLines();
                }
            } else if (destinationSwitchId < 0) {
                SaySmt.Line("", "A mysterious voice tells you to not look back.");
                SaySmt.prepClose = true;
            }
             else {
                changeScene();
            }
        }
    }
    private void changeScene() {
    Debug.Log(switchId + " triggered, going to " + destinationSwitchId);
    currentDestinationSwitchId = destinationSwitchId;
    SceneManager.LoadScene(scene_index);
}
}