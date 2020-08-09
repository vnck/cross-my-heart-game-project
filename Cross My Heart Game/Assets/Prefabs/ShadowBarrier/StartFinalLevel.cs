using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFinalLevel : MonoBehaviour
{
    private AudioSource[] bgm;
    // Start is called before the first frame update
    void Start()
    {
        bgm = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            bgm[1].Play(0);
        }
        GameObject[] cultists = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(var cultist in cultists) {
            CultistMovement cm = cultist.GetComponent<CultistMovement>();
            if (cm != null) {
                cm.StartMoving();
            }
        }
    }
}
