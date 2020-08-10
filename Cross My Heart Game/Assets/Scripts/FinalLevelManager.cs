using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FinalLevelManager : MonoBehaviour
{
    int enemyGoal = 8;
    int bookCount;
    public static int booksBurnt;
    public static int booksPlaced;
    GameObject stationaryCultists;
    GameObject mobileCultists;
    GameObject sacrificialBonnie;
    GameObject director;
    public GameObject player;
    public GameObject camera;
    AudioSource[] music;
    public bool gameStarted;
    public bool winGame;
    public bool loseGame;

    // Start is called before the first frame update
    void Start()
    {
        bookCount = GameObject.FindGameObjectsWithTag("Book").Length;
        music = GetComponents<AudioSource>();
        music[1].Play(0);
        stationaryCultists = GameObject.Find("StationaryCultists");
        mobileCultists = GameObject.Find("MobileCultists");
        sacrificialBonnie = GameObject.Find("SacrificialBonnie");
        director = GameObject.Find("TimelineManager");
        camera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameStarted){
            Debug.Log("Total Books: " + bookCount + ", Books Burnt: " + booksBurnt + ", Books Placed: " + booksPlaced);
            if (player.GetComponent<PlayerMovement>().isDead){
                ResetLevel();
            }
            else if ((bookCount - booksBurnt) < enemyGoal) {
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
        gameStarted = false;
        stationaryCultists.SetActive(false);
        mobileCultists.SetActive(false);
        sacrificialBonnie.SetActive(false);
        music[0].Stop();
        music[1].Stop();
        music[2].Play(0);
        SaySmt.PrepLine("Priests", "Noooo the ritual is ruined!!!");
        SaySmt.PrepLine("Priests", "We will be back.....");
        SaySmt.PlayLines();
        SaySmt.prepClose = true;
    }

    void LoseGame()
    {
        loseGame = true;
        gameStarted = false;
        music[0].Stop();
        music[1].Stop();
        camera.GetComponent<FollowPlayer>().enabled = false;
        camera.transform.position = new Vector3(-3,8,-10);
        player.GetComponent<PlayerMovement>().speed = 0;
        director.GetComponent<PlayableDirector>().Play();
        sacrificialBonnie.SetActive(false);
        SaySmt.PrepLine("Demon", "HAHA!");
        SaySmt.PrepLine("Demon", "I HAVE BEEN SUMMONED!");
        SaySmt.PrepLine("Demon", "DIEEE LITTLE GHOST!");
        SaySmt.PlayLines();
        SaySmt.prepClose = true;
        StartCoroutine(PlayerDeath());
    }

    IEnumerator PlayerDeath() {
        yield return new WaitForSeconds(5f);
        player.GetComponent<PlayerMovement>().gameOverSFX[1].Play(0);
        SaySmt.Line("", "GAME OVER!", true);
        SaySmt.prepClose = true;
    }

    public void StartLevel()
    {
        music[0].Play(1);
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
