using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayShadow : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject lightOn;
    public GameObject lightOff;
    private bool passed;
    private Collider2D[] colliders;
    private AudioSource wooshFX;
    void Start()
    {
        //lightOn.SetActive(true);
        lightOff.SetActive(false);
        colliders = GetComponents<Collider2D>();
        wooshFX = GetComponent<AudioSource>();
        colliders[0].isTrigger = true;
        colliders[1].enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // if (PriestManager.chasing) {
        //     collider.isTrigger = false;
        // }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player") && !passed) {
            lightOn.SetActive(false);
            lightOff.SetActive(true);
            wooshFX.Play(0);
            colliders[1].enabled = true;
            passed = true;
        }
    }

    // private void OnTriggerExit2D(Collider2D other) {
    //     if (other.CompareTag("Player")) {
            
    //     }
    // }
}
