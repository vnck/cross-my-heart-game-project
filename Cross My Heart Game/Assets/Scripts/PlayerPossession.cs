using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPossession : MonoBehaviour
{
    public bool isPossessed;
    public bool isPossessing;
    public bool isDepossessing;

    // Item attributes
    public GameObject item;
    public GameObject itemPrefab;
    private Sprite itemSprite;
    private int itemLayer;
    public float itemSpeed;
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
    public SpriteRenderer pBox;
    public GameObject oBoxContainer;
    public SpriteRenderer oBox;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("k") && Time.timeScale != 0) {
            if (!isPossessed && playerInRange) {
                if (!isPossessing) { Possess(); }
            } else if (isPossessed) {
                if (!isDepossessing) { Depossess(); }
            }
        }
        if (Input.GetKeyDown("j")) {
            if (isPossessed && Time.timeScale != 0 && item.GetComponent<Item>().moveSpeed == 0) {
                StartCoroutine(WaitForSFX());
                if (item.GetComponent<Item>().label == "locker"){
                    Debug.Log("LOCKERRRR");
                    wooshSFX[3].Play(0);
                }
                else if (item.GetComponent<Item>().label == "laptop"){
                    Debug.Log("LAPTOPPPP");
                    wooshSFX[4].Play(0);
                }
            } else if (npcInRange && !SaySmt.speaking && SaySmt.prepClose) {
                npc.GetComponent<Npc>().Speak();
            }
        }
    }

    public void Possess() {
        isPossessing = true;
        if (item.GetComponent<Item>().moveSpeed == 0) {
            GetComponents<Collider2D>()[0].isTrigger = true;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }

        // Copy item's box collider and disable our box colliders temporarily
        CopyBoxCollider();
        GetComponents<Collider2D>()[0].enabled = false;

        GetComponent<PlayerMovement>().speed = item.GetComponent<Item>().moveSpeed;
        wooshSFX[0].Play(0);
        playerAnim.Play("possession", 0, 0);
        StartCoroutine(WaitForAnim());
        pBox.enabled = false;
    }

    public void Depossess() {
        isDepossessing = true;
        GetComponent<SpriteRenderer>().sortingOrder = 1;
        GetComponent<PlayerMovement>().speed = 5;
        GetComponents<Collider2D>()[0].isTrigger = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        // Re-enable feet collider and destroy item's collider
        GetComponents<Collider2D>()[0].enabled = true;
        Destroy(GetComponents<Collider2D>()[2]);

        playerAnim.enabled = true;
        wooshSFX[0].Play(0);
        playerAnim.Play("depossession", 0, 0);
        itemName = "";
        StartCoroutine(WaitForDepossessAnim());
        if (item != null) {
            item.transform.position = transform.position;
            item.SetActive(true);
        } 
        AstarPath.active.Scan();
    }

    public void PreDeathDepossess() {
        GetComponent<SpriteRenderer>().sortingOrder = 5;
        playerAnim.enabled = true;
        playerSprite.color = Color.white;
        isPossessed = false;
        isDepossessing = false;
        if (item != null) {
            item.transform.position = transform.position;
            item.SetActive(true);
        } 
    }


    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Inside item");
        if (other.CompareTag("Item") && isPossessed == false) {
            playerInRange = true;
            item = other.gameObject;
            pBox.enabled = true;
        }
        if (other.CompareTag("Book") && !other.GetComponent<Book>().carried && isPossessed == false) {
            playerInRange = true;
            item = other.gameObject;
            pBox.enabled = true;
        }
        if (other.CompareTag("NPC")) {
            oBox.enabled = true;
            npc = other.gameObject;
            npcInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        Debug.Log("Outside item");
        if (other.CompareTag("Item") || other.CompareTag("Book")) {
            playerInRange = false;
            pBox.enabled = false;
        }
        if (other.CompareTag("NPC")) {
            oBox.enabled = false;
            npcInRange = false;
            npc = null;
        }
    }

    IEnumerator WaitForAnim() {
        yield return new WaitForSeconds(0.5f);
        FinishPossess();
        yield return null;
    }

    IEnumerator WaitForSFX() {
        yield return new WaitForSeconds(0.5f);
        itemPrefab.GetComponent<Item>().StationaryAction();
    }

    public void FinishPossess() {
        playerAnim.enabled = false;
        itemSprite = item.GetComponent<SpriteRenderer>().sprite;
        itemColor = item.GetComponent<SpriteRenderer>().color;
        itemName = item.GetComponent<Item>().label;
        itemSpeed = item.GetComponent<Item>().moveSpeed;
        itemIsKey = item.GetComponent<Item>().isKey;
        gameObject.layer = item.layer;
        playerSprite.sprite = itemSprite;
        playerSprite.color = itemColor;
        transform.position = item.transform.position;
        item.SetActive(false);
        isPossessed = true;
        isPossessing = false;
        if (item.GetComponent<Item>().moveSpeed == 0) {
            oBox.enabled = true;
        }
    }

    IEnumerator WaitForDepossessAnim() {
        yield return new WaitForSeconds(0.5f);
        FinishDepossess();
        yield return null;
    }

    public void FinishDepossess() {
        playerSprite.color = Color.white;
        oBox.enabled = false;
        if (item != null) { 
            pBox.enabled = true;
            playerInRange = true;
        }
        gameObject.layer = 0;
        isPossessed = false;
        isDepossessing = false;
    }

     void CopyBoxCollider() {
        BoxCollider2D collider = this.gameObject.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
        collider.offset = item.GetComponent<BoxCollider2D>().offset;
        collider.size = item.GetComponent<BoxCollider2D>().size;
        collider.isTrigger = item.GetComponent<BoxCollider2D>().isTrigger;
    }
}
