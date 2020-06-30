using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPossession : MonoBehaviour
{
    public bool isPossessed;
    public bool isPossessing;
    public bool isDepossessing;

    // Item attributes
    private GameObject item;
    public GameObject itemPrefab;
    private Sprite itemSprite;
    private Color itemColor;
    
    // Range for enemies to be distracted
    public float range;

    // Player related variables
    private bool playerInRange;

    //Animator
    private Animator playerAnim;
    private SpriteRenderer playerSprite;

    

    // Start is called before the first frame update
    void Start()
    {
        isPossessed = false;
        playerInRange = false;
        playerSprite = GetComponent<SpriteRenderer>();
        playerAnim = GetComponent<Animator>();
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p")) {
            if (!isPossessed && playerInRange) {
                isPossessed = true;
                isPossessing = true;
                GetComponent<PlayerMovement>().speed = item.GetComponent<Item>().moveSpeed;
                playerAnim.Play("possession", 0, 0);
                StartCoroutine(waitForAnim());
            } else if (isPossessed) {
                // health -= 1;
                isPossessed = false;
                isDepossessing = true;
                GetComponent<PlayerMovement>().speed = 5;
                playerAnim.enabled = true;
                playerAnim.Play("depossession", 0, 0);
                if (item != null) {
                    Debug.Log("Help");
                    item.transform.position = transform.position;
                    item.SetActive(true);
                } else {
                    GameObject newItem = (GameObject)Instantiate(itemPrefab, transform.position, itemPrefab.transform.rotation);
                    newItem.GetComponent<SpriteRenderer>().sprite = itemSprite;
                    newItem.GetComponent<SpriteRenderer>().color = itemColor;
                }
                StartCoroutine(waitForDepossessAnim());
            }
        }

        if (Input.GetKeyDown("o") && isPossessed) {
            itemPrefab.GetComponent<Item>().StationaryAction();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Inside item");
        if (other.CompareTag("Item")) {
            playerInRange = true;
            item = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        Debug.Log("Outside item");
        if (other.CompareTag("Item")) {
            playerInRange = false;
        }
    }

    void StationaryAction() {
        // need to attract enemies in range
        // if (Vector3.Distance(transform.position, enemy.transform.position) <= range) {
        //     //go to player
        // }
        // health -= 1;
    }

    IEnumerator waitForAnim() {
        yield return new WaitForSeconds(0.5f);
        playerAnim.enabled = false;
        itemSprite = item.GetComponent<SpriteRenderer>().sprite;
        itemColor = item.GetComponent<SpriteRenderer>().color;
        playerSprite.sprite = itemSprite;
        playerSprite.color = itemColor;
        transform.position = item.transform.position;
        item.SetActive(false);
        isPossessing = false;
        yield return null;
    }

    IEnumerator waitForDepossessAnim() {
        yield return new WaitForSeconds(0.5f);
        playerAnim.SetTrigger("depossessing");
        playerSprite.color = Color.white;
        isDepossessing = false;
        yield return null;
    }
}
