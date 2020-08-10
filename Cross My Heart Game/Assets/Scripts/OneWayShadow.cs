using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayShadow : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject lightOn;
    public GameObject lightOff;
    private bool passed;
    private Collider2D collider;
    private AudioSource wooshFX;
    void Start()
    {
        lightOn.SetActive(true);
        lightOff.SetActive(false);
        collider = GetComponent<Collider2D>();
        wooshFX = GetComponent<AudioSource>();
        collider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player") && !passed) {
            lightOn.SetActive(false);
            lightOff.SetActive(true);
            wooshFX.Play(0);
            collider.isTrigger = false;
            passed = true;
        }
    }

    // private void OnTriggerExit2D(Collider2D other) {
    //     if (other.CompareTag("Player")) {
            
    //     }
    // }
}
