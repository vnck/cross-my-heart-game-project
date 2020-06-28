using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    // private bool isPossessed;

    // Item attributes
    public float health;
    public float moveSpeed;
    // private Vector3 change;
    // private Rigidbody2D item;
    
    // // Range for enemies to be distracted
    // public float range;

    // // Player related variables
    // private bool playerInRange;
    // private GameObject player;

    // //Animator
    // private Animator playerAnim;


    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 15f;
    }

    public void StationaryAction() {
        // need to attract enemies in range
        // if (Vector3.Distance(transform.position, enemy.transform.position) <= range) {
        //     //go to player
        // }
        // health -= 1;
        Debug.Log("HI");
    }

    // // Update is called once per frame
    // void Update()
    // {
    //     // if (health > 0) {
    //         if (isPossessed) {
    //             onPossessed();
    //         }

    //         if (Input.GetKeyDown("p")) {
    //             if (!isPossessed && playerInRange) {
    //                 isPossessed = true;
    //                 playerAnim.SetTrigger("possessing");
    //                 // player.SetActive(false);
    //                 StartCoroutine(waitForAnim());
    //                 gameObject.tag = "Player";
    //             } else if (isPossessed) {
    //                 // health -= 1;
    //                 isPossessed = false;
    //                 gameObject.tag = "Item";
    //                 player.transform.position = transform.position;
    //                 player.SetActive(true);
    //             }
    //         }
    //     // }
    // }

    // private void OnTriggerEnter2D(Collider2D other) {
    //     Debug.Log("Inside item");
    //     if (other.CompareTag("Player")) {
    //         playerInRange = true;
            
    //     }
    // }

    // private void OnTriggerExit2D(Collider2D other) {
    //     Debug.Log("Outside item");
    //     if (other.CompareTag("Player")) {
    //         playerInRange = false;
    //         // possessText.enabled = false;
    //     }
    // }

    // void onPossessed() {
    //     change = Vector3.zero;
    //     change.x = Input.GetAxisRaw("Horizontal");
    //     change.y = Input.GetAxisRaw("Vertical");
    //     if (change != Vector3.zero) {
    //         MoveItem();
    //     }
    // }

    // void MoveItem() {
    //     item.MovePosition(transform.position + change * moveSpeed * Time.deltaTime);
    // }

    // IEnumerator waitForAnim() {
    //     yield return new WaitForSeconds(0.5f);
    //     player.SetActive(false);
    //     yield return null;
    // }
}
