using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalLevelManager : MonoBehaviour
{
    int bookCount;
    int booksPlaced;
    public GameObject stationaryCultists;
    public GameObject mobileCultists;
    AudioSource[] music;
    public bool gameStarted;
    bool winGame;
    bool loseGame;

    // Start is called before the first frame update
    void Start()
    {
        music = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bookCount = GameObject.FindGameObjectsWithTag("Book").Length;
        booksPlaced = BookPoint.booksPlaced;
        Debug.Log("Book Count: " + bookCount + ", Books Placed: " + booksPlaced);
        
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
}
