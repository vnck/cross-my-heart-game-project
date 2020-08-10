using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{ 
    AudioSource sfx;
    public GameObject player;

    private void Start() {
        sfx = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player")) {
            PlayerPossession playerPossession = other.GetComponent<PlayerPossession>();
            if (playerPossession.isPossessed && playerPossession.item.CompareTag("Book")) {
                sfx.Play(0);
                playerPossession.Depossess();
                Destroy(playerPossession.item);
                playerPossession.item = null;
                FinalLevelManager.BurnBook();
            }
        }
    }
}
