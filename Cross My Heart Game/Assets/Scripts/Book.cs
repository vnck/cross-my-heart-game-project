

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Book : MonoBehaviour 
{

    public bool isTarget = false;
    public bool carried = false;
    public bool inPlace = false;

    private Animation anim;

    private void Start() {
        anim = gameObject.GetComponent<Animation>();
    }

    private void Update() {
        if (!anim.isPlaying && inPlace){
            anim.Play("BookSpin");
        }
    }

    // private void OnTriggerEnter2D(Collider2D other) {
    //     Debug.Log("ON FIRE", other.gameObject);
    //     // if (other.gameObject.CompareTag("Fire")) {
    //         Destroy(this);
    //     // }
    // }
    // private void OnCollisionEnter2D(Collision2D other) {
    //     Debug.Log("ON FIRE", other.gameObject);
    //     // if (other.gameObject.CompareTag("Fire")) {
    //         Destroy(this);
    //     // }
    // }
}