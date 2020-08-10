using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFinalLevel : MonoBehaviour
{
    private AudioSource[] bgm;
    public GameObject levelManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player" && !levelManager.GetComponent<FinalLevelManager>().gameStarted && !levelManager.GetComponent<FinalLevelManager>().winGame && !levelManager.GetComponent<FinalLevelManager>().loseGame) {
            levelManager.GetComponent<FinalLevelManager>().StartLevel();
        }
    }
}
