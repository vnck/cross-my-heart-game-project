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
    private int itemLayer;
    private float itemSpeed;
    private Color itemColor;
    public string itemName;
    private bool itemIsKey;
    
    // Range for enemies to be distracted
    public float range;

    // Player related variables
    private bool playerInRange;

    private GameObject npc;

    private bool npcInRange;

    //Animator
    private Animator playerAnim;
    private SpriteRenderer playerSprite;
    private Sprite normalPlayerSprite;

    public GameObject pBoxContainer;
    private SpriteRenderer pBox;
    public GameObject oBoxContainer;
    private SpriteRenderer oBox;

    //Sound
    private AudioSource[] wooshSFX;

    // Start is called before the first frame update
    void Start()
    {
        isPossessed = false;
        playerInRange = false;
        playerSprite = GetComponent<SpriteRenderer>();
        normalPlayerSprite = playerSprite.sprite;
        playerAnim = GetComponent<Animator>();
        pBox = pBoxContainer.GetComponent<SpriteRenderer>();
        oBox = oBoxContainer.GetComponent<SpriteRenderer>();
        wooshSFX = GetComponents<AudioSource>();
        // DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p") && Time.timeScale != 0) {
            if (!isPossessed && playerInRange) {
                isPossessed = true;
                isPossessing = true;
                if (item.GetComponent<Item>().moveSpeed == 0) {
                    GetComponents<Collider2D>()[0].isTrigger = true;
                }

                // Copy item's box collider and disable our box colliders temporarily
                CopyBoxCollider();
                GetComponents<Collider2D>()[0].enabled = false;

                GetComponent<PlayerMovement>().speed = item.GetComponent<Item>().moveSpeed;
                wooshSFX[0].Play(0);
                playerAnim.Play("possession", 0, 0);
                StartCoroutine(waitForAnim());
                pBox.enabled = false;
            } else if (isPossessed) {
                GetComponent<SpriteRenderer>().sortingOrder = 1;
                isPossessed = false;
                isDepossessing = true;
                GetComponent<PlayerMovement>().speed = 5;
                GetComponents<Collider2D>()[0].isTrigger = false;

                // Re-enable feet collider and destroy item's collider
                GetComponents<Collider2D>()[0].enabled = true;
                Destroy(GetComponents<Collider2D>()[2]);

                playerAnim.enabled = true;
                wooshSFX[0].Play(0);
                playerAnim.Play("depossession", 0, 0);
                itemName = "";
                StartCoroutine(waitForDepossessAnim());
                if (item != null) {
                    Debug.Log("Help");
                    item.transform.position = transform.position;
                    item.SetActive(true);
                } 
                // else {
                //     // clone item
                //     GameObject newItem = (GameObject)Instantiate(itemPrefab, transform.position, itemPrefab.transform.rotation);
                //     newItem.GetComponent<SpriteRenderer>().sprite = itemSprite;
                //     newItem.GetComponent<SpriteRenderer>().color = itemColor;
                //     newItem.layer = itemLayer;
                //     newItem.GetComponent<Item>().moveSpeed = itemSpeed;
                //     newItem.GetComponent<Item>().label = itemName;
                //     newItem.GetComponent<Item>().isKey = itemIsKey;
                // }
                
                AstarPath.active.Scan();
            }
        }
        if (Input.GetKeyDown("o")) {
            if (isPossessed && Time.timeScale != 0) {
                itemPrefab.GetComponent<Item>().StationaryAction();
            } else if (npcInRange && !SaySmt.speaking && SaySmt.prepClose) {
                npc.GetComponent<Npc>().Speak();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Inside item");
        if (other.CompareTag("Item") && isPossessed == false) {
            playerInRange = true;
            item = other.gameObject;
            pBox.enabled = true;
        }
        if (other.CompareTag("NPC")) {
            oBox.enabled = true;
            npc = other.gameObject;
            npcInRange = true;
        }
        if (other.CompareTag("Furniture") || other.CompareTag("Item")) {
            if (transform.position.y > other.transform.position.y) {
                GetComponent<SpriteRenderer>().sortingOrder = -1;
            } else {
                GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Furniture")) {
            if (transform.position.y > other.transform.position.y) {
                GetComponent<SpriteRenderer>().sortingOrder = -1;
            } else {
                GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        Debug.Log("Outside item");
        if (other.CompareTag("Item")) {
            playerInRange = false;
            pBox.enabled = false;
            // item = null;
        }
        if (other.CompareTag("NPC")) {
            oBox.enabled = false;
            npcInRange = false;
            npc = null;
        }
    }

    public void reset() {
        isPossessed = false;
        GetComponent<PlayerMovement>().speed = 5;
        playerSprite.sprite = normalPlayerSprite;
        playerSprite.color = Color.white;
        itemName = "";
        playerAnim.enabled = true;
        playerAnim.SetBool("isDead", false);
        GetComponent<PlayerMovement>().isDead = false;
        playerAnim.Play("Idle", 0);
        if (item && isPossessed)
        {
            item.transform.position = transform.position;
            item.SetActive(true);
        }
        item = null;
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
        itemName = item.GetComponent<Item>().label;
        itemSpeed = item.GetComponent<Item>().moveSpeed;
        itemIsKey = item.GetComponent<Item>().isKey;
        playerSprite.sprite = itemSprite;
        playerSprite.color = itemColor;
        transform.position = item.transform.position;
        item.SetActive(false);
        isPossessing = false;
        yield return null;
    }

    IEnumerator waitForDepossessAnim() {
        yield return new WaitForSeconds(0.5f);
        playerSprite.color = Color.white;
        isDepossessing = false;
        item = null;
        pBox.enabled = true;
        yield return null;
    }
     void CopyBoxCollider() {
        BoxCollider2D collider = this.gameObject.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
        collider.offset = item.GetComponent<BoxCollider2D>().offset;
        collider.size = item.GetComponent<BoxCollider2D>().size;
    }
}
