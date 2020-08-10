using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalLevelManager : MonoBehaviour
{
    int enemyGoal = 8;
    int bookCount;
    public static int booksBurnt;
    public static int booksPlaced;
    GameObject stationaryCultists;
    GameObject mobileCultists;
    GameObject sacrificialBonnie;
    public GameObject player;
    AudioSource[] music;
    public bool gameStarted;
    bool winGame;
    bool loseGame;

    // Start is called before the first frame update
    void Start()
    {
        bookCount = GameObject.FindGameObjectsWithTag("BookPoint").Length;
        music = GetComponents<AudioSource>();
        stationaryCultists = GameObject.Find("StationaryCultists");
        mobileCultists = GameObject.Find("MobileCultists");
        sacrificialBonnie = GameObject.Find("SacrificialBonnie");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameStarted){
            Debug.Log("Books Burnt: " + booksBurnt + ", Books Placed: " + booksPlaced);
            if (player.GetComponent<PlayerMovement>().isDead){
                ResetLevel();
            }
            else if (bookCount - booksBurnt < enemyGoal) {
                WinGame();
            }
            else if (booksPlaced == enemyGoal)
            {
                LoseGame();
            }
        }
        
    }

    void WinGame()
    {
        winGame = true;
        stationaryCultists.SetActive(false);
        mobileCultists.SetActive(false);
        sacrificialBonnie.SetActive(false);
        SaySmt.PrepLine("Cultists", "Noooo we lost!!!");
        SaySmt.PlayLines();
    }

    void LoseGame()
    {
        loseGame = true;
        SaySmt.PrepLine("Cultists", "Hahaha we win!!!");
        SaySmt.PlayLines();
    }

    public void StartLevel()
    {
        music[0].Play(0);
        gameStarted = true;
        GameObject[] cultists = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(var cultist in cultists) {
            CultistMovement cm = cultist.GetComponent<CultistMovement>();
            if (cm != null) {
                cm.StartMoving();
            }
        }
    }

    public void ResetLevel()
    {
        Debug.Log("reset level");
        booksBurnt = 0;
        booksPlaced = 0;
        gameStarted = false;
        stationaryCultists.SetActive(true);
        mobileCultists.SetActive(true);
        sacrificialBonnie.SetActive(true);
    }

    public static void BurnBook()
    {
        Debug.Log("book burnt");
        booksBurnt += 1;
    }

    public static void PlaceBook()
    {
        Debug.Log("book placed");
        booksPlaced += 1;
    }
}
