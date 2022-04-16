using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{

    float gameTimer = 0;
    float[] endLevelTimer = { 30, 30, 45 };
    int curentSceneNumber = 0;
    bool gameEnding = false;
    Scenes scenes;
    public enum Scenes
    {
        bootUp,
        titile,
        shop,
        level1,
        level2,
        level3,
        gameOver
    }
    // Start is called before the first frame update

    private void Start()
    {
        SceneManager.sceneLoaded += OnsceneLoaded;
    }
    private void Update()
    {
        if(curentSceneNumber != SceneManager.GetActiveScene().buildIndex)
        {
            curentSceneNumber = SceneManager.GetActiveScene().buildIndex;
            GetScene();
        }
        GameTimer();
    }

    public void ResetScene()
    {
        gameTimer = 0;
        SceneManager.LoadScene(GameManager.currentScene);
    }

    public void GameOver()
    {
        SceneManager.LoadScene("gameOver");
        Debug.Log("EndScore: " + GameManager.Instance.GetComponent<ScoreManager>().PlayerScore);
    }

    public void BeginGame(int gameLevel)
    {
        SceneManager.LoadScene(gameLevel);
    }
    
    void GetScene()
    {
        scenes = (Scenes)curentSceneNumber;
    }

    void GameTimer()
    {
        switch (scenes)
        {
            case Scenes.level1: case Scenes.level2: case Scenes.level3:
                if(gameTimer < endLevelTimer[curentSceneNumber - 3])
                {
                    gameTimer += Time.deltaTime;
                }
                else
                {
                    if (!gameEnding)
                    {
                        gameEnding = true;
                        if(SceneManager.GetActiveScene().name!= "level3")
                        {
                            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTransition>().LevelEnds = true;
                        }
                        else
                        {
                            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTransition>().GameCompleted = true;
                        }
                        Invoke("NextLevel", 4);
                    }
                }
                break;
        }
    }

    void NextLevel()
    {
        gameEnding = false;
        gameTimer = 0;
        SceneManager.LoadScene(GameManager.currentScene + 1);
    }

    void OnsceneLoaded(Scene aScene, LoadSceneMode aMode)
    {
        print("loaded");
        GetComponent<GameManager>().SetliveDisplay(GameManager.playerLives);

        if (GameObject.Find("score"))
        {
            GameObject.Find("score").GetComponent<Text>().text =  ScoreManager.playerScore.ToString();
        }
    }

}
