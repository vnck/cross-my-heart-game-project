﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayShadow : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject lightOn;
    public GameObject lightOff;
    private bool passed;
    private Collider2D collider;
    void Start()
    {
        lightOn.SetActive(false);
        lightOff.SetActive(true);
        collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            passed = true;
            lightOn.SetActive(true);
            lightOff.SetActive(false);
            collider.isTrigger = false;
        }
    }

    // private void OnTriggerExit2D(Collider2D other) {
    //     if (other.CompareTag("Player")) {
            
    //     }
    // }
}
