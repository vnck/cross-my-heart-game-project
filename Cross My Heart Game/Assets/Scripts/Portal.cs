using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    // Start is called before the first frame update

    public int sceneIndex;
    private bool playerInRange = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p") && playerInRange) {
            StartCoroutine(waitForAnim());
            SceneManager.LoadScene(sceneIndex);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerInRange = false;
        }
    }

    IEnumerator waitForAnim() {
        yield return new WaitForSeconds(0.5f);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.SetActive(false);
        yield return null;
    }
}
