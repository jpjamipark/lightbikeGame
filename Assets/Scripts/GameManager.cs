using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    bool gameHasEnded = false;
    bool gameIsPaused = false;
    bool gameHasStarted = false;

    public float restartDelay;
    public float gameOverDelay;

    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject startMenu;

    int numberOfPlayers;

    public GameObject winnerText;
    public GameObject players;


    private void Start()
    {
        Time.timeScale = 0f;
        numberOfPlayers = players.transform.childCount;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gameHasStarted)
            {
                if (gameHasEnded)
                {
                    Restart();
                }
                else
                {
                    if (gameIsPaused)
                    {
                    Resume();
                    }
                    else
                    {
                    Pause();
                    }
                }   

            }
            else
            {
                StartCoroutine(StartGame());
            }


        }

    }
    IEnumerator StartGame()
    {
        // start fading
        Time.timeScale = 1f;
        yield return StartCoroutine(CanvasFader.FadeCanvas(startMenu.GetComponent<CanvasGroup>(), 1f, 0f, .5f));

        // code here will run once the fading coroutine has completed
        gameHasStarted = true;
    }

    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("GAME OVER");
            Invoke("activateGameOver", gameOverDelay);
        }
    }
    void activateGameOver()
    {
        if(players.transform.childCount == 1)
        {
            winnerText.GetComponent<TextMeshProUGUI>().SetText(players.transform.GetChild(0).name + " wins!");

            winnerText.GetComponent<TextMeshProUGUI>().color = players.transform.GetChild(0).GetComponent<Move>().lightwallColor;
        }
        else
        {
            winnerText.GetComponent<TextMeshProUGUI>().SetText("Tie!");
        }

        gameOverMenu.SetActive(true);
        StartCoroutine(CanvasFader.FadeCanvas(gameOverMenu.GetComponent<CanvasGroup>(), 0f, 1f, 1f));
        //Time.timeScale = 0.7f;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void RemovePlayer(GameObject x)
    {
        numberOfPlayers--;
        if(numberOfPlayers <= 1)
        {

            EndGame();
        }
    }
}
