using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DarkRoom : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public Light2D light;
    public bool inRoom;
    // Start is called before the first frame update
    void Awake() {
        inRoom = false;
    }
    void Start()
    {
        inRoom = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inRoom){
            if (player.GetComponent<PlayerPossession>().itemName == "lamp") {
                light.pointLightInnerRadius = 10F;
                light.pointLightOuterRadius = 10;
            } else {
                light.pointLightInnerRadius = 1F;
                light.pointLightOuterRadius = 1F;
            }
        } else {
            light.pointLightInnerRadius = 3;
            light.pointLightOuterRadius = 3;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player"){
            inRoom = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player"){
            inRoom = false;
            light.pointLightInnerRadius = 3;
            light.pointLightOuterRadius = 3;
        }
    }
}
